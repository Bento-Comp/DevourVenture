using UnityEngine;

namespace JuicyInternal
{
    public class JuicyEmulatedBanner : JuicyEmulatedElement
    {
        public override void Show()
        {
            visual.SetActive(true);
        }

        public override void Close()
        {
            visual.SetActive(false);
        }
    }
}
