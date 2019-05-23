using UnityEngine;
using System.Collections.Generic;

namespace LOT.Core
{
    public class Consts
    {
        public static class Hashes
        {
            public static int hurt = Animator.StringToHash ("hurt");
            public static int speed = Animator.StringToHash ("speed");
            public static int skill = Animator.StringToHash ("skill");
            public static int comboStage = Animator.StringToHash ("comboStage");
            public static int dead = Animator.StringToHash ("dead");
        }

        public class CardType
        {
            public const int My = 1;
            public const int Enemy = 2;
        }

        public class ChapterType
        {
            public const int Normal = 0;
            public const int Boss = 1;
        }

        // Camera shake duration in seconds
        public const float SHAKE_DURATION = 0.5f;

        // Factor for noise generator
        public const float SHAKE_SPEED = 4.0f;

        public const float StandardOrthoSize = 3.2f;

        public const int ParamAlreadySet = 1000;

        public const string TagEnemy = "Enemy";

        public const float CameraZ = -10;
        public const float EnemyLayerZ = -1;
        public const float PlayerLayerZ = -2;
        public const float EffectLayerZ = -3;

        public const int NumOfSkillButtons = 4;
        public const int MaxEquipSlotNum = 6;
        public const int UsedCurrencyIdForEnhance = 1;
        public const int GemId = 2;

        public static readonly Vector3 InvisiblePos = new Vector3 (10000, 0, 0);
        public const int maxAfflictionNum = 5;
        public const int maxSkillActionNum = 5;
        public const int maxSkillNum = 5;
        public const int BossType = 5;
        public const string LoadingIcon = "NormalLoadingIcon";

        public static readonly string [] MasterCacheNames = new string[]
        {
            "skillBlow",
            "skillAction",
            "skill",
            "projectile",
            "cardLevel",
            "card",
            "dataVersion",
            "gear",
            "nonGear",
            "chapter",
            "stage",
        };
        public static readonly Dictionary<int, int> StarLevel = new Dictionary<int, int> ()
        {
            {1,1},
            {2,5},
            {3,10}
        };
        public enum RewardType
        {
            Gear = 1,
            Exp = 2,
            Coin = 3,
            Diamond = 4,
        };
    }
}
