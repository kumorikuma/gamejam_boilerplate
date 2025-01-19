using System.Collections.Generic;
using UnityEngine;

namespace Kumorikuma.Utilities {
    public abstract class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
        ISerializationCallbackReceiver {
        [SerializeField] List<TKey> _Keys = new List<TKey>();
        [SerializeField] List<TValue> _Values = new List<TValue>();

        // Save the dictionary to lists
        void ISerializationCallbackReceiver.OnBeforeSerialize() {
            _Keys.Clear();
            _Values.Clear();

            foreach (KeyValuePair<TKey, TValue> pair in this) {
                _Keys.Add(pair.Key);
                _Values.Add(pair.Value);
            }
        }

        // Load the dictionary from lists
        void ISerializationCallbackReceiver.OnAfterDeserialize() {
            this.Clear();

            if (_Keys.Count != _Values.Count) {
                Debug.LogError("Tried to deserialize a SerializableDictionary, but the amount of keys (" + _Keys.Count +
                               ") does not match the number of values (" + _Values.Count + ").");
            }

            for (int i = 0; i < _Keys.Count; i++) {
                this.Add(_Keys[i], _Values[i]);
            }
        }
    }
}
