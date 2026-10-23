using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IDropSystem : ISystem { }

    public class DropSystem : AbstractSystem, IDropSystem
    {
        private IBattleModel mBattleModel;
        private ILootModel mLootModel;
        protected override void OnInit()
        {
            mBattleModel = this.GetModel<IBattleModel>();
            mLootModel=this.GetModel<ILootModel>();
            //  监听死亡事件，执行掉落逻辑
            this.RegisterEvent<UnitDieEvent>(e =>
            {
                
                var unit = mBattleModel.AllUnits.FirstOrDefault(u => u.UnitId == e.UnitId);
             
                TryDrop(unit);
            });
        }

        private void TryDrop(BattleUnitData unit)
        {
            var equipment = unit.SourceInfo.Equipment;
            if (equipment == null || equipment.Slots == null) return;

            foreach (var slot in equipment.Slots.Values)
            {
                var item = slot.Value;
                if (item != null&& Random.Range(0f, 1f) <0.3f)
                {
                    mLootModel.AddItem(item);
                    Debug.Log($"获得装备{item.itemData.Name}");
                }
                Debug.Log("没有获得装备");
            }
        }
    }
}
