using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace JuicyInternal
{
    public enum CheatCodeInputType
    {
        None,
        Valid,
        InValid
    }

    public class JuicyCheatCode
    {
        public string Id;
        public int Progression;
        public int[] Sequence;
        public int TouchAmount;
        public System.Action Action;

        public JuicyCheatCode(string id, int touchAmount,System.Action action, params int[] sequence)
        {
            Id = id;
            TouchAmount = touchAmount;
            Action = action;
            Sequence = sequence;
        }
    }

    public class JuicyCheatCodeManager
    {
        public static void CheckCheatExecution(JuicyCheatCode cheat)
        {
            if (cheat == null)
                return;

            if (Input.touchCount < cheat.TouchAmount)
            {
                cheat.Progression = 0;
                return;
            }

            CheatCodeInputType input = IsCorrectInput(cheat.Sequence[cheat.Progression]);
            switch (input)
            {
                case CheatCodeInputType.None:
                    break;
                case CheatCodeInputType.Valid:
                    cheat.Progression++;
                    break;
                case CheatCodeInputType.InValid:
                    cheat.Progression = 0;
                    break;
            }

            if (cheat.Progression == cheat.Sequence.Length)
            {
                cheat.Action.Invoke();
                cheat.Progression = 0;
                JuicyAnalyticsManager.Instance.TrackAnalyticsEvent($"cheat_code_execution", new Juicy.EventProperty("ID", cheat.Id));
            }
        }

        static CheatCodeInputType IsCorrectInput(int target)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.touches[i].phase == TouchPhase.Ended)
                {
                    if (i == target)
                        return CheatCodeInputType.Valid;
                    else
                        return CheatCodeInputType.InValid;
                }
            }
            return CheatCodeInputType.None;
        }
    }

}
