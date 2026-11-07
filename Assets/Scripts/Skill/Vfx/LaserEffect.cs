using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

namespace Game
{
    public partial class LaserEffect : ViewController
    {
        private List<Sprite> mFrames = new List<Sprite>();
        private Coroutine mPlayCoroutine;

        void Awake()
        {
            sr.transform.localRotation = Quaternion.Euler(0, 0, -90f);
        }

        public void Play(Texture2D tex, int tileX, int tileY, float duration, float length)
        {
            CutSpritesFromTexture(tex, tileX, tileY);
            transform.localScale = new Vector3(2f,length, 1f);

            // 3. 播放动画
            if (mPlayCoroutine != null) StopCoroutine(mPlayCoroutine);
            mPlayCoroutine = StartCoroutine(PlayAnimationRoutine(duration));
        }

        private void CutSpritesFromTexture(Texture2D tex, int tileX, int tileY)
        {
            mFrames.Clear();
            float frameWidth = (float)tex.width / tileX;
            float frameHeight = (float)tex.height / tileY;
            for (int y = tileY - 1; y >= 0; y--)
            {
                for (int x = 0; x < tileX; x++)
                {
                    Rect rect = new Rect(x * frameWidth, y * frameHeight, frameWidth, frameHeight);
                    Sprite sprite = Sprite.Create(tex, rect, new Vector2(0.5f, 0f), 100f);
                    mFrames.Add(sprite);
                }
            }
        }

        private IEnumerator PlayAnimationRoutine(float duration)
        {
            if (mFrames.Count == 0) yield break;
            if (mFrames.Count == 1)
            {
                sr.sprite = mFrames[0];
                yield break;
            }

            float frameTime = duration / mFrames.Count;
            int currentFrame = 0;

            while (currentFrame < mFrames.Count) // ★ 改成只播一次
            {
                sr.sprite = mFrames[currentFrame];
                currentFrame++;
                yield return new WaitForSeconds(frameTime);
            }
        }

        private void OnDestroy()
        {
            foreach (var frame in mFrames)
            {
                if (frame != null) Destroy(frame);
            }
            mFrames.Clear();
        }
    }
}
