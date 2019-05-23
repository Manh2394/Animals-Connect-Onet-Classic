
namespace LOT.Core
{
    public class BasePath
    {
        public static string GetGameContextPath ()
        {
            return "Scenes/GameContext";
        }

        public static string GetPrefabBasePath()
        {
            return GetResourceBasePath() + "Prefabs/";
        }

        public static string GetLocalizedPrefabBasePath()
        {
            return GetLocalizedResourceBasePath() + "Prefabs/";
        }

        public static string GetResourceBasePath()
        {
            return "lang/common/";
        }

        public static string GetLocalizedResourceBasePath()
        {
            return "lang/";
        }

        public static string GetJSONPath (string name)
        {
            return "JSON/" + name;
        }

        public static string GetStageMapPath(string name)
        {
            return GetPrefabBasePath() + "Stage/StageMap" + name;
        }

        public static string GetCharacterPath(string name)
        {
            return GetPrefabBasePath() + "Characters/" + name;
        }

        public static string GetBattleEffectPath(string name)
        {
            return BasePath.GetPrefabBasePath() + "Battle/Effects/" + name;
        }

        public static string GetSpriteCollectionPath(string name)
        {
            return GetResourceBasePath() + "Prefabs/SpriteCollections/" + name;
        }

        public static string GetProjectilePath(string name)
        {
            return GetResourceBasePath() + "Prefabs/Projectiles/" + name;
        }

        public static string GetDropItemPath(string name)
        {
            return GetResourceBasePath() + "Prefabs/DropItems/" + name;
        }

        public static string GetLoadingIconPath(string name)
        {
            return GetResourceBasePath() + "Prefabs/LoadingIcon/" + name;
        }
    }
}
