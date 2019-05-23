using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;
using Assets.Phunk.Core;
using LOT.Core;

public class CustomFSMManager : MonoBehaviour
{
    public string fsmName;
    public object comp;
    private Enum _state;
    public int state;
    public float currentStateTime = 0;
    public bool autoUpdate;

    private Dictionary<Enum, Action<Enum, GeneralOptions>> enterLookup;
    private Dictionary<Enum, Action<Enum, GeneralOptions>> exitLookup;
    private Dictionary<Enum, Func<float, bool>> updateLookup;

    public Action<float> StateMachineUpdate = NoopUpdate;
    Action<float> AutoStateMachineUpdate;

    /**
      Initialize this class with the custom enum.
     * The first enum is the default state
     **/
    public void Initialize (Type T, object comp, bool autoUpdate = true)
    {
        this.comp = comp;
        this.autoUpdate = autoUpdate;

        var values = Enum.GetValues (T);
        this._state = (Enum)values.GetValue (0);
        this.state = Convert.ToInt32 (this._state);

        enterLookup = new Dictionary<Enum, Action<Enum, GeneralOptions>> ();
        exitLookup = new Dictionary<Enum, Action<Enum, GeneralOptions>> ();
        updateLookup = new Dictionary<Enum, Func<float, bool>> ();

        string methodName;
        object f;
        foreach (var value in values) {
            methodName = String.Format ("StateMachineEnter_{0}", value.ToString ());
            f = CreateDelegate (typeof(Action<Enum, GeneralOptions>), comp, methodName) as Action<Enum, GeneralOptions>;
            if (f != null) {
                enterLookup [(Enum)value] = f as Action<Enum, GeneralOptions>;
            } else {
                enterLookup [(Enum)value] = Noop;
            }

            methodName = String.Format ("StateMachineExit_{0}", value.ToString ());
            f = CreateDelegate (typeof(Action<Enum, GeneralOptions>), comp, methodName) as Action<Enum, GeneralOptions>;
            if (f != null) {
                exitLookup [(Enum)value] = f as Action<Enum, GeneralOptions>;
            } else {
                exitLookup [(Enum)value] = Noop;
            }

            methodName = String.Format ("StateMachineUpdate_{0}", value.ToString ());
            f = CreateDelegate (typeof(Func<float, bool>), comp, methodName) as Func<float, bool>;
            if (f != null) {
                updateLookup [(Enum)value] = f as Func<float, bool>;
            } else {
                updateLookup [(Enum)value] = Noop;
            }
        }

        if (!autoUpdate) {
            StateMachineUpdate = _StateMachineUpdate;
            AutoStateMachineUpdate = NoopUpdate;
        } else {
            StateMachineUpdate = NoopUpdate;
            AutoStateMachineUpdate = _StateMachineUpdate;
        }
    }

    public static object CreateDelegate (Type T, object o, string methodName)
    {
        MethodInfo m = GetMethodRecursive (o, methodName);

        if (m != null) {
            try {
                return Delegate.CreateDelegate (T, o, m);
            } catch (Exception e) {
                Debug.LogError ("CustomFSMManager::CreateDelegate method not compatible " + methodName);
            }
        }

        return null;
    }

    public static MethodInfo GetMethodRecursive (object o, string methodName)
    {
        Type type = o.GetType ();
        MethodInfo m = null;
        while (type != null) {
            m = type.GetMethod (methodName, BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic);
            if (m != null) {
                return m;
            } else {
                type = type.BaseType;
            }
        }

        return m;
    }

    public static void Noop (Enum e, GeneralOptions options)
    {
    }

    public static bool Noop (float t)
    {
        return false;
    }

    public static void NoopUpdate (float t)
    {
    }

    // Try not to transition to the current state. It's a hack to do that
    public void StateMachineChange (Enum state, GeneralOptions options = null)
    {
        Log.VerboseFormat ("{0}::StateMachineChange {1}", comp.GetType ().Name, state);
        StateMachineExit (state, options);
        StateMachineEnter (state, options);
    }

    void StateMachineEnter (Enum state, GeneralOptions options = null)
    {
        var oldState = this._state;
        this._state = state;
        this.state = Convert.ToInt32 (this._state);
        this.currentStateTime = 0;
        enterLookup [state] (oldState, options);
    }

    void StateMachineExit (Enum state, GeneralOptions options = null)
    {
        exitLookup [_state] (state, options);
    }

    public void Update ()
    {
        AutoStateMachineUpdate (Time.deltaTime);
    }

    void _StateMachineUpdate (float deltaTime)
    {
        if (updateLookup [_state] (deltaTime)) {
            return;
        }

        currentStateTime += deltaTime;
    }
}
