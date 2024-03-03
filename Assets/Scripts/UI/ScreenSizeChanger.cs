using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

// The orthographic size determines basically the vertical FOV of the game, and the horizontal FOV
// changes depending on the aspect resolution. Because this game is a mobile game designed for a 
// vertical aspect ratio, we actually want the behavior to be the opposite. "Fit to Width" instead of "Fit to Height".
// This thing just updates the orthographic size to be larger or smaller, while maintaining the same width.
[ExecuteInEditMode]
public class ScreenSizeChanger : Singleton<ScreenSizeChanger> {
    public event EventHandler<float> OnSafeAreaYChanged;
    public event EventHandler<float> OnGameHeightChanged;

    private float previousWidth;
    private float previousHeight;

    // 17.76524f is the actual number but 17.25f makes the screen slightly larger for camera shake.
    public const float DEFAULT_GAME_HEIGHT = 17.25f;

    // How many units the width of the game will occupy.
    [SerializeField] private float gameWidth = 10.0f;
    public float gameHeight;

    // When the AR is too short, the game looks weird.
    // If we went vertical-only, this likely wouldn't happen in prod.
    [SerializeField] private float landScapeOrthographicSize = 7.5f;

    protected override void Awake() {
        base.Awake();
        previousWidth = Screen.width;
        previousHeight = Screen.height;
    }

    private void Start() {
        StartCoroutine(LateStart());
    }

    private void Update() {
        if (previousWidth != Screen.width || previousHeight != Screen.height) {
            previousWidth = Screen.width;
            previousHeight = Screen.height;
            ResizePlayArea();
        }
    }

    private void ResizePlayArea() {
        OnSafeAreaYChanged?.Invoke(this, Screen.safeArea.y);
        float aspectRatio = (float)Screen.width / Screen.height;
        gameHeight = gameWidth / aspectRatio;
        OnGameHeightChanged?.Invoke(this, gameHeight);

        if (aspectRatio > 0.75) {
            Camera.main.orthographicSize = landScapeOrthographicSize;
        } else {
            Camera.main.orthographicSize = gameHeight / 2.0f;
        }
    }

    IEnumerator LateStart() {
        yield return new WaitForSeconds(0.1f);
        ResizePlayArea();
    }
}
