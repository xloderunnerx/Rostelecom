using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map.Routing.Component {
    public class RoutingController : MonoBehaviour {
        [SerializeField] private JourneySettings journeySettings;
        [SerializeField] private Directions.Component.Directions directions;
        [SerializeField] private Places.Component.Places places;
        [SerializeField] private SmartPrediction.Component.SmartSearchPrediction smartSearchPrediction;
        [SerializeField] private Places.Component.PlaceDetails placeDetails;
        [SerializeField] private List<Route> routs;
        private void Start() {
            routs = new List<Route>();
            Init();
        }

        public void Init() {
            if (journeySettings.startPlace == null)
                return;
            if (journeySettings.endPlace == null)
                return;

            var localRoute = new Route();

            placeDetails.StartSearch(journeySettings.startPlace.place_id, (startPlaceDetails) => {
                localRoute.locationStartDetails = startPlaceDetails;
                placeDetails.StartSearch(journeySettings.endPlace.place_id, (endPlaceDetails) => {
                    localRoute.locationEndDetails = endPlaceDetails;
                    directions.StartSearchCoords(startPlaceDetails.location, endPlaceDetails.location, journeySettings.mode, (d) => {
                        OnlineMapsMarkerManager.CreateItem(localRoute.locationEndDetails.location, localRoute.locationEndDetails.name);
                        OnlineMapsMarkerManager.CreateItem(localRoute.locationStartDetails.location, localRoute.locationEndDetails.name);
                        localRoute.onlineMapsGoogleDirectionsResult = d;
                        OnlineMapsDrawingLine route = new OnlineMapsDrawingLine(d.routes[0].overview_polylineD, new Color(0.4745098f, 0.4941176f, 0.5450981f), 5);
                        localRoute.onlineMapsDrawingLine = route;
                        OnlineMapsDrawingElementManager.AddItem(route);
                        GenerateVariousDirections(localRoute);
                    });
                });
            });
        }

        public void GenerateVariousDirections(Route originalRoute) {

            for (int i = 0; i < 5; i++) {

                var localRoute = new Route();
                var randomRoute = new List<Vector2>();
                var randomPoint = originalRoute.onlineMapsGoogleDirectionsResult.routes.FirstOrDefault().overview_polyline[originalRoute.onlineMapsGoogleDirectionsResult.routes.FirstOrDefault().overview_polyline.Length / 2];
                Debug.Log("rnd = " + randomPoint);
                places.StartSearch(randomPoint.x, randomPoint.y, 100000, journeySettings.pointsOfInterest, (pr) => {
                    if (pr == null)
                        return;
                    var rndPR = pr[UnityEngine.Random.Range(0, pr.Length)];
                    OnlineMapsMarker marker = OnlineMapsMarkerManager.CreateItem(new Vector2(rndPR.location.y, rndPR.location.x), rndPR.name);
                    randomRoute.Add(new Vector2(originalRoute.locationStartDetails.location.y, originalRoute.locationStartDetails.location.x));
                    randomRoute.Add(rndPR.location);
                    randomRoute.Add(new Vector2(originalRoute.locationEndDetails.location.y, originalRoute.locationEndDetails.location.x));
                    directions.StartSearchMultiCoords(randomRoute, journeySettings.mode, (d) => {
                        localRoute.locationStartDetails = originalRoute.locationStartDetails;
                        localRoute.locationEndDetails = originalRoute.locationEndDetails;
                        localRoute.onlineMapsGoogleDirectionsResult = d;
                        OnlineMapsDrawingLine route;

                        route = new OnlineMapsDrawingLine(d.routes[0].overview_polylineD, new Color(1, 0.6570523f, 0), 5);
                        //else route = new OnlineMapsDrawingLine(d.routes[0].overview_polylineD, new Color(0.4745098f, 0.4941176f, 0.5450981f), 5);
                        localRoute.onlineMapsDrawingLine = route;
                        OnlineMapsDrawingElementManager.AddItem(route);
                        int distance = d.routes.FirstOrDefault().legs.Sum(l => l.distance.value);
                        int duration = d.routes.FirstOrDefault().legs.Sum(l => l.duration.value);
                        localRoute.distance = distance;
                        localRoute.duration = duration;
                        routs.Add(localRoute);
                    });

                });

            }
        }
    }

    [Serializable]
    public class Route {
        public OnlineMapsDrawingLine onlineMapsDrawingLine;
        public OnlineMapsGooglePlaceDetailsResult locationStartDetails;
        public OnlineMapsGooglePlaceDetailsResult locationEndDetails;
        public OnlineMapsGoogleDirectionsResult onlineMapsGoogleDirectionsResult;
        public int distance;
        public int duration;
    }
}