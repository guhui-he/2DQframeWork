using System.Collections.Generic;
using QFramework;

namespace Game
{
    

    public class DialoguePanelData : UIPanelData
    {
    }

    public partial class DialoguePanel : TimePausingPanel, ICanSendEvent
    {
        private ComponentPool<choicePrefab> _choicesBtnPool;
        private readonly List<choicePrefab> _activeBtns = new List<choicePrefab>();

        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as DialoguePanelData ?? new DialoguePanelData();
            _choicesBtnPool = new ComponentPool<choicePrefab>(choicePrefab, root);

            // ×¢²áÊÂ¼þ
            this.RegisterEvent<ShowDialogueLineEvent>(OnShowLine).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<ShowChoicesEvent>(OnShowChoices).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<EndDialogueEvent>(OnEnd).UnRegisterWhenGameObjectDestroyed(gameObject);
            NextBtn.onClick.AddListener(() =>
            {
                this.SendEvent<NextButtonClickedEvent>();
            });
        }

        void OnShowLine(ShowDialogueLineEvent e)
        {

            speakerTxt.text = e.SpeakerName;
            contentTxt.text = e.Content;
            NextBtn.gameObject.SetActive(e.IsWaitingForNext);
            if (e.IsWaitingForNext)
            {
                ClearChoices();
            }
        }

        void OnShowChoices(ShowChoicesEvent e)
        {
            NextBtn.gameObject.SetActive(false);

            ComponentPoolUtil.Refresh(
                _activeBtns,
                _choicesBtnPool,
                e.Choices,
                (btn, choiceText) =>
                {
                    btn.txt.text = choiceText;
                    btn.btn.onClick.RemoveAllListeners();
                    int index = _activeBtns.IndexOf(btn);

                    btn.btn.onClick.AddListener(() =>
                    {
                        this.SendEvent(new OptionButtonClickedEvent { Index = index });
                    });
                });
        }

        private void OnEnd(EndDialogueEvent e)
        {       
            ClearChoices();
            CloseSelf();
        }

        private void ClearChoices()
        {
            ComponentPoolUtil.Refresh<choicePrefab, string>(
                _activeBtns,
                _choicesBtnPool,
                null,
                null);
        }

        protected override void OnOpen(IUIData uiData = null)
        {
            base.OnOpen(uiData);
        }
        protected override void OnShow() { }
        protected override void OnHide() { }
        protected override void OnClose()
        {
            base.OnClose();
        }
    }
}
