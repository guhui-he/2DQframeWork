using XNode;
namespace Game
{

    public abstract class BaseDialogueNode : Node
    {
        [Input] public BaseDialogueNode entry;

        public virtual BaseDialogueNode GetNextNode(string portName = "exit")
        {
            NodePort port = GetOutputPort(portName);
            if (port != null && port.IsConnected)
                return port.Connection.node as BaseDialogueNode;
            return null;
        }
    }
}
