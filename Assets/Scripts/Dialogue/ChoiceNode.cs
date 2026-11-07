using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace Game
{
    [NodeWidth(250)]
    public class ChoiceNode : BaseDialogueNode
    {
        public string speakerName;
        [TextArea(3, 5)] public string content;
        [Output(dynamicPortList = true)] public List<string> choices = new List<string>();

        public BaseDialogueNode GetNodeByChoiceIndex(int index)
        {
            NodePort port = GetOutputPort($"choices {index}");
            if (port != null && port.IsConnected)
                return port.Connection.node as BaseDialogueNode;
            return null;
        }
    }
}
