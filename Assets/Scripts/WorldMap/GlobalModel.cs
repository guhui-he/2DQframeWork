using QFramework;

namespace Game
{
    public interface IGlobalModel : IModel
    {
        int mNextSettementId { get; set; }
        int mNextPartyId { get; set; }
        int GetNextPartyId();
        int GetNextSettlementId();
        void SetNextPartyId(int i);
        void SetNextSettlementId(int i);
    }
    public class GlobalModel : AbstractModel, IGlobalModel
    {
        public int mNextSettementId { get; set; } = 1;
        public int mNextPartyId { get; set; } =1;

        public int GetNextPartyId()=>mNextPartyId++;

        public int GetNextSettlementId()=>mNextSettementId++;

        protected override void OnInit() { }

        void IGlobalModel.SetNextPartyId(int i) => mNextPartyId = i;

        void IGlobalModel.SetNextSettlementId(int i)=>mNextSettementId=i;
    }
}
