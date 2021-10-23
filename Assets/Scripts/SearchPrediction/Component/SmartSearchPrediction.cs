using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.SmartPrediction.Component {
    public class SmartSearchPrediction : MonoBehaviour {
        [SerializeField] private string apiKey;
        
        public void StartSearch(string value, Action<OnlineMapsGooglePlacesAutocompleteResult[]> onComplete) {
            OnlineMapsGooglePlacesAutocomplete.Find(
                value,
                apiKey
                ).OnComplete += (s) => {
                    onComplete?.Invoke(OnFindComplete(s));
                };
        }
        private OnlineMapsGooglePlacesAutocompleteResult[] OnFindComplete(string value) {
            OnlineMapsGooglePlacesAutocompleteResult[] results = OnlineMapsGooglePlacesAutocomplete.GetResults(value);

            // If there is no result
            if (results == null) {
                Debug.Log("Error");
                Debug.Log(value);
                return null;
            }

            // Log description of each result.
            foreach (OnlineMapsGooglePlacesAutocompleteResult result in results) {
                Debug.Log(result.description);
            }
            return results;
        }
    }
}