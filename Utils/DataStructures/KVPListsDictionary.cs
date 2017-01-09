using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Utils.DataStructures
{
    [Serializable]
    public class KVPListsDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        [SerializeField]
        private List<TKey> keys;
        [SerializeField]
        private List<TValue> values;

        public TKey[] Keys { get { return keys.ToArray(); } }
        public TValue[] Values { get { return values.ToArray(); } }
        public int Count { get { return keys.Count; } }

        public KVPListsDictionary()
        {
            keys = new List<TKey>();
            values = new List<TValue>();
        }

        public TValue this[TKey key]
        {
            get
            {
                int index;
                if (!TryGetIndex(key, out index))
                {
                    throw new KeyNotFoundException(key.ToString());
                }
                return values[index];
            }
            set
            {
                int index;
                if (!TryGetIndex(key, out index))
                {
                    Add(key, value);
                }
                else values[index] = value;
            }
        }

        public void SetKeyAt(int i, TKey value)
        {
            AssertIndexInBounds(i);
            AssertUniqueKey(value);
            keys[i] = value;
        }

        public TKey GetKeyAt(int i)
        {
            AssertIndexInBounds(i);
            return keys[i];
        }

        public TValue GetValueAt(int i)
        {
            AssertIndexInBounds(i);
            return values[i];
        }

        public KeyValuePair<TKey, TValue> GetPairAt(int i)
        {
            AssertIndexInBounds(i);
            return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
        }

        private void AssertIndexInBounds(int i)
        {
            if(i < 0 || i >= keys.Count)
            {
                throw new IndexOutOfRangeException("i");
            }
        }

        public void Clear()
        {
            keys.Clear();
            values.Clear();
        }

        public void Insert(int i, TKey key, TValue value)
        {
            AssertUniqueKey(key);
            keys.Insert(i, key);
            values.Insert(i, value);
        }

        private void AssertUniqueKey(TKey key)
        {
            if (ContainsKey(key))
                throw new ArgumentException(string.Format("There's already a key `{0}` defined in the dictionary", key.ToString()));
        }

        public void Insert(TKey key, TValue value)
        {
            Insert(0, key, value);
        }

        public void Add(TKey key, TValue value)
        {
            Insert(Count, key, value);
        }

        public void Remove(TKey key)
        {
            int index;
            if (TryGetIndex(key, out index))
            {
                keys.RemoveAt(index);
                values.RemoveAt(index);
            }
        }

        public void RemoveAt(int i)
        {
            AssertIndexInBounds(i);
            keys.RemoveAt(i);
            values.RemoveAt(i);
        }

        public bool TryGetValue(TKey key, out TValue result)
        {
            int index;
            if (!TryGetIndex(key, out index))
            {
                result = default(TValue);
                return false;
            }
            result = values[index];
            return true;
        }

        public bool ContainsValue(TValue value)
        {
            return values.Contains(value);
        }

        public bool ContainsKey(TKey key)
        {
            return keys.Contains(key);
        }

        private bool TryGetIndex(TKey key, out int index)
        {
            return (index = keys.IndexOf(key)) != -1;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return new KeyValuePair<TKey, TValue>(keys[i], values[i]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Serializable]
    public class StrObjDict : KVPListsDictionary<string, UnityEngine.Object>
    {
    }
    [Serializable]
    public class StrStrDict : KVPListsDictionary<string, string>
    {
    }
}
