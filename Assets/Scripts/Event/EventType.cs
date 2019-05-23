using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Screw
{
    public enum EventType
    {
        NONE,
        ON_LANGUAGE_CHANGED,
        #region System

        #endregion

        #region Ads

        GDPR_CHANGED,
        MOPUB_INITIALIZED,
        INCENTIVIZED_AVAILABILITY_CHANGED,
        INCENTIVIZED_EXPIRED,
        INCENTIVIZED_SHOW,
        INCENTIVIZED_RESULT_COMPLETE,
        ADS_AUDIO_START,
        ADS_AUDIO_FINISHED,

        #endregion

        #region In App Purchase

        PURCHASE_START,
        PURCHASE_SUCCESS,
        PURCHASE_FINISH,

        #endregion

        #region Game Specific

        SEARCH_CHANGE,
        SHUFFE_CHANGE,
        TIME_CHANGE,

        #endregion
    }

    public class EventTypeComparer : IEqualityComparer<EventType>
    {
        public bool Equals(EventType x, EventType y)
        {
            return x == y;
        }

        public int GetHashCode(EventType t)
        {
            return (int)t;
        }
    }

}