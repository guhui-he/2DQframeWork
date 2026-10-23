using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;

namespace Game
{
    public class BuyHeroCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetSystem<ISillyTavenSystem>().BuyHero();
        }
    }
}
