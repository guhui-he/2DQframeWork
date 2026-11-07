using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    [NodeWidth(300)]
    public class DialogueNode : BaseDialogueNode
    {
        [Output] public DialogueNode exit;
        public string speakerName;
        [UnityEngine.TextArea(3, 5)] public string content;
    }
}
