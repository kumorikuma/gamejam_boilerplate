using System;
using UnityEngine;

public class GameLifecycleManager : Singleton<GameLifecycleManager> {
    public enum GameState {
        MainMenu,
        GameStarted,
        GamePaused,
        GameOver,
    }

    public event EventHandler<GameState> OnGameStateUpdated;
    private GameState _currentGameState = GameState.MainMenu;

    public GameState CurrentGameState {
        get { return _currentGameState; }
    }

    void Start() {
        SwitchGameState(_currentGameState);
    }

    private void SwitchGameState(GameState gameState) {
        switch (gameState) {
            case GameState.MainMenu:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.MainMenu);
                break;
            case GameState.GameStarted:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.Hud);
                // Unpause the game
                Time.timeScale = 1;
                PlayerManager.Instance.SwitchActionMaps("gameplay");
                ToggleCursor(false);
                break;
            case GameState.GamePaused:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.PauseMenu);
                // Pause the Game
                Time.timeScale = 0;
                PlayerManager.Instance.SwitchActionMaps("menu");
                ToggleCursor(true);
                break;
            case GameState.GameOver:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.GameOver);
                break;
        }

        _currentGameState = gameState;
        OnGameStateUpdated?.Invoke(this, _currentGameState);
    }

    private void ToggleCursor(bool enableCursor) {
        if (enableCursor) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void StartGame() {
        SwitchGameState(GameState.GameStarted);
    }

    public void EndGame() {
        SwitchGameState(GameState.GameOver);
    }

    public void PauseGame() {
        SwitchGameState(GameState.GamePaused);
    }

    public void UnpauseGame() {
        SwitchGameState(GameState.GameStarted);
    }

    public void ReturnToMainMenu() {
        SwitchGameState(GameState.MainMenu);
    }
}
