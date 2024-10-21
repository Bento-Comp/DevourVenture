using UnityEngine;

namespace JuicyInternal
{
    public class JuicyEmulatedElement : MonoBehaviour
    {
        [SerializeField] protected GameObject visual;
        [SerializeField] protected GameObject inputMask;
        protected RectTransform visualTransform;

        public delegate void EmulatedEvents();
        public EmulatedEvents OnOpened;
        public EmulatedEvents OnClosed;
        public EmulatedEvents OnLoaded;

        [HideInInspector] public bool IsReady = false;

        protected virtual void Awake()
        {
            visualTransform = visual.GetComponent<RectTransform>();
            visual.SetActive(false);
            if(inputMask != null)
                inputMask.SetActive(false);
        }

        public virtual void Load()
        {
            IsReady = true;
            OnLoaded?.Invoke();
        }

        public virtual void Show()
        {
            OnOpened?.Invoke();
            visual.SetActive(true);
            if (inputMask != null)
                inputMask.SetActive(true);
        }

        public virtual void Close()
        {
            OnClosed?.Invoke();
            IsReady = false;
            visual.SetActive(false);
            if (inputMask != null)
                inputMask.SetActive(false);

        }
    }
}

