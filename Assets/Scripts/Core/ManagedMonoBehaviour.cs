using System;
using UnityEngine;

namespace Core {
    // A ManagedMonoBehaviour does not use the standard Unity update method,
    // instead uses a custom method Process that is manually called.
    public class ManagedMonoBehaviour : MonoBehaviour {
        // Can be used like "Update".
        public virtual void Process() { }

        // Can be used like "Update", except it's called only once in a while.
        public virtual void LowPriorityUpdate() { }
    }
}
