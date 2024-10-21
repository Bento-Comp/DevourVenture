using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace JuicyInternal
{
    public class JuicyEmulatedRewarded : JuicyEmulatedElement
    {
        #pragma warning disable 0649
        [SerializeField] Image TimeIndicator;
        [SerializeField] GameObject QuitModal;
        [SerializeField] CanvasGroup canvasGroup;
        #pragma warning restore 0649

        float animationDuration = .5f;
        float delay = 3;
        bool hasWaited = false;
        bool isPaused = false;

        Coroutine delayCoroutine;
        Coroutine ShowAnimationCoroutine;

        public System.Action<bool> OnClosedSuccess;

        protected override void Awake()
        {
            base.Awake();
            QuitModal.SetActive(false);
        }

        public override void Show()
        {
            visualTransform.anchoredPosition = new Vector2(0, -Screen.height);
            canvasGroup.alpha = 0;

            base.Show();

            delayCoroutine = StartCoroutine(EDelay());

            if (JuicySDKSettings.Instance.SkipAdsInEditor)
            {
                hasWaited = true;
                Close();
            }

            ShowAnimation();
        }

        public override void Close()
        {
            if (delayCoroutine != null)
                StopCoroutine(delayCoroutine);
            if (ShowAnimationCoroutine != null)
                StopCoroutine(ShowAnimationCoroutine);

            OnClosedSuccess?.Invoke(hasWaited);
            hasWaited = false;
            isPaused = false;
            base.Close();
        }

        public void TryToClose()
        {
            if (hasWaited)
            {
                Close();
                return;
            }

            isPaused = true;
            QuitModal.SetActive(true);
        }

        public void CloseChoice(bool quit)
        {
            QuitModal.SetActive(false);
            if (quit)
                Close();
            else
                isPaused = false;
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
            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                visualTransform.anchoredPosition = Vector2.Lerp(visualTransform.anchoredPosition, new Vector2(0, 0), elapsedTime / animationDuration);
                canvasGroup.alpha = JuicyUtility.Remap((elapsedTime / animationDuration), 0, 1, .7f, 1);
                yield return null;
            }
            visualTransform.anchoredPosition = Vector2.zero;
            canvasGroup.alpha = 1;
        }

        IEnumerator EDelay()
        {
            float elapsedTime = 0;
            hasWaited = false;
            TimeIndicator.enabled = true;
            while (elapsedTime < delay)
            {
                if (!isPaused)
                {
                    elapsedTime += Time.unscaledDeltaTime;
                    TimeIndicator.fillAmount = 1 - (elapsedTime / delay);
                }
                yield return null;
            }
            TimeIndicator.enabled = false;
            hasWaited = true;
        }
    }
}
