using QFramework;

namespace Game
{
    // 万能参数盒（根据项目需求自行扩充字段）
    public struct UIParams
    {
        public int IntParam1;
        public int IntParam2;
        public string StringParam1;
        public object ExtraData; 

        public UIParams(int int1 = 0, int int2 = 0, string str = null, object extra = null)
        {
            IntParam1 = int1;
            IntParam2 = int2;
            StringParam1 = str;
            ExtraData = extra;
        }
    }
    public class GlobalUIData : UIPanelData
    {
        public UIParams Params;
        public GlobalUIData(UIParams p) { Params = p; }
    }
}
