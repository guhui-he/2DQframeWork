using Game;

namespace Game
{
    public static class ItemSaveMapper
    {
        public static ItemInfoSaveData ToSaveData(this ItemInfo item)
        {
            if (item == null || item.itemData == null) return null;

            var save = new ItemInfoSaveData
            {
                ConfigId = item.itemData.Id,
                Area = (int)item.area
            };

            if (item.Affixes != null)
            {
                foreach (var affix in item.Affixes)
                {
                    save.Affixes.Add(new ItemAffixSaveData
                    {
                        Stat = (int)affix.Stat,
                        Value = affix.Value
                    });
                }
            }
            return save;
        }

        public static ItemInfo FromSaveData(this ItemInfoSaveData save)
        {
            if (save == null) return null;

            var config = GameApp.Interface.GetModel<IItemConfigModel>().GetItem(save.ConfigId);

            var item = new ItemInfo();
            item.SetBaseData(config);
            item.area = (AreaType)save.Area;

            if (save.Affixes != null)
            {
                foreach (var a in save.Affixes)
                    item.Affixes.Add(new ItemAffix((AffixStatType)a.Stat, a.Value));
            }
            return item;
        }
    }
}
