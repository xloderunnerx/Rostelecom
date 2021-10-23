using Mapbox.Unity.Map;
using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public Vector2d geoCoords;
    public AbstractMap map;
    public bool assigneGeoCoords;
    public bool assigned;

    private void Start() {
        map = GameObject.FindObjectOfType<AbstractMap>();
        transform.parent.parent = null;
    }
    private void Update() {
        StayOnMap();
        AssignGeoCoords();
    }

    public void StayOnMap() {
        if (!assigned)
            return;
        transform.position = map.GeoToWorldPosition(geoCoords);
    }

    public void AssignGeoCoords() {
        if (!assigneGeoCoords)
            return;
        assigneGeoCoords = false;
        geoCoords = map.WorldToGeoPosition(transform.position);
    }
}
