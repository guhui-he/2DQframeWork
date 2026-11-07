using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class AttackAction : IBattleAction
    {
        // ★ 增加一个飞行阶段
        private enum Step { Start, WaitBeforeHit, ProjectileFlying, WaitAfterHit, Done }

        private readonly BattleUnitData mAttacker;
        private readonly List<BattleUnitData> mTargets;
        private readonly SkillInfo mSkill;
        private readonly GridPos mTargetPos;
        private Step mStep;
        private float mTimer;

        public AttackAction(BattleUnitData attacker, List<BattleUnitData> targets, SkillInfo skill, GridPos targetPos)
        {
            mAttacker = attacker;
            mTargets = targets;
            mSkill = skill;
            mTargetPos = targetPos;
        }

        public void OnStart()
        {
            mStep = Step.Start;
            if (mSkill.Config.cooldown > 0) mSkill.RemainingCd.Value = mSkill.Config.cooldown;
        }

        public bool OnUpdate(float dt)
        {
            if (mStep == Step.Done) return true;

            switch (mStep)
            {
                case Step.Start:
                    GameApp.Interface.SendEvent(new UnitAttackEvent { AtkId = mAttacker.UnitId, SkillId = mSkill.Config.id });

                    // 只传 VfxId，不传对象
                    if (mSkill.Config.CastVfxId > 0)
                    {
                        GameApp.Interface.SendEvent(new PlayVfxEvent
                        {
                            VfxId = mSkill.Config.CastVfxId,
                            TargetId = mAttacker.UnitId,
                            TargetPos = mTargetPos,
                        });
                    }

                    mTimer = 1.3f;
                    mStep = Step.WaitBeforeHit;
                    break;

                case Step.WaitBeforeHit:
                    mTimer -= dt;
                    if (mTimer <= 0)
                    {
                        // 只传 VfxId
                        if (mSkill.Config.ProjectileVfxId > 0)
                        {
                            GameApp.Interface.SendEvent(new SpawnProjectileEvent
                            {
                                VfxId = mSkill.Config.ProjectileVfxId,
                                FromId = mAttacker.UnitId,
                                Range=mSkill.Config.atkRange,
                                ToPos = mTargetPos
                            });
                            var vfxConfig = GameApp.Interface.GetModel<IVfxModel>().GetVfxConfig(mSkill.Config.ProjectileVfxId);
                            float speed = (vfxConfig != null && vfxConfig.Speed > 0) ? vfxConfig.Speed : 10f;

                            float distance = mAttacker.Pos.ManhattanTo(mTargetPos);
                            mTimer = distance / speed;
                            mStep = Step.ProjectileFlying;
                        }
                        else
                        {
                            ExecuteHit();
                        }
                    }
                    break;

                case Step.ProjectileFlying:
                    mTimer -= dt;
                    if (mTimer <= 0)
                    {
                        // 3. 飞行物到达终点，执行命中
                        ExecuteHit();
                    }
                    break;

                case Step.WaitAfterHit:
                    mTimer -= dt;
                    if (mTimer <= 0)
                    {
                        if (mTargets != null)
                        {
                            foreach (var target in mTargets)
                            {
                                if (target != null)
                                    GameApp.Interface.SendEvent(new UnitDamagedCompletedEvent { TargetId = target.UnitId });
                            }
                        }
                        Done();
                    }
                    break;
            }
            return mStep == Step.Done;
        }

        private void ExecuteHit()
        {
            if (mSkill.Config.HitVfxId > 0)
            {
                GameApp.Interface.SendEvent(new PlayVfxEvent
                {
                    VfxId = mSkill.Config.HitVfxId,
                    TargetPos = mTargetPos
                });
            }

            if (mSkill.Effects != null)
            {
                foreach (var effect in mSkill.Effects)
                {
                    effect.Execute(mAttacker, mTargets, mTargetPos);
                }
            }

            mTimer = 0.3f; // 后摇
            mStep = Step.WaitAfterHit;
        }

        public void OnCancel() => Done();

        private void Done()
        {
            if (mAttacker != null && mAttacker.IsAlive)
            {
                mAttacker.HasActed = true;
                mAttacker.HasMoved = true;
            }
            mStep = Step.Done;
        }
    }
}
