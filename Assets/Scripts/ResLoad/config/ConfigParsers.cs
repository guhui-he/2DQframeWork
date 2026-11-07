using System;
using System.Collections.Generic;


namespace Game
{
    public static class ConfigParsers
    {
        public static UIPanelConfig ParseUIPanel(Dictionary<string,string> row)
        {
            return new UIPanelConfig
            {
                Id = row.GetEnum("Id",UIPanelId.None),
                Name = row.GetString("Name"),
                PrefabName=row.GetString("PrefabName")
            };
        }
        public static RegionConfig ParseRegion(Dictionary<string,string> row)
        {
            return new RegionConfig
            {
                RegionId = row.GetInt("Id"),
                RegionName = row.GetString("Name"),
                NestPool = ParseListInt(row.GetString("NestPool")),
                MaxNestCount = row.GetInt("MaxNestCount")
            };
        }
        private static List<int> ParseListInt(string raw)
        {
            var result = new List<int>();
            raw = raw.Replace(" ", "").Replace("\t", "");
            foreach (var segment in raw.Split('|'))
            {
                result.Add(int.Parse(segment));
            }
            return result;
        }

        // ================= 城市 =================
        public static CityConfig ParseBuild(Dictionary<string, string> row)
        {
            return new CityConfig
            {
                Id = row.GetInt("Id"),
                Name = row.GetString("Name"),
                Des = row.GetString("Des"),
                SceneName=row.GetString("SceneName"),
                RolePool =ParseListInt(row.GetString("RolePool")),
                PartyNum=row.GetInt("PartyNum"),
                PartyPool=ParseListInt(row.GetString("PartyPool")),
                Faction=row.GetEnum("Faction",Faction.Nest)
            };
        }
        public static FactionRelationConfig ParseRelation(Dictionary<string , string> row)
        {
            return new FactionRelationConfig
            {
                Id = row.GetInt("Id"),
                FactionA = row.GetEnum("FactionA", Faction.Nest),
                FactionB = row.GetEnum("FactionB", Faction.Player),
                AttitudeType = row.GetEnum("Attitude", AttitudeType.Neutral)
            };
        }
        // ================= 队伍 =================
         
        public static PartyConfig ParseParty(Dictionary<string,string> row)
        {
            return new PartyConfig
            {
                Id = row.GetInt("Id"),
                Name = row.GetString("Name"),
                Type = row.GetEnum("PartyType", PartyType.Enemy),
                MaxMember = row.GetInt("MaxMember"),
                MoveSpeed = row.GetFloat("MoveSpeed"),
                MinRarity = row.GetInt("MinRarity"),
                MaxRarity = row.GetInt("MaxRaruty"),
                IconPath = row.GetString("IconPath"),
            };
        }
        // ================= 角色 =================
        public static RoleConfigData ParseRole(Dictionary<string, string> row)
        {
            return new RoleConfigData
            {
                Id = row.GetInt("Id"),
                Name = row.GetString("Name"),
                Icon = row.GetString("Icon"),
                Model = row.GetString("Model"),
                Info = row.GetString("Info"),
                Ani=row.GetString("Ani"),
                HpMin = row.GetInt("HpMin"),
                HpMax = row.GetInt("HpMax"),
                AtkMin = row.GetInt("AtkMin"),
                AtkMax = row.GetInt("AtkMax"),
                DefMin = row.GetInt("DefMin"),
                DefMax = row.GetInt("DefMax"),
                StepMin = row.GetInt("StepMin"),
                StepMax = row.GetInt("StepMax"),
                CritMin = row.GetInt("CritMin"),
                CritMax = row.GetInt("CritMax"),
                AvdMin = row.GetInt("AvdMin"),
                AvdMax = row.GetInt("AvdMax")
            };
        }

        // ================= 效果 =================
        public static EffectConfig ParseEffect(Dictionary<string, string> row)
        {
            return new EffectConfig
            {
                Id = row.GetInt("Id"),
                Type = row.GetEnum("Type", EffectType.Damage),
                Value1 = row.GetFloat("Value1"),
                Value2 = row.GetFloat("Value2"),
                Duration = row.GetInt("Duration"),
                Params = row.GetString("Params")
            };
        }

