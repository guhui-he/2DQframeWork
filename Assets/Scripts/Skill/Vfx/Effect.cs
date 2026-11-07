using UnityEngine;
using QFramework;

namespace Game
{
    public partial class Effect : ViewController
    {
        private ParticleSystemRenderer mRenderer;

        void Awake()
        {
            mRenderer = GetComponent<ParticleSystemRenderer>();
        }

        public void Play(Texture2D tex, int tilex, int tiley = 1, float duration = 0.4f, bool isLoop = false)
        {
            if (tex != null && mRenderer != null)
            {
                var mat = mRenderer.material;
                if (mat.HasProperty("_MainTex")) mat.mainTexture = tex;
                if (mat.HasProperty("_BaseMap")) mat.SetTexture("_BaseMap", tex);
            }
            var main = ps.main;
            var texSheet = ps.textureSheetAnimation;
            texSheet.numTilesX = tilex;
            texSheet.numTilesY = tiley;
            main.startLifetime = duration;
            main.loop = isLoop;

            ps.Clear();
            ps.Play();
        }
    }
}
