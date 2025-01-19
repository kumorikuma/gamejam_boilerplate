#nullable enable

using System;
using Core;
using Kumorikuma.Utilities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Kumorikuma.Gameplay {
    public class GameDirector : ManagedMonoBehaviour {
        private LifecycleManager _LifecycleManager = null!;
        private PlayerInputManager _PlayerInputManager = null!;

        [NonSerialized] public Action<Actor>? OnActorSpawned;
        [NonSerialized] public Action<Actor>? OnActorDestroyed;

        [ShowInInspector] [ReadOnly] [NonSerialized]
        private IntActorSerializableDictionary _ActorReferences = new();

        [ShowInInspector] [ReadOnly] [NonSerialized]
        private IntPlayerSerializableDictionary _PlayerReferences = new();

        void Awake() {
            _LifecycleManager = Main.Instance.LifecycleManager;
            _PlayerInputManager = Main.Instance.PlayerInputManager;

            OnActorSpawned += _OnActorSpawned;
            OnActorDestroyed += _OnActorDestroyed;
        }

        public override void Process() {
            if (_PlayerInputManager.Inputs.GameplayPauseInputAction.WasPerformedThisFrame()) {
                _LifecycleManager.PauseGame();
            } else if (_PlayerInputManager.Inputs.MenuPauseInputAction.WasPerformedThisFrame()) {
                _LifecycleManager.UnpauseGame();
            }

            foreach (Player player in _PlayerReferences.Values) {
                player.PlayerController.Process();
            }
        }

        private void _OnActorSpawned(Actor pActor) {
            if (pActor is Player player) {
                _PlayerReferences.Add(pActor.GetInstanceID(), player);
            } else {
                _ActorReferences.Add(pActor.GetInstanceID(), pActor);
            }
        }

        private void _OnActorDestroyed(Actor pActor) {
            if (pActor is Player) {
                _PlayerReferences.Remove(pActor.GetInstanceID());
            } else {
                _ActorReferences.Remove(pActor.GetInstanceID());
            }
        }
    }
}
