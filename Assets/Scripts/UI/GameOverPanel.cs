
using QFramework;

namespace Game
{
	public class GameOverPanelData : UIPanelData
	{
	}
	public partial class GameOverPanel : UIPanel,IController
	{
		ILootModel lootModel;
		IBagModel bagModel;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as GameOverPanelData ?? new GameOverPanelData();
			// please add init code here
			lootModel = this.GetModel<ILootModel>();
			bagModel = this.GetModel<IBagModel>();

			bagModel.GetBindableList().OnAdd.Register((index, item) =>
			{
				BgContent.Append(item);
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
			bagModel.GetBindableList().OnRemove.Register((index, item) =>
			{
				BgContent.Remove(item);
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
			lootModel.GetBindableList().OnAdd.Register((index, item) =>
			{
				LootContent.Append(item);
			}).UnRegisterWhenGameObjectDestroyed(gameObject);
			lootModel.GetBindableList().OnRemove.Register((index, item) =>
			{
				LootContent.Remove(item);
			}).UnRegisterWhenGameObjectDestroyed(gameObject);

        }
		
		protected override void OnOpen(IUIData uiData = null)
		{
            LootContent.Clear();
            for (int i = 0; i < lootModel.Items.Count; i++)
            {
                LootContent.Append(lootModel.Items[i]);
            }
            BgContent.Clear();
            for (int i = 0; i < bagModel.Items.Count; i++)
            {
                BgContent.Append(bagModel.Items[i]);
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
