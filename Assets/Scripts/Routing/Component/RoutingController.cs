using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map.Routing.Component {
    public class RoutingController : MonoBehaviour {
        [SerializeField] private JourneySettings journeySettings;
        [SerializeField] private Directions.Component.Directions directions;
        [SerializeField] private Places.Component.Places places;
        [SerializeField] private SmartPrediction.Component.SmartSearchPrediction smartSearchPrediction;
        [SerializeField] private Places.Component.PlaceDetails placeDetails;

        private void Start() {
            InitDetails();
        }

        public void InitDetails() {
            if (journeySettings.startPlace == null)
                return;
            if (journeySettings.endPlace == null)
                return;

            OnlineMapsGooglePlaceDetailsResult locationStartDetails;
            OnlineMapsGooglePlaceDetailsResult locationEndDetails;

            placeDetails.StartSearch(journeySettings.startPlace.place_id, (startPlaceDetails) => {
                locationStartDetails = startPlaceDetails;
                placeDetails.StartSearch(journeySettings.endPlace.place_id, (endPlaceDetails) => {
                    locationEndDetails = endPlaceDetails;
                    directions.StartSearchCoords(startPlaceDetails.location, endPlaceDetails.location, journeySettings.mode, (d) => {
                        foreach (OnlineMapsGoogleDirectionsResult.Leg leg in d.routes[0].legs) {
                            foreach (OnlineMapsGoogleDirectionsResult.Step step in leg.steps) {
                                Debug.Log(step.string_instructions);
                            }
                        }

                        // Create a line, on the basis of points of the route.
                        OnlineMapsDrawingLine route = new OnlineMapsDrawingLine(d.routes[0].overview_polylineD, new Color(1, 0.6570523f, 0), 5);

                        // Draw the line route on the map.
                        OnlineMapsDrawingElementManager.AddItem(route);
                    });
                });
            });
        }
    }
}