using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Directions.Component {
    public class Test : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            //StartTest();
        }

        void StartTest() {
            var waypoints = new List<Vector2>();
            waypoints.Add(new Vector2(55.736389f, 37.621875f));
            waypoints.Add(new Vector2(55.188834f, 37.055712f));
            waypoints.Add(new Vector2(55.105873f, 36.605876f));
            GetComponent<Directions>().StartSearchMultiCoords(waypoints, OnlineMapsGoogleDirections.Mode.walking, (r) => {

                foreach (OnlineMapsGoogleDirectionsResult.Leg leg in r.routes[0].legs) {
                    foreach (OnlineMapsGoogleDirectionsResult.Step step in leg.steps) {
                        Debug.Log(step.string_instructions);
                    }
                }

                OnlineMapsDrawingLine route = new OnlineMapsDrawingLine(r.routes[0].overview_polylineD, new Color(1, 0.6570523f, 0), 5);

                OnlineMapsDrawingElementManager.AddItem(route);
            });
        }

        // Update is called once per frame
        void Update() {

        }
    }
}