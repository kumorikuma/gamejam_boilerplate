#nullable enable

using System;
using Kumorikuma.Gameplay;
using UnityEngine;

namespace Kumorikuma.Utilities {
    [Serializable]
    public class IntPlayerSerializableDictionary : SerializableDictionary<int, Player> {
    }
    
    [Serializable]
    public class IntActorSerializableDictionary : SerializableDictionary<int, Actor> {
    }

    [Serializable]
    public class StringStringSerializableDictionary : SerializableDictionary<string, string> {
    }

    [Serializable]
    public class StringBoolSerializableDictionary : SerializableDictionary<string, bool> {
    }

    [Serializable]
    public class StringTransformSerializableDictionary : SerializableDictionary<string, Transform> {
    }
}
