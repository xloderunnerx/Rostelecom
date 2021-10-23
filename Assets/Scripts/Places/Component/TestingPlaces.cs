using Map.Places.Component;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPlaces : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //StartTest();
    }

    public void StartTest() {
        var places = new List<string>();
        places.Add("food");
        places.Add("health");
        places.Add("gym");
        GetComponent<Places>().StartSearch(55.094578f, 36.612107f, 5000, places, (r) => {

            List<OnlineMapsMarker> markers = new List<OnlineMapsMarker>();

            foreach (OnlineMapsGooglePlacesResult result in r) {
                // Log name and location of each result.
                Debug.Log(result.name);
                Debug.Log(result.location);

                // Create a marker at the location of the result.
                OnlineMapsMarker marker = OnlineMapsMarkerManager.CreateItem(result.location, result.name);
                markers.Add(marker);
            }

            // Get center point and best zoom for markers
            Vector2 center;
            int zoom;
            OnlineMapsUtils.GetCenterPointAndZoom(markers.ToArray(), out center, out zoom);

            // Set map position and zoom.
            OnlineMaps.instance.position = center;
            OnlineMaps.instance.zoom = zoom + 1;
        });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
