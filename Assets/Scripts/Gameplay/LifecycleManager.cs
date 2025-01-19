#nullable enable

using System;
using Kumorikuma;
using UnityEngine;

public class LifecycleManager : MonoBehaviour {
    private PlayerInputManager _PlayerInputManager;
    
    public enum GameState {
        MainMenu,
        GameStarted,
        GamePaused,
        GameOver,
    }

    public event Action<GameState>? OnGameStateUpdated;
    private GameState _CurrentGameState = GameState.MainMenu;

    public GameState CurrentGameState {
        get { return _CurrentGameState; }
    }

    void Awake() {
        _PlayerInputManager = Main.Instance.PlayerInputManager;
    }

    void Start() {
        _SwitchGameState(_CurrentGameState);
    }

    private void _SwitchGameState(GameState pGameState) {
        switch (pGameState) {
            case GameState.MainMenu:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.None);
                break;
            case GameState.GameStarted:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.Hud);
                // Unpause the game
                Time.timeScale = 1;
                _PlayerInputManager.SwitchToGameplayActions();
                _EnableCursor(false);
                break;
            case GameState.GamePaused:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.PauseMenu);
                // Pause the Game
                Time.timeScale = 0;
                _PlayerInputManager.SwitchToMenuActions();
                _EnableCursor(true);
                break;
            case GameState.GameOver:
                UIRouter.Instance.SwitchRoutes(UIRouter.Route.GameOver);
                break;
        }

        _CurrentGameState = pGameState;
        OnGameStateUpdated?.Invoke(_CurrentGameState);
    }

    private void _EnableCursor(bool pEnableCursor) {
        if (pEnableCursor) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void StartGame() {
        _SwitchGameState(GameState.GameStarted);
    }

    public void EndGame() {
        _SwitchGameState(GameState.GameOver);
    }

    public void PauseGame() {
        _SwitchGameState(GameState.GamePaused);
    }

    public void UnpauseGame() {
        _SwitchGameState(GameState.GameStarted);
    }

    public void ReturnToMainMenu() {
        _SwitchGameState(GameState.MainMenu);
    }
}
