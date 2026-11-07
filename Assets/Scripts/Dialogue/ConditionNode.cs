using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class ConditionNode : BaseDialogueNode
    {
        public string conditionExpression;

        [Output] public BaseDialogueNode trueNode;
        [Output] public BaseDialogueNode falseNode;
    }
}
