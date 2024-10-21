using UnityEngine;
using System.Collections;

namespace JuicyInternal
{
    public class JuicyEmulatedInterstitial : JuicyEmulatedElement
    {
        #pragma warning disable CS0649
        [SerializeField] CanvasGroup canvasGroup;
        #pragma warning restore CS0649

        float animationDuration = .5f;
        Coroutine ShowAnimationCoroutine;

        public override void Show()
        {
            visualTransform.anchoredPosition = new Vector2(0, -Screen.height);
            canvasGroup.alpha = 0;

            base.Show();

            if (JuicySDKSettings.Instance.SkipAdsInEditor)
                Close();

            ShowAnimation();
        }

        public override void Close()
        {
            if (ShowAnimationCoroutine != null)
                StopCoroutine(ShowAnimationCoroutine);

            base.Close();
        }

        void ShowAnimation()
        {
            if (ShowAnimationCoroutine != null)
                StopCoroutine(ShowAnimationCoroutine);
            ShowAnimationCoroutine = StartCoroutine(EShowAnimation());
        }

        IEnumerator EShowAnimation()
        {
            float elapsedTime = 0;
            while(elapsedTime < animationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                visualTransform.anchoredPosition = Vector2.Lerp(visualTransform.anchoredPosition, new Vector2(0, 0), elapsedTime / animationDuration);
                canvasGroup.alpha = JuicyUtility.Remap((elapsedTime / animationDuration),0,1,.7f,1);
                yield return null;
            }
            visualTransform.anchoredPosition = Vector2.zero;
            canvasGroup.alpha = 1;
        }
    }
}
