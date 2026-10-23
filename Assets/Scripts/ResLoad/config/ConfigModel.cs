using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IConfigModel : IModel
    {
        ConfigTable   GetTable(string tableName);

    }

    public class ConfigModel : AbstractModel, IConfigModel
    {
        private readonly Dictionary<string, ConfigTable> mTables = new();
        private ResLoader mResLoader;

        protected override void OnInit()
        {
            mResLoader = ResLoader.Allocate();
            // 注册所有要加载的表
            foreach (var tableName in ConfigConstants.AllTables)
                LoadTable(tableName,tableName);
        }

        private void LoadTable(string tableName, string path)
        {
            var textAsset = mResLoader.LoadSync<TextAsset>(path);
            var table = new ConfigTable(tableName);
            table.Load(textAsset.text);
            mTables[tableName] = table;
        }

        public ConfigTable GetTable(string tableName)
            => mTables.TryGetValue(tableName, out var t) ? t : null;
    }
}
