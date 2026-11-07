using System.Collections.Generic;
using System.Linq;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface ISettlementModel : IModel
    {
        Dictionary<int, SettlementInfo> Settlements { get; }
        void AddSettlement(SettlementInfo data);
        void RemoveSettlement(int id);
        SettlementInfo GetSettlement(int id);
        bool IsPosOccupied(Vector2Int pos);
    }

    public class SettlementModel : AbstractModel, ISettlementModel
    {
        public Dictionary<int, SettlementInfo> Settlements { get; } = new();

        protected override void OnInit() { }

        public void AddSettlement(SettlementInfo data) => Settlements[data.Id] = data;
        public void RemoveSettlement(int id) => Settlements.Remove(id);
        public SettlementInfo GetSettlement(int id) => Settlements.TryGetValue(id, out var data) ? data : null;

        public bool IsPosOccupied(Vector2Int pos) => Settlements.Values.Any(s => s.Pos == pos);
    }
}
