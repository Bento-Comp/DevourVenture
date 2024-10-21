using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(-1)]
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/Setup_LevelManager")]
	public class Setup_LevelManager : MonoBehaviour
	{
		public bool overrideLeveling = true;
		public bool leveling = true;

		public LevelManager.LevelIndicesMapping levelIndicesMapping = new LevelManager.LevelIndicesMapping();

		public LevelManager LevelManager => LevelManager.Instance;

#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			levelIndicesMapping.Initialize();

			// Link to simple game level manager editor parameters
			LevelManager frameworkLevelManager = LevelManager.Instance;
			if(frameworkLevelManager != null)
			{
				if(overrideLeveling)
					LevelManager.leveling = leveling;

				LevelManager.levelIndicesMapping.CopyFrom(levelIndicesMapping);

				UnityEditor.EditorUtility.SetDirty(LevelManager);
			}
		}
#endif
	}
}