        // ================= 特效 =================
        public static VfxConfig ParseVfx(Dictionary<string, string> row)
        {
            return new VfxConfig
            {
                Id = row.GetInt("Id"),
                Name = row.GetString("Name"),
                Path = row.GetString("Path"),
                TileX = row.GetInt("TileX"),
                TileY = row.GetInt("TileY"),
                OffsetX = row.GetFloat("OffsetX"),
                OffsetY = row.GetFloat("OffsetY"),
                Speed = row.GetInt("Speed"),
                Timer = row.GetFloat("Timer"),
                PrefabPath = row.GetString("PrefabPath")
            };
        }

        // ================= Buff =================
        public static BuffConfig ParseBuff(Dictionary<string,string> row)
        {
            return new BuffConfig
            {
                Id = row.GetInt("Id"),
                Name = row.GetString("Name"),
                Icon = row.GetString("Icon"),
                Des = row.GetString("Des"),
                IsDebuff = row.GetBool("IsDebuff"),
                Duration = row.GetInt("Duration"),
                TickTiming = row.GetEnum("TickTiming", BuffTickTiming.None),
                TickEffectId = row.GetInt("TickEffectId"),
                TickValue = row.GetFloat("TickValue"),
                TickVfxId = row.GetInt("TickVfxId"),
                Modifiers = ParseStatModifier(row.GetString("Modifiers"))
            };
        }
        public static List<StatModifier> ParseStatModifier(string raw)
        {
            var result = new List<StatModifier>();
            if (string.IsNullOrEmpty(raw) || raw == "-" || raw == "None")
                return result;
            raw = raw.Replace(" ", "").Replace("\t", "");
            foreach (var segment in raw.Split('|'))
            {
                var parts = segment.Split(':');

                result.Add(new StatModifier
                {
                    StatType = (AffixStatType)System.Enum.Parse(typeof(AffixStatType), parts[0], true),
                    Type = (StatModifyType)System.Enum.Parse(typeof(StatModifyType), parts[1], true),
                    Value = float.Parse(parts[2], System.Globalization.CultureInfo.InvariantCulture)
                });
            }

            return result;
        } 
        // ================= 技能 =================
        public static SkillConfig ParseSkill(Dictionary<string, string> row, IEffectConfigModel effectModel)
        {
            return new SkillConfig
            {
                id = row.GetInt("Id"),
                name = row.GetString("Name"),
                des = row.GetString("Des"),
                icon = row.GetString("Icon"),
                CastMode = row.GetEnum("SkillType", SkillCastMode.Normal),
                targetType = row.GetEnum("TargetType", SkillTargetType.Self),
                targetCategory = row.GetEnum("TargetCategory", SkillTargetCategory.Enemy),
                cooldown = row.GetInt("Cooldown"),
                atkRange = row.GetInt("Range"),
                maxLevel = row.GetInt("MaxLevel"),
                CastVfxId = row.GetInt("CastVfxId"),
                ProjectileVfxId = row.GetInt("ProjectileVfxId"),
                HitVfxId = row.GetInt("HitVfxId"),
                Effects = ParseSkillEffects(row.GetString("EffectIds"), effectModel)
            };
        }

        private static List<EffectConfig> ParseSkillEffects(string raw, IEffectConfigModel effectModel)
        {
            var result = new List<EffectConfig>();
            if (string.IsNullOrEmpty(raw) || raw == "-") return result;

            foreach (var token in raw.Split('|'))
            {
                if (int.TryParse(token.Trim(), out int id))
                {
                    var cfg = effectModel.GetEffect(id);
                    if (cfg != null) result.Add(cfg);
                }
            }
            return result;
        }

        // ================= 事件 =================

        public static GamePlayEventConfig ParseEvent(Dictionary<string, string> row)
        {
            return new GamePlayEventConfig
            {
                Id = row.GetInt("Id"),
                type=row.GetEnum("Type",GamePlayEventType.AcceptQuest),
                Value1=row.GetInt("Param1"),
                Value2=row.GetInt("Param2"),
                Des=row.GetString("Des")
            };
        }
        // ================= 对话 =================
        public static DialogueConfig ParseDialogue(Dictionary<string, string> row)
        {
            return new DialogueConfig
            {
                Id = row.GetInt("Id"),
                GraphPath = row.GetString("GraphPath")
            };
        }
        // ================= 物品 =================
        public static ItemConfigData ParseItem(Dictionary<string, string> row, ItemType type)
        {
            return new ItemConfigData
            {
                Id = row.GetInt("Id"),
                Type = type,
                Name = row.GetString("Name"),
                Icon = row.GetString("Icon"),
                Price = row.GetInt("Price"),
                Slot = row.GetString("Slot"),
                Level = row.GetInt("Level")
            };
        }
    }
}
