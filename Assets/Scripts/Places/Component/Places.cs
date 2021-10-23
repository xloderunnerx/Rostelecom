using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Places.Component {
    public class Places : MonoBehaviour {
        [SerializeField] private string apiKey;
        public void StartSearch(double longitude, double latitude, int radius, List<String> types, Action<OnlineMapsGooglePlacesResult[]> onComplete) {

            var allTypes = "";
            for(int i = 0; i < types.Count; i++) {
                if (i == 0) {
                    allTypes += types[i];
                    break;
                }
                allTypes += "|" + types[i];
            }

            OnlineMapsGooglePlaces.FindNearby(
                apiKey,
                new OnlineMapsGooglePlaces.NearbyParams(
                    latitude,
                    longitude,
                    radius)
                {
                    types = allTypes
                }).OnComplete += (s) => {
                    onComplete?.Invoke(OnFindComplete(s));
                };
        }

        public OnlineMapsGooglePlacesResult[] OnFindComplete(string value) {
            OnlineMapsGooglePlacesResult[] results = OnlineMapsGooglePlaces.GetResults(value);

            if (results == null) {
                Debug.Log("Error");
                Debug.Log(value);
                return null;
            }

            return results;
        }
    }
}