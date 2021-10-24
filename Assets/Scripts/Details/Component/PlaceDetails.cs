using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Places.Component {
    public class PlaceDetails : MonoBehaviour {
        [SerializeField] private string apiKey;

        public void StartSearch(string placeID, Action<OnlineMapsGooglePlaceDetailsResult> onComplete) {

            OnlineMapsGooglePlaceDetails.FindByPlaceID(apiKey, placeID).OnComplete += (s) => {
                onComplete?.Invoke(OnFindComplete(s));
            };
        }

        private OnlineMapsGooglePlaceDetailsResult OnFindComplete(string value) {
            OnlineMapsGooglePlaceDetailsResult result = OnlineMapsGooglePlaceDetails.GetResult(value);

            // If there is no result
            if (result == null) {
                Debug.Log("Error");
                Debug.Log(value);
                return null;
            }

            return result;
        }
    }
}