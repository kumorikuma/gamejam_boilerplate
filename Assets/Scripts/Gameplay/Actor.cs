#nullable enable

using UnityEngine;

namespace Kumorikuma.Gameplay {
    public class Actor : MonoBehaviour {
        protected GameDirector _GameDirector = null!;

        protected virtual void Awake() {
            _GameDirector = Main.Instance.GameDirector;
        }

        void Start() {
            _GameDirector.OnActorSpawned?.Invoke(this);
            Debug.Log("MOOO");
        }

        private void OnDestroy() {
            _GameDirector.OnActorDestroyed?.Invoke(this);
        }
    }
}
