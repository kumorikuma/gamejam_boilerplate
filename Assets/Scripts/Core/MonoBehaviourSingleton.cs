#nullable enable

using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T> {
    private static T _Instance = null!;

    public static T Instance { get { return _Instance; } }

    protected virtual void Awake() {
        if (_Instance != null && _Instance != this) {
            Destroy(this.gameObject);
        } else {
            _Instance = (T)this;
        }
    }
}
