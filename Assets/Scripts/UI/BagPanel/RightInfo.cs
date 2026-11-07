using System.Data;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public partial class RightInfo : UIElement, IController
    {
        IHeroModel hero;
        private IUnRegister mPendingPointsUnregister;

        private void Awake()
        {
            hero = this.GetModel<IHeroModel>();
            var curHero = hero.GetCurHero();
            var pendingProp = curHero.PendingPoints;

            // ★ 安全订阅
            mPendingPointsUnregister = pendingProp.Register(e =>
            {
                // 当加点数变化时，重新计算属性
                RefreshInfo(hero.GetCurHero());
                RefreshUI(e > 0);
            });

            // 初始化一下 UI
            RefreshInfo(curHero);
            RefreshUI(pendingProp.Value > 0);

            // 按钮监听
            hpUp.onClick.AddListener(() => this.SendCommand(new UpgradeStatCommand(AffixStatType.MaxHp)));
            atkUp.onClick.AddListener(() => this.SendCommand(new UpgradeStatCommand(AffixStatType.Attack)));
            defUp.onClick.AddListener(() => this.SendCommand(new UpgradeStatCommand(AffixStatType.Defense)));
            critUp.onClick.AddListener(() => this.SendCommand(new UpgradeStatCommand(AffixStatType.Crit)));
            avdUp.onClick.AddListener(() => this.SendCommand(new UpgradeStatCommand(AffixStatType.Avd)));
            stepUp.onClick.AddListener(() => this.SendCommand(new UpgradeStatCommand(AffixStatType.Step)));
        }

        // 提供一个方法供外层在切换英雄时调用
        public void SwitchHero(RoleInfo newRole)
        {
            if (newRole == null) return;

            // 1. 注销旧英雄的加点监听
            mPendingPointsUnregister?.UnRegister();

            // 2. 订阅新英雄的加点
            mPendingPointsUnregister = newRole.PendingPoints.Register(e =>
            {
                RefreshInfo(newRole);
                RefreshUI(e > 0);
            });

            // 3. 刷新基础数据
            RefreshInfo(newRole);
            RefreshUI(newRole.PendingPoints.Value > 0);
        }

        void RefreshUI(bool visible)
        {
            hpUp.gameObject.SetActive(visible);
            atkUp.gameObject.SetActive(visible);
            defUp.gameObject.SetActive(visible);
            critUp.gameObject.SetActive(visible);
            stepUp.gameObject.SetActive(visible);
            avdUp.gameObject.SetActive(visible);
        }

        public void RefreshInfo(RoleInfo info)
        {
            if (info == null) return;

            // ★ 性能优化：只计算一次
            var finalStat = info.GetFinalStat();

            hp.text = finalStat.MaxHp.ToString();
            atk.text = finalStat.Attack.ToString();
            def.text = finalStat.Defense.ToString();
            crit.text = finalStat.Crit.ToString();
            avd.text = finalStat.Avd.ToString();
            step.text = finalStat.Step.ToString();
            txt.text = info.PendingPoints.Value.ToString();
        }

        public void RefreshTalent(RoleInfo info)
        {
            star.SetActive(true);

            foreach (var talent in info.Talents)
            {
                Transform parent = null;
                switch (talent.Key)
                {
                    case AffixStatType.Attack: parent = Atk; break;
                    case AffixStatType.Defense: parent = Def; break;
                    case AffixStatType.MaxHp: parent = Hp; break;
                    case AffixStatType.Crit: parent = Crit; break;
                    case AffixStatType.Avd: parent = Avd; break;
                    case AffixStatType.Step: parent = Step; break;
                    case AffixStatType.AtkRange:
                    default:
                        continue; // 跳过不需要显示星星的属性
                }

                if (parent != null)
                {
                    CreateStars(parent, talent.Value);
                }
            }

            star.SetActive(false);

        }
        private void CreateStars(Transform parent, int count)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                if (parent.GetChild(i).CompareTag("Star"))
                    Destroy(parent.GetChild(i).gameObject);
            }
            for (int i = 0; i < count; i++)
            {
                Instantiate(star, parent);
            }
            ActionKit.NextFrame(() =>
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(parent as RectTransform);
            }).StartGlobal();
        }

        private void OnDestroy()
        {
            mPendingPointsUnregister?.UnRegister();
            mPendingPointsUnregister = null;
        }
        protected override void OnBeforeDestroy() { }
        public IArchitecture GetArchitecture() => GameApp.Interface;
    }
}
