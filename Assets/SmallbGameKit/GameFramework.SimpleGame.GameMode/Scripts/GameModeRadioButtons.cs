using UnityEngine;

using UniUI.UniActivation;

using System.Collections.Generic;

namespace GameFramework.SimpleGame.GameMode
{
	[AddComponentMenu("GameFramework/SimpleGame/GameMode/GameModeRadioButtons")]
	public class GameModeRadioButtons : MonoBehaviour
	{
		public List<GameModeRadioButton> buttons;

		public GameModeRadioButtons_Activations activations;
	}
}