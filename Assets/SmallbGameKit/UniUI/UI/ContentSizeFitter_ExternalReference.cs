using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UniUI
{
    [AddComponentMenu("UniUI/ContentSizeFitter_ExternalReference")]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public class ContentSizeFitter_ExternalReference : UIBehaviour, ILayoutSelfController
    {
		public RectTransform controlledRectTransform;

        [System.NonSerialized] private RectTransform m_Rect;
        private RectTransform rectTransform
        {
			get
			{
				if (m_Rect == null)
					m_Rect = GetComponent<RectTransform>();
				return m_Rect;
			}
        }

        private DrivenRectTransformTracker m_Tracker;

        protected ContentSizeFitter_ExternalReference()
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

			controlledRectTransform.SetSizeWithCurrentAnchors((RectTransform.Axis)axis, LayoutUtility.GetPreferredSize(rectTransform, axis));
        }

        /// <summary>
        /// Calculate and apply the horizontal component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutHorizontal()
        {
            m_Tracker.Clear();
            HandleSelfFittingAlongAxis(0);
        }

        /// <summary>
        /// Calculate and apply the vertical component of the size to the RectTransform
        /// </summary>
        public virtual void SetLayoutVertical()
        {
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


		void LateUpdate()
        {
			HandleSelfFittingAlongAxis(0);
            HandleSelfFittingAlongAxis(1);
		}
    }
}
