using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;

// Place this component on the same gameobject as a CinemachineClearShot camera to debug it.
// Based on some code shared in this thread: https://forum.unity.com/threads/switch-to-closest-camera.1351976/
// Some tips:
// - Make sure the LookAt target is not in the ground
// - Make sure to ignored the collider attached to the LookAt target
// - Colliders must not be triggers: https://forum.unity.com/threads/cinemachine-clearshot-camera-and-trigger-collider-volumes.596995/
// - Distance to the LookAt is not considered unless "Optimal Target Distance" is used.
//   - Note: Cameras that are more than 3x optimal distance are not prioritized: https://forum.unity.com/threads/switch-to-closest-camera.1351976/
public class ClearShotDebug : MonoBehaviour {
    private CinemachineClearShot _clearShotCamera;
    private CinemachineVirtualCameraBase[] _subCameras;
    private CinemachineCollider _cinemachineCollider;

    public List<List<Vector3>> _debugPaths;

    void Awake() {
        _clearShotCamera = GetComponent<CinemachineClearShot>();
        _cinemachineCollider = GetComponent<CinemachineCollider>();
        _subCameras = _clearShotCamera.ChildCameras;
    }

    private void OnDrawGizmos() {
        if (!Application.isPlaying) {
            return;
        }

        CinemachineVirtualCameraBase closestCamera = null;
        float minDistance = Mathf.Infinity;

        foreach (CinemachineVirtualCameraBase subCamera in _subCameras) {
            float distance;
            if (_cinemachineCollider.IsTargetObscured(subCamera)) {
                Draw(subCamera, Color.red, out distance);
            } else {
                Draw(subCamera, Color.white, out distance);
            }

            if (distance < minDistance) {
                minDistance = distance;
                closestCamera = subCamera;
            }
        }
    }

    private void Draw(CinemachineVirtualCameraBase camera, Color c, out float distance) {
        Vector3 from = camera.LookAt.position;
        Vector3 to = camera.transform.position;
        Vector3 vector = to - from;
        distance = vector.magnitude;
        Vector3 labelPosition = from + vector * 0.5f;
        Draw(from, to, distance, camera.State.ShotQuality, labelPosition, c);
    }

    private void Draw(Vector3 from, Vector3 to, float distance, float shotQuality, Vector3 labelPosition, Color color) {
#if UNITY_EDITOR
        Color cacheColor = UnityEditor.Handles.color;
        UnityEditor.Handles.color = color;
        UnityEditor.Handles.DrawDottedLine(from, to, 4f);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = color;
        UnityEditor.Handles.Label(labelPosition,
            new GUIContent($"Shot Quality: {shotQuality:F2}\nDistance: {distance:F2}"), style);
        UnityEditor.Handles.color = cacheColor;
#endif
    }
}
