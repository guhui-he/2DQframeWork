using QFramework;
using UnityEngine;

namespace Game
{
    public interface IPrefabLoaderUtility : IUtility
    {
        /// <summary>
        /// 同步加载并实例化预制体（自动处理 ResLoader 的分配与释放）
        /// </summary>
        GameObject Instantiate(string path, Transform parent = null, Vector3? pos = null);

        /// <summary>
        /// 加载预设体（仅返回 GameObject，不实例化）
        /// </summary>
        GameObject LoadPrefab(string path);
    }

    public class PrefabLoaderUtility : IPrefabLoaderUtility
    {

        private readonly ResLoader mLoader = ResLoader.Allocate();

        public GameObject LoadPrefab(string path)
        {
            var prefab = mLoader.LoadSync<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError($"[PrefabLoader] 找不到预制体: {path}");
            }
            return prefab;
        }

        public GameObject Instantiate(string path, Transform parent = null, Vector3? pos = null)
        {
            var prefab = LoadPrefab(path);
            if (prefab == null) return null;

            var obj = Object.Instantiate(prefab, parent);

            if (pos.HasValue)
                obj.transform.position = pos.Value;

            return obj;
        }
        
    }
}
