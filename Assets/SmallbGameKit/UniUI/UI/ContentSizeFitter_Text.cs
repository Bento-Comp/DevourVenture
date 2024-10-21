using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniUI
{
    [AddComponentMenu("UniUI/ContentSizeFitter_Text")]
    [ExecuteAlways]
    [RequireComponent(typeof(Text))]
    public class ContentSizeFitter_Text : UIBehaviour, ILayoutSelfController
    {
		public RectTransform controlledRectTransform;

		public bool constrainsWidth;
		public bool constrainsHeight;

        [System.NonSerialized] private Text m_Text;
        private Text text
        {
			get
			{
				if (m_Text == null)
					m_Text = GetComponent<Text>();
				return m_Text;
			}
        }

        private DrivenRectTransformTracker m_Tracker;

        protected ContentSizeFitter_Text()
        {}

        protected override void OnEnable()
        {
			base.OnEnable();
			SetDirty();
        }

        protected override void OnDisable()
        {
			m_Tracker.Clear();

			if(controlledRectTransform == null)
				return;

			LayoutRebuilder.MarkLayoutForRebuild(controlledRectTransform);

			base.OnDisable();
        }

        protected override void OnRectTransformDimensionsChange()
        {
			SetDirty();
        }

        private void HandleSelfFittingAlongAxis(int axis)
        {
			if(controlledRectTransform == null)
				return;

			m_Tracker.Add(this, controlledRectTransform,
				(axis == 0 ? DrivenTransformProperties.SizeDeltaX : DrivenTransformProperties.SizeDeltaY));

			// Handle Text Best fit
			text.cachedTextGenerator.Invalidate();
			Vector2 rectSize = (text.transform as RectTransform).rect.size;
			TextGenerationSettings generationSettings = text.GetGenerationSettings(rectSize);
			text.cachedTextGenerator.Populate(text.text, generationSettings);

			float beforeBestFitFontSize = (float)text.fontSize * generationSettings.scaleFactor;
			float afterBestFitFontSize = (float)text.cachedTextGenerator.fontSizeUsedForBestFit;

			if(beforeBestFitFontSize <= 0)
				return;

			float bestFitFontScale = afterBestFitFontSize/beforeBestFitFontSize;

			float size;
			generationSettings.scaleFactor = 1.0f;
			if(axis==0)
            {
				size = text.cachedTextGenerator.GetPreferredWidth(text.text, generationSettings);//text.preferredWidth;
			}
			else
            {
				size = text.cachedTextGenerator.GetPreferredHeight(text.text, generationSettings);//text.preferredHeight;
            }

			size *= bestFitFontScale;

			controlledRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, size);
        }

        /// <summary>
        /// Calculate and apply the horizontal component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutHorizontal()
        {
            m_Tracker.Clear();

			if(constrainsWidth)
				HandleSelfFittingAlongAxis(0);
        }

        /// <summary>
        /// Calculate and apply the vertical component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutVertical()
        {
			if(constrainsHeight)
				HandleSelfFittingAlongAxis(1);
        }

        protected void SetDirty()
        {
            if (!IsActive())
                return;

			if(controlledRectTransform == null)
				return;

            LayoutRebuilder.MarkLayoutForRebuild(controlledRectTransform);
        }

		#if UNITY_EDITOR
        protected override void OnValidate()
        {
            SetDirty();
        }
		#endif
    }
}
