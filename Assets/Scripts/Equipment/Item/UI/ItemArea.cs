
using Game;
using QFramework;
using UnityEngine;

public class ItemArea : MonoBehaviour,IController
{
    public ItemType acceptType;
    public AreaType areaType;

    public IArchitecture GetArchitecture() => GameApp.Interface;

    private void Start()
    {
        
    }
}
