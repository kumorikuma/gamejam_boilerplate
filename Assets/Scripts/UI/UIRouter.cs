using System;
using UnityEngine;

public class UIRouter : Singleton<UIRouter> {
    public enum Route {
        None,
        MainMenu,
        PauseMenu,
        Hud,
        GameOver,
    }

    public Route DebugRoute = Route.None;

    public event EventHandler<string> OnRouteUpdate;

    public void SwitchRoutes(Route routeName) {
        OnRouteUpdate(this, RouteNameToPath(routeName));
    }

    void OnValidate() {
        if (!Application.isPlaying) { return; }

        OnRouteUpdate?.Invoke(this, RouteNameToPath(DebugRoute));
    }

    string RouteNameToPath(Route routeName) {
        string routePath = "";
        switch (routeName) {
            case Route.None:
                routePath = "/";
                break;
            case Route.MainMenu:
                routePath = "/mainMenu";
                break;
            case Route.PauseMenu:
                routePath = "/pauseMenu";
                break;
            case Route.Hud:
                routePath = "/hud";
                break;
            case Route.GameOver:
                routePath = "/gameOver";
                break;
        }

        return routePath;
    }
}
