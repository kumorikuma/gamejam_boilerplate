using System;
using UnityEngine;

public class StateMachineEvents : StateMachineBehaviour {
    public delegate void StateUpdatedHandler(int currentStateHash, int previousStateHash);

    public event StateUpdatedHandler StateUpdated;

    public delegate void EventfulStateUpdatedHandler();

    public event EventfulStateUpdatedHandler EventfulStateUpdated;

    public delegate void OnDestroyHandler();

    public event EventfulStateUpdatedHandler Destroyed;

    private int _previousStateHash = 0;

    public void OnDestroy() {
        Destroyed?.Invoke();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        StateUpdated?.Invoke(stateInfo.fullPathHash, _previousStateHash);
        _previousStateHash = stateInfo.fullPathHash;
    }

    public void OnEventfulStateUpdated() {
        Debug.Log("OnEventfulStateUpdated");
        if (EventfulStateUpdated == null) {
            Debug.Log("No listeners!");
        }

        EventfulStateUpdated?.Invoke();
    }
}
