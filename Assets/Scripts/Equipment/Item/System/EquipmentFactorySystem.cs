using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IEquipmentFactorySystem : ISystem
    {
        HeroEquipment CreateRandom(HeroEquipment equipment);
        ItemInfo CreateRandom(ItemType type, int minLevel, int maxLevel);
        ItemInfo CreateRandom(ItemType type);
        //ItemInfo CreateFrom(ItemConfigData template);
        List<ItemInfo> CreateRandomBatch(ItemType type, int count);

    }

    public class EquipmentFactorySystem : AbstractSystem, IEquipmentFactorySystem
    {
        private IRandomItemSystem mRandomSystem;
        private IAffixGeneratorSystem mAffixSystem;

        protected override void OnInit()
        {
            mRandomSystem = this.GetSystem<IRandomItemSystem>();
            mAffixSystem = this.GetSystem<IAffixGeneratorSystem>();
        }

        public HeroEquipment CreateRandom(HeroEquipment equipment)
        {
            var pool = new List<ItemType>(EquipSlots);
            Shuffle(pool);
            int targetCount = Tools.Roll(0, pool.Count, 0.1f);
            for (int i = 0; i < targetCount; i++)
            {
                ItemType slot = pool[i];
                var info = CreateRandom(slot);
                info.area = AreaType.Equipment;
                equipment.SetEquipped(slot, info);
            }
            return equipment;
        }

        public ItemInfo CreateRandom(ItemType type, int minLevel, int maxLevel)
        {
            var template = mRandomSystem.GetRandomByType(type, minLevel, maxLevel);
            return CreateFrom(template);
        }

        public ItemInfo CreateRandom(ItemType type)
        {
            var template = mRandomSystem.GetRandomByType(type);
            if (template == null)
            {
                Debug.LogWarning($"[装备] {type} 没有可抽的模板");
                return null;
            }
            return CreateFrom(template);
        }
        /// <summary>
        /// 创造装备底层副本
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private ItemInfo CreateFrom(ItemConfigData template)
        {
            if (template == null) return null;

            var item = new ItemInfo();
            item.SetBaseData(template);
            // 生成词条
            mAffixSystem.GenerateAffixes(item);

            return item;
        }
        public List<ItemInfo> CreateRandomBatch(ItemType type, int count)
        {
            var result = new List<ItemInfo>(count);
            if (count <= 0) return result;

            var templates = mRandomSystem
                .GetRandomByType(type, count);

            foreach (var template in templates)
            {
                var item = CreateFrom(template);
                if (item != null) result.Add(item);
            }
            return result;
        }


        private static readonly ItemType[] EquipSlots =
        {
            ItemType.Helmet,
            ItemType.Armor,
            ItemType.Shield,
            ItemType.Weapon,
            ItemType.Boot,
        };
        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
