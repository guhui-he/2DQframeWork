using System.Collections.Generic;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IDialogueConfigModel : IModel
    {
        void AddConfig(DialogueConfig config);
        DialogueConfig GetDialogueConfig(int dialogueId);
    }

    public class DialogueConfigModel : AbstractModel, IDialogueConfigModel
    {
        private Dictionary<int, DialogueConfig> mConfig = new();

        protected override void OnInit()
        {
        }

        public DialogueConfig GetDialogueConfig(int dialogueId)
        {
            return mConfig.TryGetValue(dialogueId, out var path) ? path : null;
        }

        public void AddConfig(DialogueConfig config)
        {
            mConfig[config.Id] = config;
        }
    }
}
