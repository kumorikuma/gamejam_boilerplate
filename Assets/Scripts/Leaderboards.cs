
using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using Unity.Services.Leaderboards.Exceptions;

// Handles both keeping track of highest local score (for offline situations),
// and also using online Unity leaderboards.
public class Leaderboards : Singleton<Leaderboards>
{
    private const string LEADERBOARD_ID = "Global";
    public const string PREFS_BEST_SCORE = "Best_Score";

    public struct LeaderboardScores
    {
        public int LatestScore;
        public int LatestRanking;
        public int BestScore;
        public int BestRanking;
        public int GlobalBestScore;
        public int NumberScores;
    }
    
    public event EventHandler<LeaderboardScores> OnLeaderboardScoresUpdated;
    private LeaderboardScores _scores = new();

    async void Start()
    {
        // Initialize UGS
        await UnityServices.InitializeAsync();
        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in as: " + AuthenticationService.Instance.PlayerId);
        };
        AuthenticationService.Instance.SignInFailed += s => {
            // Take some action here...
            Debug.LogError("Sign in failed!");
        };
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        
        LoadLeaderboardAsync();
    }
    
    private int LoadCachedBestScore() {
        return PlayerPrefs.GetInt(PREFS_BEST_SCORE, 0);
    }
    
    public async Task LoadLeaderboardAsync() {
        // Pull data from local cache as a fallback first. 
        _scores.BestScore = LoadCachedBestScore();

        // If the user is not signed in- try signing in first.
        if (!AuthenticationService.Instance.IsSignedIn) {
            try {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            } catch (Exception)
            {
                Debug.LogError("[Leaderboards::LoadLeaderboardAsync] Anon sign-in failed!");
            }
        }

        // NOTE: Awaits need to wrapped in try-catch otherwise if they throw an exception they will just not return.
        Task loadGlobalScoreTask = LoadGlobalScoreAsync();
        Task loadPlayerScoreTask = LoadPlayerScoreAsync();
        try {
            await Task.WhenAll(loadGlobalScoreTask, loadPlayerScoreTask);
        } catch (Exception) { }
    }
    
    public async Task SubmitScoreToLeaderboard(int score)
    {
        Debug.Log("[Leaderboards::SubmitScoreToLeaderboard] Score " + score);
        // Pull data from local cache as a fallback first.
        // This will get us:
        // - [Fallback] Best Score: Sets to the highest score this device has known in the past. It's possible that a
        // better score exists on the leaderboard that this device doesn't know about, or that latest score is higher.
        // - [unset] Best Rank: Sets to 0 because it's out of date now.
        _scores.BestScore = LoadCachedBestScore();
        _scores.BestRanking = 0;

        // If the user is not signed in- try signing in first.
        if (!AuthenticationService.Instance.IsSignedIn) {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (Exception)
            {
                Debug.LogError("[Leaderboards::SubmitScoreToLeaderboard] Anon sign-in failed!");
            }
            Debug.Log("[Leaderboards::SubmitScoreToLeaderboard] Signed in! ID: " + AuthenticationService.Instance.PlayerId);
        }

        // Update latest score. This gets us:
        // - [Done] Latest Score
        // - [unset] Latest Rank: Sets to 0 because we don't know what the rank is w/o internet connection.
        _scores.LatestScore = score;
        _scores.LatestRanking = 0;
        
        // Update best score based on the latest score.
        // This doesn't depend on network so if the rest of the leaderboard stuff fails at least we'll have the best scores updated.
        // It could still be incorrect because there might be a better score on the leaderboard.
        // - [updated] Best Score
        if (_scores.LatestScore > _scores.BestScore)
        {
            _scores.BestScore = _scores.LatestScore;
        }
        
        // Update UI here as any of the future await calls to Unity Game Services (UGS) may never finish if no internet.
        OnLeaderboardScoresUpdated?.Invoke(this, _scores);

        // We'll first get the scores from the leaderboard. This is the best score that the server has seen from us.
        // We need to cache it now because we need to overwrite our score on the server in order to get it to give us
        // a current rank. Then need to re-submit to the server.
        try {
            LeaderboardEntry playerScoreResponse = await LeaderboardsService.Instance
                .GetPlayerScoreAsync(
                    LEADERBOARD_ID,
                    new GetPlayerScoreOptions { IncludeMetadata = true });
            int networkBestScore = (int)playerScoreResponse.Score;
            int networkBestRank = playerScoreResponse.Rank + 1; // Rank is 0 based

            // At this point we know definitively the best score.
            // - [Done] Best Score
            // - [updated] Best Rank: If our latest score was better, or cached score was better, we don't know the rank.
            if (networkBestScore > _scores.BestScore) {
                // If the network score is better than our score, update the best score one last time.
                _scores.BestScore = networkBestScore;
                _scores.BestRanking = networkBestRank;
            }
        } catch (Exception) {
            // This happens when the user doesn't have a score yet.
        }
        
        // In order to get a ranking for our latest score, we need to submit our latest score to the server.
        // - [Done] Latest rank
        try
        {
            LeaderboardEntry scoreResponse =
                await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID,
                    _scores.LatestScore);
            _scores.LatestRanking = scoreResponse.Rank + 1;
        }
        catch (Exception)
        {
            Debug.LogError("[Leaderboards::SubmitScoreToLeaderboard] Error submitting latest score!");
        }
        
        // Now that we know our best score, and we've gotten our latest rank, we can resubmit to the server.
        // This will let the server know our best score so far, and we'll also get an up to date best rank.
        // - [Done] Best rank
        try
        {
            LeaderboardEntry scoreResponse =
                await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID,
                    _scores.BestScore);
            _scores.BestRanking = scoreResponse.Rank + 1;
        }
        catch (Exception)
        {
            Debug.LogError("[Leaderboards::SubmitScoreToLeaderboard] Error submitting best score!");
        }
        
        // At this point we'll update the UI and cache the data.
        OnLeaderboardScoresUpdated?.Invoke(this, _scores);
        PlayerPrefs.SetInt(PREFS_BEST_SCORE, _scores.BestScore);
        PlayerPrefs.Save();
        
        Debug.Log("[Leaderboards::SubmitScoreToLeaderboard] Finished submitting!");
        
        // Finally, unrelated we can get the global score. This updates the UI by itself.
        // - [Done] Global score
        await LoadGlobalScoreAsync();
    }

    private async Task LoadGlobalScoreAsync() {
        LeaderboardScoresPage scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(
            LEADERBOARD_ID,
            new GetScoresOptions { Offset = 0, Limit = 1 }
        );
        _scores.NumberScores = scoresResponse.Total;
        if (scoresResponse.Results.Count == 0) {
            _scores.GlobalBestScore = 0;
        } else {
            _scores.GlobalBestScore = (int)scoresResponse.Results[0].Score;
        }

        OnLeaderboardScoresUpdated?.Invoke(this, _scores);
    }
    
    private async Task LoadPlayerScoreAsync() {
        int networkScore = 0;
        int networkRank = 0;

        try {
            LeaderboardEntry playerScoreResponse = await LeaderboardsService.Instance
                .GetPlayerScoreAsync(
                    LEADERBOARD_ID,
                    new GetPlayerScoreOptions { IncludeMetadata = true });
            networkScore = (int)playerScoreResponse.Score;
            networkRank = playerScoreResponse.Rank + 1; // Rank is 0 based
        } catch (Exception) {
            // This can happen if there's either no internet connection,
            // or the player doesn't have a network score yet.
        }
        
        int cachedBestScore = LoadCachedBestScore();
        if (networkScore >= cachedBestScore) {
            // Network score was better, update the cached score.
            // Or the cached score was invalid (for a different day).
            _scores.BestScore = networkScore;
            _scores.BestRanking = networkRank;
            PlayerPrefs.SetInt(PREFS_BEST_SCORE, networkScore);
            PlayerPrefs.Save();
        } else if (cachedBestScore > networkScore) {
            _scores.BestScore = cachedBestScore;
            // Cached score was better (and valid), need to get an up to date ranking and submit the cached score to leaderboard.
            try {
                LeaderboardEntry scoreResponse =
                    await LeaderboardsService.Instance.AddPlayerScoreAsync(LEADERBOARD_ID, cachedBestScore);
                _scores.BestRanking = scoreResponse.Rank + 1;
            } catch (Exception) { }
        }

        OnLeaderboardScoresUpdated?.Invoke(this, _scores);
    }
}
