using System;
using System.Collections;
using System.Collections.Generic;
using RhythmJam;
using UnityEngine;

public class GameLifecycleManager : Singleton<GameLifecycleManager>
{
    public enum GameState
    {
        MainMenu,
        Instructions,
        GameStarted,
        GamePaused,
        GameOver,
        Leaderboard,
    }

    public enum GameType
    {
        SpacePop,
        OldManRave,
    }

    public event EventHandler<GameState> OnGameStateUpdated;
    public event EventHandler<int> OnScoreUpdated;
    public event EventHandler<string> OnStatusUpdated;

    private GameState _currentGameState = GameState.MainMenu;

    [NonNullField] public OldManLevelController OldManLevelController;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
    }

    private GameType _currentGameType = GameType.SpacePop;

    public GameType CurrentGameType
    {
        get { return _currentGameType; }
    }

    private int _score = 0;
    public int Score
    {
        get { return _score; }
    }

    private string _status = "GameOver";
    public string Status
    {
        get { return _status; }
    }

    private bool _canContinue = true;
    public bool CanContinue
    {
        get { return _canContinue; }
    }

    private void SwitchGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.MainMenu);
                LevelManager.Instance.DisableLevel();
                break;
            case GameState.Instructions:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.Instructions);
                break;
            case GameState.GameStarted:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.Hud);
                LevelManager.Instance.SwitchLevels(_currentGameType);
                CallResponseGameplayManager.Instance.Play();
                // Unpause the game
                Time.timeScale = 1;
                break;
            case GameState.GamePaused:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.PauseMenu);
                // Pause the game
                Time.timeScale = 0;
                // PlayerManager.Instance.SwitchActionMaps("menu");
                break;
            case GameState.GameOver:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.GameOver);
                break;
            case GameState.Leaderboard:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.Leaderboard);
                break;
        }

        _currentGameState = gameState;
        OnGameStateUpdated?.Invoke(this, _currentGameState);
    }

    public void SetScore(int score)
    {
        _score = score;
        OnScoreUpdated?.Invoke(this, _score);
    }

    public void SetStatus(string status)
    {
        _status = status;
        OnStatusUpdated?.Invoke(this, _status);
    }

    [JsCallable]
    public void StartGame(GameType gameType, bool resetScore = true)
    {
        if (resetScore)
        {
            SetScore(0);
        }
        _currentGameType = gameType;
        if (_currentGameType == GameType.OldManRave)
        {
            OldManLevelController.Initialize();
        }
        CallResponseGameplayManager.Instance.Initialize(_currentGameType);
        SwitchGameState(GameState.GameStarted);
    }

    [JsCallable]
    public void PauseGame()
    {
        SwitchGameState(GameState.GamePaused);
    }

    [JsCallable]
    public void UnpauseGame()
    {
        SwitchGameState(GameState.GameStarted);
    }

    [JsCallable]
    public void ReturnToMainMenu()
    {
        SwitchGameState(GameState.MainMenu);
    }

    [JsCallable]
    public void EndLevel()
    {
        if (_currentGameState == GameState.GameOver)
        {
            return;
        }
        
        _canContinue = _currentGameType != GameType.OldManRave; 
        SwitchGameState(GameState.GameOver);
        // TODO: Stop RhythmEngine if its not stopped.
    }
    
    [JsCallable]
    public void ToLeaderboard()
    {
        SwitchGameState(GameState.Leaderboard);
    }

    [JsCallable]
    public void ToNextLevel()
    {
        StartGame(GameType.OldManRave, false);
    }

    void Start()
    {
        SwitchGameState(_currentGameState);
    }
}
