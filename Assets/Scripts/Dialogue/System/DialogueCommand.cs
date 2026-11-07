
using QFramework;
using UnityEngine;

namespace Game
{
    public class DialogueCommand : AbstractCommand
    {
        int id;
        public DialogueCommand(int id) { this.id = id; }
        protected override void OnExecute()
        {
            var resSystem = this.GetSystem<IResSystem>();
            var path = this.GetModel<IDialogueConfigModel>().GetDialogueConfig(id).GraphPath;
            var graph = resSystem.GetGraph(path);
            UIKit.OpenPanel<DialoguePanel>();
            this.SendEvent(new StartDialogueEvent { Graph = graph });        
        }
    }
}
