using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.StationsData {
  [Serializable]
  public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver {
    [SerializeField] private List<TKey> _mKeys = new List<TKey>();

    [SerializeField] private List<TValue> _mValues = new List<TValue>();

    public void OnBeforeSerialize() {
      _mKeys.Clear();
      _mValues.Clear();
      using Enumerator enumerator = GetEnumerator();
      while (enumerator.MoveNext()) {
        var (key, value) = enumerator.Current;
        _mKeys.Add(key);
        _mValues.Add(value);
      }
    }

    public void OnAfterDeserialize() {
      Clear();
      for (int i = 0; i < _mKeys.Count; i++) {
        Add(_mKeys[i], _mValues[i]);
      }

      _mKeys.Clear();
      _mValues.Clear();
    }
  }
}
