using UnityEngine;

public static class AnimatorExtensions {
    public static StateMachineEvents GetEvents(this Animator animator) {
        // Retrieve the behaviours attached to the current state and filter by the desired type.
        StateMachineEvents behaviour = animator.GetBehaviour<StateMachineEvents>();

        // Assuming you only have one StateMachineEvents behaviour on that state:
        if (behaviour == null) {
            Debug.Log("[Animator] Failed to find StateMachineEvents behavior on Animator Controller. Add StateMachineEvents behavior to the base layer to use this.");
        }

        return behaviour;
    }
}
