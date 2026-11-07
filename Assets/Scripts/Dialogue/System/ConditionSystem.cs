using System;
using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IConditionSystem : ISystem
    {
        bool Evaluate(string expression);
    }
    public class ConditionSystem : AbstractSystem, IConditionSystem
    {
        private Dictionary<string, Func<object>> mAttributeGetters;
        private Dictionary<string, Func<string, object>> mParamGetters;
        protected override void OnInit()
        {
            mAttributeGetters = new Dictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Day", () => this.GetModel<ITimeModel>().Day },
                { "Hour", () => this.GetModel<ITimeModel>().Hour },
                { "TimePhase", () => {
                    int hour = this.GetModel<ITimeModel>().Hour;
                    return (hour >= 6 && hour < 18) ? "Day" : "Night";
                }}
            };
            mParamGetters = new Dictionary<string, Func<string, object>>(StringComparer.OrdinalIgnoreCase)
            {
                // 背包里的物品数量: Item_1001
                { "Item", (idStr) =>
                    {
                        int itemId = int.Parse(idStr);
                        var bagModel = this.GetModel<IBagModel>();
                        return bagModel.IsEquipped(itemId);
                    }
                },
                
            };
        }
        public bool Evaluate(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) return true;

            // 1. 如果包含 ||拆开分别判断
            if (expression.Contains("||"))
            {
                var orParts = expression.Split(new[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in orParts)
                {
                    if (EvaluateSingle(part.Trim())) return true; 
                }
                return false;
            }

            // 2. 如果包含 && 拆开分别判断
            if (expression.Contains("&&"))
            {
                var andParts = expression.Split(new[] { "&&" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in andParts)
                {
                    if (!EvaluateSingle(part.Trim())) return false;
                }
                return true;
            }

            // 3. 单一条件
            return EvaluateSingle(expression);
        }
        private bool EvaluateSingle(string expression)
        {
            // 定义符号，先匹配 >=，再匹配 >，防止被截断
            string[] operators = { ">=", "<=", "==", "!=", ">", "<" };
            string usedOp = null;

            foreach (var op in operators)
            {
                if (expression.Contains(op))
                {
                    usedOp = op;
                    break;
                }
            }

            // 如果没有符号，默认当作 "IsRich == true" 处理
            if (usedOp == null)
            {
                return ParseAndCompare(expression.Trim(), "==", "true");
            }

            var parts = expression.Split(new[] { usedOp }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2) return false;

            return ParseAndCompare(parts[0].Trim(), usedOp, parts[1].Trim());
        }
        private bool ParseAndCompare(string attrName, string op, string expectedValue)
        {
            object actualValue = null;

            // ★ 核心修改：判断是否带下划线
            if (attrName.Contains("_"))
            {
                var parts = attrName.Split('_');
                string baseName = parts[0];
                string param = parts[1];

                if (mParamGetters.TryGetValue(baseName, out var paramGetter))
                {
                    actualValue = paramGetter.Invoke(param);
                }
                else
                {
                    Debug.LogWarning($"[Condition] 未定义的带参属性: {baseName}");
                    return false;
                }
            }
            else
            {
                // 无参属性
                if (mAttributeGetters.TryGetValue(attrName, out var getter))
                {
                    actualValue = getter.Invoke();
                }
                else
                {
                    Debug.LogWarning($"[Condition] 未定义的属性: {attrName}");
                    return false;
                }
            }

            if (actualValue == null) return false;

            // 数字比较
            if (actualValue is int || actualValue is float || actualValue is bool)
            {
                float actualNum = Convert.ToSingle(actualValue);
                // 把 true/false 转成 1/0 方便比较
                float expectedNum = expectedValue.Equals("true", StringComparison.OrdinalIgnoreCase) ? 1f :
                                    expectedValue.Equals("false", StringComparison.OrdinalIgnoreCase) ? 0f :
                                    Convert.ToSingle(expectedValue);

                return op switch
                {
                    ">=" => actualNum >= expectedNum,
                    "<=" => actualNum <= expectedNum,
                    "==" => Mathf.Approximately(actualNum, expectedNum),
                    "!=" => !Mathf.Approximately(actualNum, expectedNum),
                    ">" => actualNum > expectedNum,
                    "<" => actualNum < expectedNum,
                    _ => false
                };
            }

            // 字符串/枚举比较
            string actualStr = actualValue.ToString();
            return op switch
            {
                "==" => actualStr.Equals(expectedValue, StringComparison.OrdinalIgnoreCase),
                "!=" => !actualStr.Equals(expectedValue, StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }
    }
}
