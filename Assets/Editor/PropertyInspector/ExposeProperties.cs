using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;


public static class ExposeProperties
{
    public static void Expose (PropertyField[] properties)
    {

        GUILayoutOption[] emptyOptions = new GUILayoutOption[0];

        EditorGUILayout.BeginVertical (emptyOptions);

        foreach (PropertyField field in properties) {

            EditorGUILayout.BeginHorizontal (emptyOptions);

            switch (field.Type) {
            case SerializedPropertyType.Integer:
                var oldInteger = (int)field.GetValue();
                var newInteger = EditorGUILayout.IntField (field.Name, oldInteger, emptyOptions);
                if (oldInteger != newInteger) {
                    field.SetValue (newInteger);
                }
                break;

            case SerializedPropertyType.Float:
                var oldFloat = (float)field.GetValue();
                var newFloat = EditorGUILayout.FloatField (field.Name, oldFloat, emptyOptions);
                if (oldFloat != newFloat) {
                    field.SetValue (newFloat);
                }
                break;

            case SerializedPropertyType.Boolean:
                var oldBoolean = (Boolean) field.GetValue();
                var newBoolean =  EditorGUILayout.Toggle (field.Name, oldBoolean, emptyOptions);
                if (oldBoolean != newBoolean) {
                    field.SetValue (newBoolean);
                }
                break;

            case SerializedPropertyType.String:
                var oldString = (String) field.GetValue();
                var newString = EditorGUILayout.TextField (field.Name, oldString, emptyOptions);
                if (oldString != newString) {
                    field.SetValue (newString);
                }
                break;

            case SerializedPropertyType.Vector2:
                var oldVector2 = (Vector2) field.GetValue();
                var newVector2 =  EditorGUILayout.Vector2Field (field.Name, oldVector2, emptyOptions);
                if (oldVector2 != newVector2) {
                    field.SetValue (newVector2);
                }
                break;

            case SerializedPropertyType.Vector3:
                var oldVector3 = (Vector3) field.GetValue();
                var newVector3 =  EditorGUILayout.Vector3Field (field.Name, oldVector3, emptyOptions);
                if (oldVector3 != newVector3) {
                    field.SetValue (newVector3);
                }
                break;

            case SerializedPropertyType.Enum:
                var oldEnum = (Enum) field.GetValue();
                var newEnum = EditorGUILayout.EnumPopup (field.Name, oldEnum, emptyOptions);
                if (oldEnum != newEnum) {
                    field.SetValue (newEnum);
                }
                break;

            case SerializedPropertyType.ObjectReference:
                var oldObjectReference = field.GetValue();
                var newObjectReference = EditorGUILayout.ObjectField (field.Name, (UnityEngine.Object)field.GetValue (), field.GetPropertyType (), true, emptyOptions);
                if (oldObjectReference != newObjectReference) {
                    field.SetValue (newObjectReference);
                }
                break;

            default:

                break;

            }

            EditorGUILayout.EndHorizontal ();

        }

        EditorGUILayout.EndVertical ();

    }

    public static PropertyField[] GetProperties (System.Object obj)
    {

        List< PropertyField > fields = new List<PropertyField> ();

        PropertyInfo[] infos = obj.GetType ().GetProperties (BindingFlags.Public | BindingFlags.Instance);

        foreach (PropertyInfo info in infos) {

            if (!(info.CanRead && info.CanWrite))
                continue;

            object[] attributes = info.GetCustomAttributes (true);

            bool isExposed = false;

            foreach (object o in attributes) {
                if (o.GetType () == typeof(ExposePropertyAttribute)) {
                    isExposed = true;
                    break;
                }
            }

            if (!isExposed)
                continue;

            SerializedPropertyType type = SerializedPropertyType.Integer;

            if (PropertyField.GetPropertyType (info, out type)) {
                PropertyField field = new PropertyField (obj, info, type);
                fields.Add (field);
            }

        }

        return fields.ToArray ();

    }

}


public class PropertyField
{
    System.Object m_Instance;
    PropertyInfo m_Info;
    SerializedPropertyType m_Type;

    MethodInfo m_Getter;
    MethodInfo m_Setter;

    public SerializedPropertyType Type {
        get {
            return m_Type;
        }
    }

    public String Name {
        get {
            return ObjectNames.NicifyVariableName (m_Info.Name);
        }
    }

    public PropertyField (System.Object instance, PropertyInfo info, SerializedPropertyType type)
    {

        m_Instance = instance;
        m_Info = info;
        m_Type = type;

        m_Getter = m_Info.GetGetMethod ();
        m_Setter = m_Info.GetSetMethod ();
    }

    public System.Object GetValue ()
    {
        return m_Getter.Invoke (m_Instance, null);
    }

    public void SetValue (System.Object value)
    {
        m_Setter.Invoke (m_Instance, new System.Object[] { value });
    }

    public Type GetPropertyType ()
    {
        return m_Info.PropertyType;
    }

    public static bool GetPropertyType (PropertyInfo info, out SerializedPropertyType propertyType)
    {

        propertyType = SerializedPropertyType.Generic;

        Type type = info.PropertyType;

        if (type == typeof(int)) {
            propertyType = SerializedPropertyType.Integer;
            return true;
        }

        if (type == typeof(float)) {
            propertyType = SerializedPropertyType.Float;
            return true;
        }

        if (type == typeof(bool)) {
            propertyType = SerializedPropertyType.Boolean;
            return true;
        }

        if (type == typeof(string)) {
            propertyType = SerializedPropertyType.String;
            return true;
        }

        if (type == typeof(Vector2)) {
            propertyType = SerializedPropertyType.Vector2;
            return true;
        }

        if (type == typeof(Vector3)) {
            propertyType = SerializedPropertyType.Vector3;
            return true;
        }

        if (type.IsEnum) {
            propertyType = SerializedPropertyType.Enum;
            return true;
        }
        // COMMENT OUT to NOT expose custom objects/types
        propertyType = SerializedPropertyType.ObjectReference;
        return true;

        //return false;

    }

}
