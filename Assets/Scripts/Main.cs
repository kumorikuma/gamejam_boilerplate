#nullable enable

using System;
using Kumorikuma.Gameplay;
using UnityEngine;

namespace Kumorikuma {
    public class Main : MonoBehaviourSingleton<Main> {
        // Note: Non-nullable types cannot be initialized in Awake (only in constructor), but it's unadvisable to use
        // constructors with MonoBehaviours. So we set them to "null!" to suppress the warning.
        // We opt to use MonoBehaviours instead of regular C# classes because of the ease of debugging by exposing fields
        // to the inspector. However, it is not advised to rely on the Update method. It's more predictable to manually
        // invoke a "Process" method every frame instead.

        #region Application Level Services

        // Contains all application level MonoBehaviour services.
        // This container is not destroyed and persisted across scenes.
        private GameObject _AppServicesContainer = null!;

        [NonSerialized] public LifecycleManager LifecycleManager = null!;
        [NonSerialized] public PlayerInputManager PlayerInputManager = null!;
        [NonSerialized] public GameDirector GameDirector = null!;

        public GameConfig GameConfig = new();
        public UserSettings DefaultUserSettings = new();
        [NonSerialized] public UserSettings UserSettings;

        #endregion

        private float _LowPriorityUpdateTimer = 0.0f;

        // Entrypoint into the application (aka main function)
        // This is guaranteed to be executed before our other scripts in the Script Execution Order.
        // ScriptExecutionOrder is set in: Edit -> Project Settings -> ScriptExecutionOrder
        // See: https://docs.unity3d.com/Manual/class-MonoManager.html
        protected override void Awake() {
            base.Awake();

            // The object that Main lives on will be considered the application services container.
            // Make sure it doesn't get destroyed across scene loads.
            _AppServicesContainer = this.gameObject;
            DontDestroyOnLoad(_AppServicesContainer);

            // Set all app service references.
            LifecycleManager = _AppServicesContainer.GetComponentInChildren_ThrowIfNull<LifecycleManager>();
            GameDirector = _AppServicesContainer.GetComponentInChildren_ThrowIfNull<GameDirector>();
            PlayerInputManager = _AppServicesContainer.GetComponentInChildren_ThrowIfNull<PlayerInputManager>();

            // Load configuration
            // TODO: Load from player preferences.
            UserSettings = DefaultUserSettings;
        }

        void Update() {
            GameDirector.Process();

            // For things that don't need realtime updating.
            _LowPriorityUpdateTimer += Time.deltaTime;
            if (_LowPriorityUpdateTimer > 0.25f) {
                _LowPriorityUpdateTimer = 0.0f;

                PlayerInputManager.LowPriorityUpdate();
            }
        }

        #region Dumping Ground - Code that should probably be moved out sooner or later

        #endregion
    }
}
