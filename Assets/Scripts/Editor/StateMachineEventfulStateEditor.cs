using Sirenix.OdinInspector.Editor;
using UnityEditor;

[CustomEditor(typeof(StateMachineEventfulState))]
public class StateMachineEventfulStateEditor : OdinEditor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        StateMachineEventfulState state = (StateMachineEventfulState)target;
        foreach (var entry in state.parameterUpdates) {
            if (entry.eventfulState == null) {
                entry.eventfulState = state;
            }
        }
    }
}
