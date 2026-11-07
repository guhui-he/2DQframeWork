using System.Collections.Generic;
using UnityEngine;
using XNode;

// 1. 对话图容器
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue Graph")]
public class DialogueGraph : NodeGraph
{
    // 可以从外部指定起始节点
    public Node StartNode;
}

