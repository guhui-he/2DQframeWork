
using System.Collections.Generic;
using QFramework;
using Spine.Unity;
using UnityEngine;
using UnityEngine.U2D;

namespace Game
{
    public interface IResSystem : ISystem
    {
        Sprite GetSprite(string atlasPath, string spriteName);
        Sprite GetSprite(ItemType type, string spriteName);

        SkeletonDataAsset GetSkeleton(string path);
        Texture2D LoadTexture2D(string path);
        DialogueGraph GetGraph(string path);
    }
    public class ResSystem : AbstractSystem, IResSystem
    {
        private ResLoader mResLoader;
        private IResModel mResModel;
        private Dictionary<string, SpriteAtlas> mAtlasCache = new();
        protected override void OnInit()
        {
            mResLoader= ResLoader.Allocate();
            mResModel = this.GetModel<IResModel>();

            mResModel.AtlasPaths.OnAdd.Register((index, atlasPath) =>
            {
                var atlas = mResLoader.LoadSync<SpriteAtlas>(atlasPath);
                if (atlas != null)
                {
                    mAtlasCache[atlasPath] = atlas;
                }
            });
        }
        public Sprite GetSprite(string atlasPath, string spriteName)
        {
            if (mAtlasCache.TryGetValue(atlasPath, out var atlas))
            {
                return atlas.GetSprite(spriteName);
            }
            Debug.LogWarning($"找不到图集:{atlasPath}");
            return null;
        }
        public Sprite GetSprite(ItemType type, string spriteName)
        {
            switch (type)
            {
                case ItemType.Weapon:
                    //GetSprite()
                    return GetSprite(AssetPathDef.AtlasWeapon, spriteName);
                case ItemType.Shield:
                    return GetSprite(AssetPathDef.AtlasShield, spriteName);
                case ItemType.Helmet:
                    return GetSprite(AssetPathDef.AtlasHelmet, spriteName);
                case ItemType.Armor:
                    return GetSprite(AssetPathDef.AtlasArmor, spriteName);
                case ItemType.Boot:
                    return GetSprite(AssetPathDef.AtlasBoot, spriteName);
                case ItemType.Supplie:
                    return GetSprite(AssetPathDef.AtlasSupplies, spriteName);
                default:
                    return null;
            }
        }

        public Texture2D LoadTexture2D(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            // QFramework 会自动根据你配置的规则走 AB 包
            return mResLoader.LoadSync<Texture2D>(path);
        }

        public SkeletonDataAsset GetSkeleton(string path)
        {
            return mResLoader.LoadSync<SkeletonDataAsset>(path);
        }
        public DialogueGraph GetGraph(string path)
        {
            return mResLoader.LoadSync<DialogueGraph>(path);
        }
    }
}
