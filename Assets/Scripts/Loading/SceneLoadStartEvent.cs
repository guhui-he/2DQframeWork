

namespace Game
{
    public struct SceneLoadStartEvent
    {
        public string SceneName;
        public SceneLoadStartEvent(string sceneName) => SceneName = sceneName;
    }

    public struct SceneLoadProgressEvent
    {
        public float Progress; // 0 ~ 1
        public SceneLoadProgressEvent(float progress) => Progress = progress;
    }

    public struct SceneLoadCompleteEvent
    {
        public string SceneName;
        public SceneLoadCompleteEvent(string sceneName) => SceneName = sceneName;
    }
}
