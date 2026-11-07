using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;
using UnityEngine;

namespace Game
{
    public interface IGamePlayEventSystem:ISystem
    {
        void ExecuteEvent(int eventId, Action onComplete = null);
    }
    public class GamePlayEventSystem : AbstractSystem, IGamePlayEventSystem
    {
        public void ExecuteEvent(int eventId, Action onComplete = null)
        {
            var config=this.GetModel<IGamePlayEventModel>().GetConfig(eventId);
            var displayData = new EventDisPlayData
            {
                Description = config.Des,
            };
            switch (config.type)
            {
                case GamePlayEventType.AcceptQuest:
                    break;
                case GamePlayEventType.GiveItem:
                    Debug.Log(1121);
                    break;
                case GamePlayEventType.RecruitPartner:
                    break;
                case GamePlayEventType.OpenPanel:
                    this.SendEvent(new OpenPanelEvent
                    {
                        PanelId = (UIPanelId)config.Value1,
                        Params = new UIParams(extra: displayData),
                        OnClosed = () =>
                        {
                            Debug.Log($"[UI关闭] PanelId: {config.Value1} 关闭了，继续执行后续逻辑");
                            onComplete?.Invoke();
                        }
                    });
                    break;
            }
        }

        protected override void OnInit() { }
    }
}
