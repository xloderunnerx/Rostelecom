using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI.Component {
    public class CategoryItem : MonoBehaviour {
        public string category;
        public JourneySettings journeySettings;

        public void SetCategory() {
            journeySettings.pointsOfInterest.Add(category);
            journeySettings.pointsOfInterest = journeySettings.pointsOfInterest.Distinct().ToList();
        }
    }
}