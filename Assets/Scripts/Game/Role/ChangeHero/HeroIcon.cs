
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public struct BattleBeginDragEvent
    {
        public int id;
        public BattleBeginDragEvent(int id) { this.id = id; } 
    }

    public struct BattleEndDragEvent { public int id; }
	public partial class HeroIcon : ViewController,IController, IEndDragHandler, IBeginDragHandler, IDragHandler,ICanSendEvent
    {
		public int curHeroId;
		public IArchitecture GetArchitecture() => GameApp.Interface;
        private IHeroModel model;
        void Start()
		{
			SelfButton.onClick.AddListener(() =>
			{
				//发commnad?
				this.SendCommand(new ChangeHeroCommand(curHeroId));
			});
            model = this.GetModel<IHeroModel>();
        }
		public void SetIcon(RoleInfo hero)
		{
			curHeroId = hero.Id;
			Tools.SetSprite(icon,AssetPathDef.AtlasRole, hero.Icon);
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            this.SendEvent(new BattleBeginDragEvent(curHeroId));
        }

        public void OnDrag(PointerEventData eventData)
        {

        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            var col = Tools.ScreenPointToRay2D(Camera.main, eventData.position);
            if (col != null)
            {
                block b = col.GetComponent<block>();
                if (b != null)
                {
                    var role = model.GetHero(curHeroId);
                    var pos = new GridPos(b.RowIndex, b.ColIndex);

                    // 1. 创建逻辑数据
                    this.SendCommand(new SpawnUnitCommand(role, pos, true));
                    this.SendEvent(new BattleEndDragEvent());
                    // 2. 创建视图
                    var loader = this.GetUtility<IPrefabLoaderUtility>();
                    var obj = loader.Instantiate(role.Model);
                    if (BlockViewRegistry.TryGetWorldPosition(pos, out var wp))
                        obj.transform.position = new Vector3(wp.x, wp.y, -1);

                    // 3. 绑定 View 与 Data
                    var unit = this.GetModel<IBattleModel>().GetUnitAt(pos);
                    obj.GetComponent<Hero>().UnitId = unit.UnitId;

                    Destroy(this.gameObject);
                }
            }
        }
    }
}
