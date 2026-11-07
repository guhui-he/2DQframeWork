

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public static class Tools
    {
        public static int Roll(int minLevel, int maxLevel, float decay=0.3f)
        {
            // 计算总权重
            float total = 0f;
            float weight = 1f;
            for (int level = minLevel; level <= maxLevel; level++)
            {
                total += weight;
                weight *= decay;
            }

            // 按权重选
            float roll = Random.value * total;
            weight = 1f;
            for (int level = minLevel; level <= maxLevel; level++)
            {
                if (roll < weight) return level;
                roll -= weight;
                weight *= decay;
            }
            return maxLevel;  
        }
        public static Collider2D ScreenPointToRay2D(Camera cam, Vector2 mousePos)
        {
            Vector2 worldPos = cam.ScreenToWorldPoint(mousePos);
            Collider2D col = Physics2D.OverlapCircle(worldPos, 0.02f);
            return col;
        }
        public static  void SetAlpha(this Image image,float a)
        {
            Color color = image.color;
            color.a = a;
            image.color = color;
        }

        public static void SetSprite(Image image, ItemType type, string iconKey)
        {
            if (image == null || string.IsNullOrEmpty(iconKey)) return;
            image.sprite = GameApp.Interface.GetSystem<IResSystem>().GetSprite(type, iconKey);
        }
        public static void SetSprite(Image image, string path, string iconKey)
        {
            if (image == null || string.IsNullOrEmpty(iconKey)) return;
            image.sprite = GameApp.Interface.GetSystem<IResSystem>().GetSprite(path, iconKey);
        }
        public static readonly Color[] LevelColors = new Color[]
    {
        Color.white,                              // 占位，索引 0 不用
        new Color(0.85f, 0.85f, 0.85f),          // Lv1 灰（普通）
        new Color(0.30f, 0.90f, 0.30f),          // Lv2 绿（优秀）
        new Color(0.30f, 0.60f, 1.00f),          // Lv3 蓝（稀有）
        new Color(0.75f, 0.40f, 1.00f),          // Lv4 紫（史诗）
        new Color(1.00f, 0.65f, 0.10f),          // Lv5 橙（传说）
        new Color(1.00f, 0.30f, 0.30f),          // Lv6 红（神话）
        new Color(1.00f, 0.85f, 0.30f),          // Lv7 金（神器）★ 新增
    };
        public static Color GetLevelColor(int level)
        {
            if (level <= 0) level = 1;
            if (level >= LevelColors.Length) level = LevelColors.Length - 1;
            return LevelColors[level];
        }
    }
}
