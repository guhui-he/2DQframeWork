using QFramework;
using UnityEngine;
namespace Game
{
    public class GameApp : Architecture<GameApp>
    {
        protected override void Init()
        {
            Application.targetFrameRate = 60;
            ResKit.Init();
            RegisterUtility<IPrefabLoaderUtility>(new PrefabLoaderUtility());
            RegisterModel();
            RegisterSystem();
        }
        void RegisterModel()
        {
            RegisterModel<ITimeModel>(new TimeModel());
            RegisterModel<IConfigModel>(new ConfigModel());
            RegisterModel<IResModel>(new ResModel());
            RegisterModel<IGameStartModel>(new GameStartModel());   
            RegisterModel<IPlayerModel>(new PlayerModel());
            RegisterModel<ICameraModel>(new CameraModel());
            RegisterModel<ISpawnEnemyModel>(new  SpawnEnemyModel());
            RegisterModel<IEnemyIconModel>(new EnemyIconModel());
            RegisterModel<IBuildModel>(new BuildModel());
            RegisterModel<ISillyTavernModel>(new  SillyTavernModel());
            RegisterModel<IShopModel>(new  ShopModel());
            RegisterModel<IItemConfigModel>(new ItemConfigModel());
            RegisterModel<IRoleConfigModel>(new RoleConfigModel());
            RegisterModel<IBagModel>(new  BagModel());
            RegisterModel<IDragModel>(new DragModel());
            RegisterModel<IHeroModel>(new HeroModel());
            RegisterModel<IEnemyModel>(new EnemyModel());
            RegisterModel<IMapModel>(new MapModel());
            RegisterModel<IBattleModel>(new BattleModel());
            RegisterModel<ILootModel>(new LootModel());
        }
        void RegisterSystem()
        {
            RegisterSystem<ITimeSystem>(new TimeSystem());
            RegisterSystem<IResSystem>(new  ResSystem());
            RegisterSystem<IInitDataSystem>(new InitDataSystem());
            RegisterSystem<ISaveSystem>(new SaveSystem());
            RegisterSystem<IRandomItemSystem>(new RandomItemSystem());
            RegisterSystem<IAffixGeneratorSystem>(new AffixGeneratorSystem());
            RegisterSystem<IEquipmentFactorySystem>(new EquipmentFactorySystem());

            RegisterSystem<IGameStartSystem>(new GameStartSystem());
            RegisterSystem<ILoadingSystem>(new LoadingSystem());
            RegisterSystem<IUISystem>(new  UISystem());

            RegisterSystem<IPlayerMoveSystem>(new PlayerMoveSystem());
            RegisterSystem<IPlayerInputSystem>(new PlayerInputSystem());
            RegisterSystem<IBuildTriggerSystem>(new BuildTriggerSystem());
            RegisterSystem<IEnemyTriggerSystem>(new EnemyTriggerSystem());
            RegisterSystem<ICameraSystem>(new CameraSystem());
            RegisterSystem<ISpawnEnemySystem>(new  SpawnEnemySystem()); 
            RegisterSystem<IEnemyMoveSystem>(new EnemyMoveSystem());
            RegisterSystem<IEnemyPerceptionSystem>(new EnemyPerceptionSystem());
            RegisterSystem<IEnemyBattleAISystem>(new EnemyBattleAISystem());

            RegisterSystem<IRoleFactorySystem>(new RoleFactorySystem());
            RegisterSystem<IEnterTownSystem>(new EnterTownSystem());
            RegisterSystem<IShopSystem>(new ShopSystem());
            RegisterSystem<ISillyTavenSystem>(new SillyTavernSystem());
            RegisterSystem<IDragDropRouterSystem>(new DragDropRouterSystem());
            RegisterSystem<IEquipmentSystem>(new EquipmentSystem());
            RegisterSystem<IEnemyFactorySystem>(new EnemyFactorySystem());

            RegisterSystem<IBagSystem>(new BagSystem());
            RegisterSystem<IChangeHeroSystem>(new  ChangeHeroSystem());

            RegisterSystem<IMapGenerateSystem>(new  MapGenerateSystem());
            RegisterSystem<IPathfindingSystem>(new PathfindingSystem());
            RegisterSystem<IActionQueueSystem>(new ActionQueueSystem());
            RegisterSystem<IBattleTurnSystem>(new TurnSystem());
            RegisterSystem<IPathPreviewSystem>(new PathPreviewSystem());   
            RegisterSystem<ICombatSystem>(new CombatSystem());
            RegisterSystem<IBattleSystem>(new BattleSystem());
            RegisterSystem<IAISystem>(new AISystem());
            RegisterSystem<IDropSystem>(new DropSystem());
            RegisterSystem<ILootSystem>(new  LootSystem());
        }
        
    }
}
