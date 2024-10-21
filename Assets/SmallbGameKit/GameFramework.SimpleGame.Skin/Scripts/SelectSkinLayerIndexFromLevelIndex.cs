using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniSkin;

namespace GameFramework.SimpleGame.Skin
{
	[ExecuteInEditMode()]
	[AddComponentMenu("GameFramework/SimpleGame/Skin/SelectSkinLayerIndexFromLevelIndex")]
	public class SelectSkinLayerIndexFromLevelIndex : GameBehaviour
	{
		public SkinLayer skinLayer;

		public int changePeriod_beforeLoop = 1;

		public int changePeriod_inLoop = 1;

		public int differentLoopCount = 0;

		public int shuffledIndexSeed = 0;

		[Header("Editor")]
		public bool linkToLevelManagerEditorParameters;

		[SerializeField]
		int currentLevelIndex = 0;

		[SerializeField]
		UniRandom.ShuffledIntMapping shuffledIndicesMapping;

		public int SkinCount
		{
			get
			{
				if(skinLayer == null)
					return 0;

				return skinLayer.SkinCount;
			}
		}

		int SelectedLevelIndex
		{
			set
			{
				currentLevelIndex = value;

				int selectedIndex = currentLevelIndex;

				if(changePeriod_beforeLoop != 0)
					selectedIndex = selectedIndex / changePeriod_beforeLoop;

                int skinCount = SkinCount;
                if (selectedIndex >= skinCount)
				{
					int localToLoopIndex = (currentLevelIndex - skinCount);

					if(changePeriod_inLoop != 0)
						localToLoopIndex = localToLoopIndex / changePeriod_inLoop;

					selectedIndex = shuffledIndicesMapping.GetMappedIndex(skinCount + localToLoopIndex,  skinCount);
				}

				skinLayer.SelectedSkinIndex = selectedIndex;
			}
		}

		protected override void OnLoadGame()
		{
			base.OnLoadGame();

			SelectedLevelIndex = LevelManager.LevelIndex_RawAndContinuous - 1;
		}

#if UNITY_EDITOR
		void LateUpdate()
		{
			if(Application.isPlaying)
				return;
			
			InitializedIndexMapping();		
			
			if(linkToLevelManagerEditorParameters)
			{
				if(LevelManager.Instance != null)
				{
					LevelManager.Editor_LaunchParameters launchParameters = LevelManager.Instance.editor_launchParameters;
					SelectedLevelIndex = launchParameters.forceLevelStart_Index - 1;
				}
			}
		}
#endif

		void InitializedIndexMapping()
		{
			List<int> indices = new List<int>();
			for(int i = 0; i < SkinCount; ++i)
			{
				indices.Add(i);
			}

			Random.InitState(shuffledIndexSeed);

			shuffledIndicesMapping = new UniRandom.ShuffledIntMapping(differentLoopCount, indices);

			Random.InitState(System.Environment.TickCount);
		}
	}
}