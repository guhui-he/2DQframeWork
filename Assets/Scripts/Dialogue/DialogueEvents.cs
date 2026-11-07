using System.Collections.Generic;

namespace Game
{
    public struct StartDialogueEvent
    {
        public DialogueGraph Graph;
    }
    public struct ShowDialogueLineEvent
    {
        public string SpeakerName;
        public string Content;
        public bool IsWaitingForNext; // 是否显示“下一步”按钮（而不是选项）
    }
    public struct NextButtonClickedEvent { }
    public struct OptionButtonClickedEvent { public int Index; }
    // 通知 UI 显示选项
    public struct ShowChoicesEvent
    {
        public List<string> Choices;
    }

    // 通知 UI 关闭对话面板
    public struct EndDialogueEvent { }
}
