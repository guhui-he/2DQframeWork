using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 单张配置表。约定：第 1 行字段名，第 2 行注释，第 3 行开始数据。
    /// </summary>
    public class ConfigTable
    {
        public string TableName { get; }
        private readonly Dictionary<int, Dictionary<string, string>> mData = new();

        public ConfigTable(string tableName)
        {
            TableName = tableName;
        }

        public void Load(string txt)
        {
            if (string.IsNullOrEmpty(txt)) return;

            string[] lines = txt
                .Replace("\r\n", "\n")
                .Replace("\r", "\n")
                .Split('\n');

            if (lines.Length < 3)
            {
                Debug.LogError($"[Config] {TableName} 行数不足");
                return;
            }

            // 表头（第 1 行）
            string[] headers = lines[0]
                .Trim()
                .TrimStart('\uFEFF')
                .Split(',');

            // 数据从第 3 行开始
            for (int i = 2; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (string.IsNullOrEmpty(line)) continue;

                string[] cells = line.Split(',');
                var row = new Dictionary<string, string>();

                for (int j = 0; j < headers.Length; j++)
                {
                    string key = headers[j].Trim();
                    string val = (j < cells.Length) ? cells[j].Trim() : "";
                    row[key] = val;
                }

                if (!row.TryGetValue("Id", out var idStr) || !int.TryParse(idStr, out int id))
                {
                    Debug.LogError($"[Config] {TableName} 第 {i + 1} 行 Id 非法");
                    continue;
                }

                if (mData.ContainsKey(id))
                    Debug.LogWarning($"[Config] {TableName} Id={id} 重复，覆盖");

                mData[id] = row;
            }
        }

        public Dictionary<string, string> GetRow(int id)
            => mData.TryGetValue(id, out var row) ? row : null;

        public IEnumerable<Dictionary<string, string>> GetAllRows()
            => mData.Values;


    }
}
