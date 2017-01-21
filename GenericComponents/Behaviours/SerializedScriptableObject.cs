using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using Utils.DataStructures;

namespace GenericComponents.Behaviours
{
    public abstract class SerializedScriptableObject : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        [HideInInspector]
        private StrObjDict serializedObjects = new StrObjDict();
        [SerializeField]
        [HideInInspector]
        private StrStrDict serializedStrings = new StrStrDict();
        [SerializeField]
        [HideInInspector]
        private StrObjDict serializedObjectsFromList = new StrObjDict();

        private BinaryFormatter serializer = new BinaryFormatter();

        public void OnAfterDeserialize()
        {
            Deserialize();
        }
        public void OnBeforeSerialize()
        {
            Serialize();
        }
        private void Serialize()
        {
            serializedObjectsFromList.Clear();
            foreach (var field in GetInterfaces())
            {
                var value = field.GetValue(this);
                if (value == null)
                    continue;
                string name = field.Name;
                var obj = value as UnityEngine.Object;
                if (obj != null) // the implementor is a UnityEngine.Object
                {
                    serializedObjects[name] = obj; // using the field's name as a key because you can't have two fields with the same name
                }
                else if(!IsGenericListOfInterfaces(field))
                {
                    // try to serialize the interface to a string and store the result in our other dictionary
                    // if it's a plain object
                    SerializePlainObject(value, name);
                }
                else
                {
                    //if it's a list it main contain a Unity Object, we have to serialize them separatelly
                    SerializeList(value as IList, name, field.FieldType.GetGenericArguments().First());
                }
            }
        }

        private void Deserialize()
        {
            foreach (var field in GetInterfaces())
            {
                object result = null;
                string name = field.Name;
                // Try and fetch the field's serialized value
                UnityEngine.Object obj;
                if (serializedObjects.TryGetValue(name, out obj)) // if the implementor is a UnityObject, then we just fetch the value from our dictionary as the result
                {
                    result = obj;
                }
                else // otherwise, get it from our other dictionary
                {
                    string serializedString;
                    if (serializedStrings.TryGetValue(name, out serializedString))
                    {
                        // deserialize the string back to the original object
                        byte[] bytes = Convert.FromBase64String(serializedString);
                        using (var stream = new MemoryStream(bytes))
                            result = serializer.Deserialize(stream);
                    }
                    //if it's a list it's possible that it includes a ScriptableObject implementing an interface
                    if (IsGenericListOfInterfaces(field))
                    {
                        var unityObjects = serializedObjectsFromList.Where(kvp => kvp.Key.StartsWith(name)).ToList();
                        var list = result as IList;
                        for(var idx = 0; idx < unityObjects.Count(); idx++)
                        {
                            list.Add(unityObjects[idx].Value);
                        }
                    }

                }
                field.SetValue(this, result);
            }
        }

        private void SerializeList(IList list, string name, Type genericType)
        {
            var listWithUnityObject = new List<UnityEngine.Object>();
            var listWithoutUnityObject = new List<UnityEngine.Object>();
            var listType = typeof(List<>);
            var constructedListType = listType.MakeGenericType(genericType);
            var listCopy = Activator.CreateInstance(constructedListType) as IList;

            for (var idx = 0; idx < list.Count; idx++)
            {
                if(list[idx] is UnityEngine.Object)
                {
                    listWithUnityObject.Add(list[idx] as UnityEngine.Object);
                }
                else
                {
                    listCopy.Add(list[idx]);
                }
            }

            SerializePlainObject(listCopy, name);
            SerializeUnityObjectsFromListObject(listWithUnityObject, name);
        }

        private void SerializePlainObject(object value, string name)
        {
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, value);
                stream.Flush();
                serializedObjects.Remove(name); // it could happen that the field might end up in both the dictionaries, ex when you change the implementation of the interface to use a System.Object instead of a UnityObject
                serializedStrings[name] = Convert.ToBase64String(stream.ToArray());
            }
        }

        private void SerializeUnityObjectsFromListObject(IList<UnityEngine.Object> values, string name)
        {
            for(var idx = 0; idx < values.Count; idx++)
            {
                serializedObjectsFromList[name + ";" + idx] = values[idx];
            }
            
        }

        private IEnumerable<FieldInfo> GetInterfaces()
        {
            return 
                GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(f => 
                        !f.IsDefined(typeof(HideInInspector), true) && 
                        (f.IsPublic || f.IsDefined(typeof(SerializeField), true)))
                    .Where(f => f.FieldType.IsInterface || IsGenericListOfInterfaces(f));
        }

        private bool IsGenericListOfInterfaces(FieldInfo field)
        {
            var fieldType = field.FieldType;
            if (fieldType.IsGenericType && 
                (fieldType.GetGenericTypeDefinition() == typeof(List<>)) &&
                (fieldType.GetGenericArguments().Count() == 1) &&
                (fieldType.GetGenericArguments().First().IsInterface))
            {
                return true;
            }
            return false;
        }
    }
}
