using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.SimpleGame
{
	[DefaultExecutionOrder(-1)]
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/Editor_LevelManager")]
	public class Editor_LevelManager : MonoBehaviour
	{
		public LevelManager.Editor_LaunchParameters launchParameters = new LevelManager.Editor_LaunchParameters();

		public LevelManager LevelManager => LevelManager.Instance;

#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;

			// Link to simple game level manager editor parameters
			GameFramework.SimpleGame.LevelManager frameworkLevelManager = GameFramework.SimpleGame.LevelManager.Instance;
			if(frameworkLevelManager != null)
			{
				LevelManager.editor_launchParameters.CopyFrom(launchParameters);

				UnityEditor.EditorUtility.SetDirty(LevelManager);
			}
		}
#endif
	}
}