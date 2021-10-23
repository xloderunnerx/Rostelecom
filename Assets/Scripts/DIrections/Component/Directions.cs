using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map.Directions.Component {
    public class Directions : MonoBehaviour {
        [SerializeField] private string apiKey;

        public void StartSearchCoords(Vector2 start, Vector2 end, Action<OnlineMapsGoogleDirectionsResult> onComplete) {
            if (string.IsNullOrEmpty(apiKey)) Debug.LogWarning("Please specify Google API Key");
            OnlineMapsGoogleDirections request = new OnlineMapsGoogleDirections
            (
                apiKey,
                start,
                end
            );
            request.OnComplete += (s) => {
                onComplete?.Invoke(OnFind(s));
            };
            request.Send();
        }

        public void StartSearchNames(string nameStart, string nameEnd, Action<OnlineMapsGoogleDirectionsResult> onComplete) {
            if (string.IsNullOrEmpty(apiKey)) Debug.LogWarning("Please specify Google API Key");
            OnlineMapsGoogleDirections request = new OnlineMapsGoogleDirections
            (
                apiKey,
                nameStart,
                nameEnd
            );
            request.OnComplete += (s) => {
                onComplete?.Invoke(OnFind(s));
            };
            request.Send();
        }

        public void StartSearchMultiCoords(List<Vector2> startWaypointsEnd, Action<OnlineMapsGoogleDirectionsResult> onComplete) {
            if (string.IsNullOrEmpty(apiKey)) Debug.LogWarning("Please specify Google API Key");
            for(int i = 0; i < startWaypointsEnd.Count; i++) {
                var oldX = startWaypointsEnd[i].x;
                var oldY = startWaypointsEnd[i].y;
                startWaypointsEnd[i] = new Vector2(oldY, oldX);
            }
            var waypoints = startWaypointsEnd.Where(w => w != startWaypointsEnd.FirstOrDefault()).Where(w => w != startWaypointsEnd.LastOrDefault()).ToList();
            OnlineMapsGoogleDirections request = new OnlineMapsGoogleDirections
            (
                apiKey,
                startWaypointsEnd.FirstOrDefault(),
                startWaypointsEnd.LastOrDefault(),
                waypoints
            );
            request.OnComplete += (s) => {
                onComplete?.Invoke(OnFind(s));
            };
            request.Send();
        }

        private OnlineMapsGoogleDirectionsResult OnFind(string value) {
            
            OnlineMapsGoogleDirectionsResult result = OnlineMapsGoogleDirections.GetResult(value);

            if (result == null || result.routes.Length == 0) {
                Debug.Log("Find direction failed");
                Debug.Log(value);
                return null;
            }

            return result;
        }
    }
}