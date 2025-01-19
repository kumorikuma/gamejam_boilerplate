#nullable enable

using System;
using Core;
using Kumorikuma;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

// This is meant to be put onto the same object as Player Input component, to receive messages.
// TODO: PlayerInputReceiver should get these sorts of values from settings, and also adapt to when the settings change.
// TODO: How can we support local multi-player?
[RequireComponent(typeof(PlayerInput))]
public class PlayerInputManager : ManagedMonoBehaviour {
    private const string GAMEPLAY_ACTION_MAP = "Gameplay";
    private const string MENU_ACTION_MAP = "Menu";

    private PlayerInput _PlayerInput = null!;
    [NonSerialized] public ActionReferences Inputs = null!;

    [ShowInInspector] [ReadOnly] public Vector3 _MoveVector = Vector3.zero;
    [ShowInInspector] [ReadOnly] public Vector3 _LookVector = Vector3.zero;

    void Awake() {
        _PlayerInput = this.gameObject.GetComponent_ThrowIfNull<PlayerInput>();
        Inputs = new ActionReferences(Main.Instance.UserSettings);

        Inputs.MoveInputAction = _PlayerInput.actions.FindAction($"{GAMEPLAY_ACTION_MAP}/Move", true);
        Inputs.MouseLookInputAction = _PlayerInput.actions.FindAction($"{GAMEPLAY_ACTION_MAP}/MouseLook", true);
        Inputs.JoystickLookInputAction = _PlayerInput.actions.FindAction($"{GAMEPLAY_ACTION_MAP}/JoystickLook", true);
        Inputs.JumpInputAction = _PlayerInput.actions.FindAction($"{GAMEPLAY_ACTION_MAP}/Jump", true);
        Inputs.WalkInputAction = _PlayerInput.actions.FindAction($"{GAMEPLAY_ACTION_MAP}/Walk", true);
        Inputs.GameplayPauseInputAction = _PlayerInput.actions.FindAction($"{GAMEPLAY_ACTION_MAP}/Pause", true);

        Inputs.MenuMoveInputAction = _PlayerInput.actions.FindAction($"{MENU_ACTION_MAP}/Move", true);
        Inputs.MenuPauseInputAction = _PlayerInput.actions.FindAction($"{MENU_ACTION_MAP}/Pause", true);
    }

    public override void LowPriorityUpdate() {
        _MoveVector = Inputs.MoveVector3d;
        _LookVector = Inputs.LookVector;
    }

    private void _SwitchActionMaps(string pActionMapName) {
        _PlayerInput.SwitchCurrentActionMap(pActionMapName);
    }

    public void SwitchToGameplayActions() {
        _SwitchActionMaps(GAMEPLAY_ACTION_MAP);
    }

    public void SwitchToMenuActions() {
        _SwitchActionMaps(MENU_ACTION_MAP);
    }
}

public class ActionReferences {
    private UserSettings _UserSettings;

    // Gameplay Actions
    public InputAction MoveInputAction = null!;
    public InputAction MouseLookInputAction = null!;
    public InputAction JoystickLookInputAction = null!;
    public InputAction JumpInputAction = null!;
    public InputAction WalkInputAction = null!;
    public InputAction GameplayPauseInputAction = null!;

    // Menu Actions
    public InputAction MenuMoveInputAction = null!;
    public InputAction MenuPauseInputAction = null!;

    public Vector2 MoveVector2d { get { return MoveInputAction.ReadValue<Vector2>(); } }
    public Vector3 MoveVector3d { get { return new Vector3(MoveVector2d.x, 0, MoveVector2d.y); } }

    public Vector2 LookVector {
        get {
            // Mouse look takes priority over joystick look
            Vector2 mouseLookVector = MouseLookInputAction.ReadValue<Vector2>() * _UserSettings.MouseLookSensitivity;
            if (mouseLookVector.magnitude > 0) {
                return mouseLookVector;
            }

            return JoystickLookInputAction.ReadValue<Vector2>() * _UserSettings.JoystickLookSensitivity;
        }
    }

    public ActionReferences(UserSettings pUserSettings) {
        _UserSettings = pUserSettings;
    }
}
