using InfinityCode.OnlineMapsExamples;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Map.SmartPrediction.Component {
    public class SmartPredictionInput : MonoBehaviour {
        [SerializeField] private SmartSearchPrediction smartSearchPrediction;
        [SerializeField] private TMP_InputField input;
        void Start() {
            input.onValueChanged.AddListener(ValueChange);
        }

        void Update() {

        }

        private void ValueChange(string value) {
            smartSearchPrediction.StartSearch(value, OnPredictionComplete);
        }

        public void OnPredictionComplete(OnlineMapsGooglePlacesAutocompleteResult[] places) {
            foreach (OnlineMapsGooglePlacesAutocompleteResult result in places) {
                Debug.Log(result.description);
            }
        }
    }
}