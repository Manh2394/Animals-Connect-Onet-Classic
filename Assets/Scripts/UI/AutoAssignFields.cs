using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

public class AutoAssignFields : MonoBehaviour
{
	public Component component;

	void Awake ()
	{
		Assign ();
	}


	public void Assign ()
	{
		if (component == null)
			return;

		var fields = component.GetType ().GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
		foreach (var info in fields) 
		{
			var attributes = info.GetCustomAttributes (typeof(AutoAssignAttribute), false);
			if (attributes.Length > 0) 
			{
				var att = attributes [0] as AutoAssignAttribute;
				object value = null;
				switch (att.scope) {
				case AutoAssignAttribute.Scope.Children:
					value = component.GetComponentInChildren (info.FieldType, true);
					break;
				case AutoAssignAttribute.Scope.Parent:
					value = component.GetComponentInParent (info.FieldType);
					break;
				case AutoAssignAttribute.Scope.Global:
					value = GameObject.FindObjectOfType (info.FieldType);
					break;
				}
					
				info.SetValue (component, value);
			}
		}

		var properties = component.GetType ().GetProperties (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
		foreach (var info in properties) 
		{
			var attributes = info.GetCustomAttributes (typeof(AutoAssignAttribute), false);
			if (attributes.Length > 0) 
			{
				var att = attributes [0] as AutoAssignAttribute;
				object value = null;
				switch (att.scope) {
				case AutoAssignAttribute.Scope.Children:
					value = component.GetComponentInChildren (info.PropertyType, true);
					break;
				case AutoAssignAttribute.Scope.Parent:
					value = component.GetComponentInParent (info.PropertyType);
					break;
				case AutoAssignAttribute.Scope.Global:
					value = GameObject.FindObjectOfType (info.PropertyType);
					break;
				}

				info.SetValue (component, value, null);
			}
		}
	}
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class AutoAssignAttribute : Attribute
{
	public Scope scope;

	public AutoAssignAttribute (Scope scope=Scope.Children)
	{
		this.scope = scope;
	}

	public enum Scope
	{
		Children,
		Parent,
		Global,
	}
}
