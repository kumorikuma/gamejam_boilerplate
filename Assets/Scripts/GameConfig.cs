#nullable enable

using System;
using UnityEngine;

namespace Kumorikuma {
    [Serializable]
    public class GameConfig {
        [Header("Movement")] [Tooltip("Walking controller speed")] [SerializeField]
        public float WalkSpeed = 1.0f;

        [Tooltip("Normal controller speed")] [SerializeField]
        public float RunSpeed = 3.0f;

        [Tooltip("Turning controller speed")] [SerializeField]
        public float TurnSpeed = 360.0f;

        [Tooltip("Force of the jump with which the controller rushes upwards")] [SerializeField]
        public float JumpForce = 1.0f;

        [Tooltip("Gravity, pushing down controller when it jumping")] [SerializeField]
        public float Gravity = -9.81f;
    }
}
