using QFramework;
namespace Game
{
	public class StorePanelData : UIPanelData
	{
        public int buildId;
	}
	public partial class StorePanel : UIPanel,IController
	{
		private IShopModel mShopModel;
        private IBagModel mBagModel;
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as StorePanelData ?? new StorePanelData();
            close.onClick.AddListener(CloseSelf);

            mShopModel = this.GetModel<IShopModel>();
            var mBindList = mShopModel.Items[mData.buildId];
            mBindList.OnAdd.Register((index, item) =>
            {
                StoreContent.Append(item);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            mBindList.OnRemove.Register((index, item) =>
            {
                StoreContent.Remove(item);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            mBindList.OnClear.Register(() =>
            {
                StoreContent.Clear();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            mBagModel=this.GetModel<IBagModel>();
            var mBagList = mBagModel.GetBindableList();
            mBagList.OnAdd.Register((index, item) =>
            {
                BagContent.Append(item);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            mBagList.OnRemove.Register((index, item) =>
            {
                BagContent.Remove(item);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            mBagList.OnClear.Register(() =>
            {
                BagContent.Clear();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        protected override void OnOpen(IUIData uiData = null)
        {
            RefreshAll();
        }

        private void RefreshAll()
        {
            StoreContent.Clear();
            for (int i = 0; i < mShopModel.Items[mData.buildId].Count; i++)
            {
                StoreContent.Append(mShopModel.Items[mData.buildId][i]);
            }
            BagContent.Clear();
            for(int i = 0;i < mBagModel.Items.Count;i++)
            {
                BagContent.Append(mBagModel.Items[i]);
            }
        }
        protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

		public IArchitecture GetArchitecture() => GameApp.Interface;

    }
}
