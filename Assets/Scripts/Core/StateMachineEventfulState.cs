using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEditor;
using UnityEditor.Animations;

public class StateMachineEventfulState : StateMachineBehaviour {
#if UNITY_EDITOR
    // The condition upon which the animation parameter is updated.
    public enum Condition {
        StateEntered,
        StateExited
    }

    // Populated at editor time.
    [HideInInspector] public Dictionary<string, AnimatorControllerParameter> AnimatorParameters = new();

    [Serializable]
    public class AnimatorParameterEntry {
        [HideInInspector] public StateMachineEventfulState eventfulState;

        [ValueDropdown("ParameterNames")] [OnValueChanged("OnParameterSelected")]
        public string Parameter;

        // This method will be called by Odin to get the dropdown values
        private IEnumerable<string> ParameterNames() {
            if (eventfulState != null && eventfulState.AnimatorParameters != null) {
                return eventfulState.AnimatorParameters.Keys;
            }

            return new List<string>();
        }

        // This method is called whenever SelectedParameterName changes.
        private void OnParameterSelected() {
            AnimatorControllerParameter param = eventfulState.AnimatorParameters[Parameter];
            Type = param.type;
        }

        public Condition condition;

        [ReadOnly] public AnimatorControllerParameterType Type;

        [ShowIf("Type", AnimatorControllerParameterType.Float)]
        public float FloatValue;

        [ShowIf("Type", AnimatorControllerParameterType.Bool)]
        public bool BoolValue;

        [ShowIf("Type", AnimatorControllerParameterType.Int)]
        public int IntValue;
    }

    private void OnValidate() {
        // See: https://forum.unity.com/threads/editor-not-runtime-how-to-get-animatorcontroller-reference-within-statemachinebehaviour.1371780/
        string path = AssetDatabase.GetAssetPath(this);
        AnimatorController animatorController = AssetDatabase.LoadMainAssetAtPath(path) as AnimatorController;
        AnimatorControllerParameter[] parameters = animatorController.parameters;

        AnimatorParameters.Clear();
        foreach (AnimatorControllerParameter parameter in parameters) {
            AnimatorParameters[parameter.name] = parameter;
        }
    }
#endif

    public AnimatorParameterEntry[] parameterUpdates;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        UpdateEventfulState(animator, Condition.StateEntered);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        UpdateEventfulState(animator, Condition.StateExited);
    }

    private void UpdateEventfulState(Animator animator, Condition condition) {
        bool eventfulStateUpdated = false;
        foreach (AnimatorParameterEntry entry in parameterUpdates) {
            if (entry.condition != condition || entry.Parameter == null || entry.Parameter == "") {
                continue;
            }

            switch (entry.Type) {
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(entry.Parameter, entry.BoolValue);
                    break;
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(entry.Parameter, entry.FloatValue);
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(entry.Parameter, entry.IntValue);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    animator.SetTrigger(entry.Parameter);
                    break;
            }

            eventfulStateUpdated = true;
        }

        if (eventfulStateUpdated) {
            animator.GetEvents().OnEventfulStateUpdated();
        }
    }
}
