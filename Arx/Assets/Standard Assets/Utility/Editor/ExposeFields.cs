using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Standard_Assets.Utility.Editor
{
    public static class ExposeFields
    {
        public static void Expose(BaseField[] fields)
        {

            var emptyOptions = new GUILayoutOption[0];

            EditorGUILayout.BeginVertical(emptyOptions);

            foreach (var field in fields)
            {

                EditorGUILayout.BeginHorizontal(emptyOptions);

                switch (field.Type)
                {
                    case SerializedPropertyType.Integer:
                        field.SetValue(EditorGUILayout.IntField(field.Name, (int)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.Float:
                        field.SetValue(EditorGUILayout.FloatField(field.Name, (float)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.Boolean:
                        field.SetValue(EditorGUILayout.Toggle(field.Name, (bool)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.String:
                        field.SetValue(EditorGUILayout.TextField(field.Name, (String)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.Vector2:
                        field.SetValue(EditorGUILayout.Vector2Field(field.Name, (Vector2)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.Vector3:
                        field.SetValue(EditorGUILayout.Vector3Field(field.Name, (Vector3)field.GetValue(), emptyOptions));
                        break;



                    case SerializedPropertyType.Enum:
                        field.SetValue(EditorGUILayout.EnumPopup(field.Name, (Enum)field.GetValue(), emptyOptions));
                        break;

                    case SerializedPropertyType.ObjectReference:
                        field.SetValue(EditorGUILayout.ObjectField(field.Name, (UnityEngine.Object)field.GetValue(), field.GetFieldType(), true, emptyOptions));
                        break;

                    default:

                        break;

                }

                EditorGUILayout.EndHorizontal();

            }

            EditorGUILayout.EndVertical();

        }

        public static PropertyField[] GetProperties(System.Object obj)
        {

            var fields = new List<PropertyField>();

            var propInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var info in propInfos)
            {

                if (!(info.CanRead && info.CanWrite))
                    continue;

                object[] attributes = info.GetCustomAttributes(true);

                /*bool isExposed = false;

                foreach (object o in attributes)
                {
                    if (o.GetType() == typeof(NonSerialized))
                    {
                        isExposed = true;
                        break;
                    }
                }

                if (!isExposed)
                    continue;*/

                var type = SerializedPropertyType.Integer;

                if (PropertyField.GetPropertyType(info.PropertyType, out type))
                {
                    PropertyField field = new PropertyField(obj, info, type);
                    fields.Add(field);
                }

            }

            return fields.ToArray();

        }

        public static Field[] GetFields(System.Object obj)
        {
            var fields = new List<Field>();

            var fieldInfos = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var info in fieldInfos)
            {

                var attributes = info.GetCustomAttributes(true);

                bool isExposed = true;

                foreach (object o in attributes)
                {
                    if (o.GetType() == typeof(HideInInspector))
                    {
                        isExposed = false;
                        break;
                    }
                }

                if (!isExposed)
                    continue;

                var type = SerializedPropertyType.Integer;

                if (Field.GetPropertyType(info.FieldType, out type))
                {
                    var field = new Field(obj, info, type);
                    fields.Add(field);
                }

            }

            return fields.ToArray();

        }
    }

    public abstract class BaseField
    {
        public abstract SerializedPropertyType Type { get; }
        public abstract string Name { get; }

        public abstract System.Object GetValue();
        public abstract void SetValue(System.Object value);
        public abstract Type GetFieldType();

        public static bool GetPropertyType(Type type, out SerializedPropertyType propertyType)
        {
            propertyType = SerializedPropertyType.Generic;

            if (type == typeof(int))
            {
                propertyType = SerializedPropertyType.Integer;
                return true;
            }

            if (type == typeof(float))
            {
                propertyType = SerializedPropertyType.Float;
                return true;
            }

            if (type == typeof(bool))
            {
                propertyType = SerializedPropertyType.Boolean;
                return true;
            }

            if (type == typeof(string))
            {
                propertyType = SerializedPropertyType.String;
                return true;
            }

            if (type == typeof(Vector2))
            {
                propertyType = SerializedPropertyType.Vector2;
                return true;
            }

            if (type == typeof(Vector3))
            {
                propertyType = SerializedPropertyType.Vector3;
                return true;
            }

            if (type.IsEnum)
            {
                propertyType = SerializedPropertyType.Enum;
                return true;
            }
            // COMMENT OUT to NOT expose custom objects/types
            propertyType = SerializedPropertyType.ObjectReference;
            return true;

            //return false;

        }

    }

    public class Field : BaseField
    {
        System.Object m_Instance;
        FieldInfo m_Info;
        SerializedPropertyType m_Type;

        public override SerializedPropertyType Type
        {
            get
            {
                return m_Type;
            }
        }

        public override string Name
        {
            get
            {
                return ObjectNames.NicifyVariableName(m_Info.Name);
            }
        }

        public Field(System.Object instance, FieldInfo info, SerializedPropertyType type)
        {
            m_Instance = instance;
            m_Info = info;
            m_Type = type;
        }

        public override System.Object GetValue()
        {
            return m_Info.GetValue(m_Instance);
        }

        public override void SetValue(System.Object value)
        {
            m_Info.SetValue(m_Instance, value);
        }

        public override Type GetFieldType()
        {
            return m_Info.FieldType;
        }
    }


    public class PropertyField : BaseField
    {
        System.Object m_Instance;
        PropertyInfo m_Info;
        SerializedPropertyType m_Type;

        MethodInfo m_Getter;
        MethodInfo m_Setter;

        public override SerializedPropertyType Type
        {
            get
            {
                return m_Type;
            }
        }

        public override string Name
        {
            get
            {
                return ObjectNames.NicifyVariableName(m_Info.Name);
            }
        }

        public PropertyField(System.Object instance, PropertyInfo info, SerializedPropertyType type)
        {

            m_Instance = instance;
            m_Info = info;
            m_Type = type;

            m_Getter = m_Info.GetGetMethod();
            m_Setter = m_Info.GetSetMethod();
        }

        public override System.Object GetValue()
        {
            return m_Getter.Invoke(m_Instance, null);
        }

        public override void SetValue(System.Object value)
        {
            m_Setter.Invoke(m_Instance, new System.Object[] { value });
        }

        public override Type GetFieldType()
        {
            return m_Info.PropertyType;
        }
    }
}
