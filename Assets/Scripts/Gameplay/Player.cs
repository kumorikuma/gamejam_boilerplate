#nullable enable

using System;
using UnityEngine;

namespace Kumorikuma.Gameplay {
    [RequireComponent(typeof(PlayerController))]
    public class Player : Actor {
        [NonSerialized] public PlayerController PlayerController = null!;

        protected override void Awake() {
            base.Awake();

            PlayerController = this.gameObject.GetComponent_ThrowIfNull<PlayerController>();
        }
    }
}
