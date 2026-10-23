using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;

namespace Game
{
    public interface IResModel : IModel
    {
        BindableList<string> AtlasPaths { get; }
        void AddAtlasPath(string path);
    }

    public class ResModel : AbstractModel, IResModel
    {
        public BindableList<string> AtlasPaths { get; } = new BindableList<string>();
        protected override void OnInit()
        {
            
        }
        public void AddAtlasPath(string path)
        {
            AtlasPaths.Add(path);
        }
    }
}
