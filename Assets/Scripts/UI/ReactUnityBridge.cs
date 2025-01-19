using Kumorikuma;
using ReactUnity;
using ReactUnity.Reactive;
using UnityEngine;

public class ReactUnityBridge : MonoBehaviour {
    private ReactiveValue<string> _UiRoute;
    private ReactiveValue<bool> _DebugModeEnabled;
    private ReactiveValue<string> _DebugGameState;
    private ReactiveValue<Leaderboards.LeaderboardScores> _LeaderboardScores;

    private ReactRendererBase reactRenderer;

    void Awake() {
        reactRenderer = GetComponentInChildren<ReactUnity.UGUI.ReactRendererUGUI>();

        _UiRoute = new ReactiveValue<string>();
        _DebugModeEnabled = new ReactiveValue<bool>();
        _DebugGameState = new ReactiveValue<string>();
        _LeaderboardScores = new ReactiveValue<Leaderboards.LeaderboardScores>();

        // Routing
        reactRenderer.Globals["route"] = _UiRoute;
        reactRenderer.Globals["leaderboardScores"] = _LeaderboardScores;

        // Debug values
        reactRenderer.Globals["debugGameState"] = _DebugGameState;
        reactRenderer.Globals["debugModeEnabled"] = _DebugModeEnabled;

        // Enable Debug Mode when in Unity Editor
        _DebugModeEnabled.Value = false;
#if UNITY_EDITOR
        _DebugModeEnabled.Value = true;
#endif

        // Singletons become available after Awake. ScriptExecutionOrder should make sure this is executed last.
        UIRouter.Instance.OnRouteUpdate += _OnRouteUpdate;
        Main.Instance.LifecycleManager.OnGameStateUpdated += _OnGameStateUpdated;
        if (Leaderboards.Instance != null) {
            Leaderboards.Instance.OnLeaderboardScoresUpdated += _OnLeaderboardScoresUpdated;
            // To enable leaderboards, need to connect to a Unity Project and add the Leaderboards singleton to the game.
        }

        // Game System References   
        reactRenderer.Globals["gameLifecycleManager"] = Main.Instance.LifecycleManager;
    }

    private void _OnLeaderboardScoresUpdated(object _, Leaderboards.LeaderboardScores pLeaderboardScores) {
        _LeaderboardScores.Value = pLeaderboardScores;
    }

    private void _OnRouteUpdate(object _, string pUiRoute) {
        _UiRoute.Value = pUiRoute;
    }

    private void _OnGameStateUpdated(LifecycleManager.GameState pGameState) {
        _DebugGameState.Value = pGameState.ToString();
    }
}
