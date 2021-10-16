using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class UITexts : PublicVars
    {
        public Text CDs;
        public Text RemainingFT;

        void Update()
        {
            CDs.text = "Blink Times:"+ PlayerBlinkTimes + "\nFloat is ready: "+ PlayerFloatIsReady;
            RemainingFT.text = "~Floating~\n" + Mathf.Ceil(PlayerFloatTime * 10) / 10;
        }
    }
}
