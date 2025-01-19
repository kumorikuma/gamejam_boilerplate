using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

// This editor script listens for the `playModeStateChanged` event and checks all MonoBehaviour instances for fields marked with the `[NonNullField]` attribute. 
// If any of these fields are null, it logs an error message in the console with the relevant GameObject and field name.
[InitializeOnLoad]
public class NonNullFieldChecker {
    static NonNullFieldChecker() {
        EditorApplication.playModeStateChanged += _CheckNonNullFields;
    }

    private static void _CheckNonNullFields(PlayModeStateChange pState) {
        if (pState != PlayModeStateChange.EnteredPlayMode) {
            return;
        }

        // Time how long this takes so we're aware of any PlayMode slowdowns.
        Stopwatch stopwatch = Stopwatch.StartNew();

        MonoBehaviour[] allMonoBehaviours = GameObject.FindObjectsOfType<MonoBehaviour>();
        foreach (var monoBehaviour in allMonoBehaviours) {
            IEnumerable<FieldInfo> nonNullFields = _GetNonNullFields(monoBehaviour.GetType());

            foreach (var field in nonNullFields) {
                object fieldValue = field.GetValue(monoBehaviour);
                if (fieldValue == null) {
                    Debug.LogError(
                        $"[NonNullChecker] {monoBehaviour.name}: {field.Name} is not assigned a valid value!",
                        monoBehaviour);
                }
            }
        }

        // Stop timing and print the elapsed time
        stopwatch.Stop();
        Debug.Log($"[NonNullFieldChecker] CheckNonNullFields took {stopwatch.ElapsedMilliseconds} ms");
    }

    private static IEnumerable<FieldInfo> _GetNonNullFields(System.Type pType) {
        FieldInfo[] fields = pType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        List<FieldInfo> nonNullFields = new List<FieldInfo>();

        foreach (var field in fields) {
            if (field.GetCustomAttribute(typeof(NonNullFieldAttribute)) != null) {
                nonNullFields.Add(field);
            }
        }

        return nonNullFields;
    }
}
