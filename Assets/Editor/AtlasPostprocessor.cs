using UnityEngine;
using UnityEditor;
using System.IO;

public class AtlasPostprocessor : AssetPostprocessor
{
    // 当资源被导入时自动调用
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string assetPath in importedAssets)
        {
            // 检查是否是 .atlas 结尾的文件
            if (assetPath.EndsWith(".atlas"))
            {
                // 生成新的带 .txt 的路径
                string newPath = assetPath + ".txt";

                // 如果新路径不存在，则重命名文件
                if (!File.Exists(newPath))
                {
                    // 告诉 Unity 资源数据库即将移动文件
                    string error = AssetDatabase.MoveAsset(assetPath, newPath);
                    if (!string.IsNullOrEmpty(error))
                    {
                        Debug.LogError($"重命名失败: {assetPath} -> {newPath}, 原因: {error}");
                    }
                    else
                    {
                        Debug.Log($"成功将 {assetPath} 重命名为 {newPath}");
                    }
                }
            }
        }
    }
}
