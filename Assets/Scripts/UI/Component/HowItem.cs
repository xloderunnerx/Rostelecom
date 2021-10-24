using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Component {
    public class HowItem : MonoBehaviour {
        [SerializeField] private OnlineMapsGoogleDirections.Mode mode;
        [SerializeField] private JourneySettings journeySettings;

        public void SetJourneyMode() {
            journeySettings.mode = mode;
        }
    }
}