using UnityEngine;
using System;

namespace Assets.Phunk.Core
{
    public static class Log
    {
        #region Error
        [System.Diagnostics.Conditional ("LOG_ERROR")]
        [System.Diagnostics.Conditional ("LOG_WARNING")]
        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void ErrorFormat (UnityEngine.Object context, string template, params object[] args)
        {
            if (!Debug.isDebugBuild) return;
            var message = string.Format (template, args);
            Error (context, message);
        }

        [System.Diagnostics.Conditional ("LOG_ERROR")]
        [System.Diagnostics.Conditional ("LOG_WARNING")]
        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void ErrorFormat (string template, params object[] args)
        {
            if (!Debug.isDebugBuild) return;
            var message = string.Format (template, args);
            Error (message);
        }

        [System.Diagnostics.Conditional ("LOG_ERROR")]
        [System.Diagnostics.Conditional ("LOG_WARNING")]
        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void Error (object message)
        {
            if (!Debug.isDebugBuild) return;
            Debug.LogError (String.Format ("{0:0.000} {1}", Time.time, message));
        }

        [System.Diagnostics.Conditional ("LOG_ERROR")]
        [System.Diagnostics.Conditional ("LOG_WARNING")]
        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void Error (UnityEngine.Object context, object message)
        {
            if (!Debug.isDebugBuild) return;
            Debug.LogError (String.Format ("{0:0.000} {1}", Time.time, message), context);
        }

        #endregion

        #region Warning

        [System.Diagnostics.Conditional ("LOG_WARNING")]
        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void WarningFormat (UnityEngine.Object context, string template, params object[] args)
        {
            if (!Debug.isDebugBuild) return;
            var message = string.Format (template, args);
            Warning (context, message);
        }

        [System.Diagnostics.Conditional ("LOG_WARNING")]
        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void WarningFormat (string template, params object[] args)
        {
            if (!Debug.isDebugBuild) return;
            var message = string.Format (template, args);
            Warning (message);
        }

        [System.Diagnostics.Conditional ("LOG_WARNING")]
        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void Warning (object message)
        {
            if (!Debug.isDebugBuild) return;
            Debug.LogWarning (String.Format ("{0:0.000} {1}", Time.time, message));
        }

        [System.Diagnostics.Conditional ("LOG_WARNING")]
        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void Warning (UnityEngine.Object context, object message)
        {
            if (!Debug.isDebugBuild) return;
            Debug.LogWarning (String.Format ("{0:0.000} {1}", Time.time, message), context);
        }

        #endregion

        #region Message
        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void MessageFormat (UnityEngine.Object context, string template, params object[] args)
        {
            if (!Debug.isDebugBuild) return;
            var message = string.Format (template, args);
            Message (context, message);
        }

        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void MessageFormat (string template, params object[] args)
        {
            if (!Debug.isDebugBuild) return;
            var message = string.Format (template, args);
            Message (message);
        }

        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void Message (object message)
        {
            if (!Debug.isDebugBuild) return;
            Debug.Log (String.Format ("{0:0.000} {1}", Time.time, message));
        }

        [System.Diagnostics.Conditional ("LOG_DEBUG")]
        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void Message (UnityEngine.Object context, object message)
        {
            if (!Debug.isDebugBuild) return;
            Debug.Log (String.Format ("{0:0.000} {1}", Time.time, message), context);
        }

        #endregion

        #region Verbose

        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void VerboseFormat (UnityEngine.Object context, string template, params object[] args)
        {
            if (!Debug.isDebugBuild) return;
            var message = string.Format (template, args);
            Verbose (context, message);
        }

        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void VerboseFormat (string template, params object[] args)
        {
            if (!Debug.isDebugBuild) return;
            var message = string.Format (template, args);
            Verbose (message);
        }

        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void Verbose (object message)
        {
            if (!Debug.isDebugBuild) return;
            Debug.Log (String.Format ("<color=grey>[VERBOSE]</color> {0:0.000} {1}", Time.time, message));
        }

        [System.Diagnostics.Conditional ("LOG_ALL")]
        public static void Verbose (UnityEngine.Object context, object message)
        {
            if (!Debug.isDebugBuild) return;
            Debug.Log (String.Format ("<color=grey>[VERBOSE]</color> {0:0.000} {1}", Time.time, message), context);
        }

        #endregion
    }
}