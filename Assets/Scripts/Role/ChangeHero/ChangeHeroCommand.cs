using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QFramework;

namespace Game
{
    public class ChangeHeroCommand : AbstractCommand
    {
        private int _id;
        public ChangeHeroCommand(int curHeroId)
        {
            _id = curHeroId;
        }
        protected override void OnExecute()
        {
            this.GetSystem<IChangeHeroSystem>().ChangeHero(_id);
        }
    }
}
