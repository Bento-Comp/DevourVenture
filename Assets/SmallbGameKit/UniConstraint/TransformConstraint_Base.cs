using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniConstraint
{
	[AddComponentMenu("UniConstraint/TransformConstraint_Base")]
	public abstract class TransformConstraint_Base : MonoBehaviour
	{
		[SerializeField]
		Transform controlledTransform;

		[SerializeField]
		bool useMultipleTransforms;

		[SerializeField]
		List<Transform> controlledTransforms = new List<Transform>();

		[Header("Editor")]
		public bool editor_updateInEditMode = true;

		public bool ValidControlledTransform =>
			useMultipleTransforms ? true : controlledTransform != null;

		void OnEnable()
		{
#if UNITY_EDITOR
			if(Application.isPlaying == false)
			{
				if(ValidControlledTransform == false)
					return;
			}
#endif
			UpdateConstraint();
		}

		protected abstract void UpdateConstraint(Transform transformToUpdate);

		protected void UpdateConstraint()
		{
			if(useMultipleTransforms)
			{
				int count = controlledTransforms.Count;
				for(int i = 0; i < count; ++i)
				{
					Transform transformToUpdate = controlledTransforms[i];

#if UNITY_EDITOR
					if(Application.isPlaying == false && transformToUpdate == null)
							continue;
#endif

					UpdateConstraint(transformToUpdate);
				}
			}
			else
			{
				UpdateConstraint(controlledTransform);
			}
		}

		#if UNITY_EDITOR
		protected virtual void Editor_Update()
		{
			if(Application.isPlaying)
				return;

			if(editor_updateInEditMode == false)
				return;

			if(ValidControlledTransform == false)
				return;

			UpdateConstraint();
		}
		#endif
	}
}
