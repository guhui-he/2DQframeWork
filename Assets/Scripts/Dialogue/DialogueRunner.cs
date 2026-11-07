using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public partial class DialogueRunner : MonoBehaviour, IController,ICanSendEvent
    {
        public IArchitecture GetArchitecture() => GameApp.Interface; 

        private DialogueGraph mCurrentGraph;
        private BaseDialogueNode mCurrentNode;
        private BaseDialogueNode mPendingNextNode;
        void Start()
        {
            this.RegisterEvent<StartDialogueEvent>(OnStartDialogue)
                .UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<NextButtonClickedEvent>(_ => OnNextClicked()).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<OptionButtonClickedEvent>(e => OnChoiceSelected(e.Index)).UnRegisterWhenGameObjectDestroyed(gameObject);
            
        }

        private void OnStartDialogue(StartDialogueEvent e)
        {
            mCurrentGraph = e.Graph;
            if (mCurrentGraph == null || mCurrentGraph.StartNode == null) return;
            mCurrentNode = mCurrentGraph.StartNode as BaseDialogueNode;
            ProcessNode();
        }
        private void ProcessNode()
        {
            if (mCurrentNode == null)
            {
                this.SendEvent<EndDialogueEvent>();
                return;
            }
            switch (mCurrentNode)
            {
                case DialogueNode dialogueNode:
                    this.SendEvent(new ShowDialogueLineEvent
                    {
                        SpeakerName = dialogueNode.speakerName,
                        Content = dialogueNode.content,
                        IsWaitingForNext = true
                    });
                    break;

                case ChoiceNode choiceNode:
                    this.SendEvent(new ShowDialogueLineEvent
                    {
                        SpeakerName = choiceNode.speakerName,
                        Content = choiceNode.content,
                        IsWaitingForNext = false
                    });
                    this.SendEvent(new ShowChoicesEvent { Choices = choiceNode.choices });
                    break;
                case EventNode eventNode:
                    mPendingNextNode = eventNode.GetNextNode("exit");
                    this.GetSystem<IGamePlayEventSystem>().ExecuteEvent(eventNode.EventId, () =>
                    {
                        if (mPendingNextNode != null)
                        {
                            mCurrentNode = mPendingNextNode;
                            mPendingNextNode = null;
                            ProcessNode();
                        }
                    });
                    return;
                case ConditionNode conditionNode:
                    bool conditionMet = this.GetSystem<IConditionSystem>().Evaluate(conditionNode.conditionExpression);
                    mCurrentNode = conditionMet ? conditionNode.GetNextNode("trueNode") : conditionNode.GetNextNode("falseNode");
                    ProcessNode();
                    break;
            }
        }
        public void OnNextClicked()
        {
            if (mCurrentNode is DialogueNode dialogueNode)
            {
                mCurrentNode = dialogueNode.GetNextNode("exit");
                ProcessNode();
            }
        }

        // 外部调用：玩家点击了某个选项
        public void OnChoiceSelected(int index)
        {
            if (mCurrentNode is ChoiceNode choiceNode)
            {
                mCurrentNode = choiceNode.GetNodeByChoiceIndex(index);
                ProcessNode();
            }
        }
    }
   
}
