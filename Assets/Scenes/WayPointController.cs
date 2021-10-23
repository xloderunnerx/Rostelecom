using Mapbox.Geocoding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map.Geocoder.Component;
using Mapbox.Directions;
using Mapbox.Unity.MeshGeneration.Modifiers;
using Mapbox.Unity.Map;
using Mapbox.Utils;
using Mapbox.Unity.Utilities;
using Mapbox.Unity.MeshGeneration.Data;
using System.Linq;
using Mapbox.Unity;
using Mapbox.Examples;
using Mapbox.Platform;

public class WayPointController : MonoBehaviour {

    public GeocoderInput inputA;
    public GeocoderInput inputB;

    [SerializeField]
    AbstractMap _map;

    [SerializeField]
    MeshModifier[] MeshModifiers;
    [SerializeField]
    Material _material;

    private Directions _directions;
    private int _counter;

    GameObject _directionsGO;
    private DirectionsResponse latestResponse;

    public List<Vector3> intermediateWayPoints;

    private void Awake() {
        intermediateWayPoints = new List<Vector3>();
        if (_map == null) {
            _map = FindObjectOfType<AbstractMap>();
        }

        _directions = MapboxAccess.Instance.Directions;
    }

    // Start is called before the first frame update
    void Start() {
        foreach (var modifier in MeshModifiers) {
            modifier.Initialize();
        }
    }

    // Update is called once per frame
    void Update() {
        if (latestResponse != null) {

            HandleDirectionsResponse(latestResponse);
        }
    }

    public void generateRoutes() {

        var list = new List<Vector2d>();

        var firstValue = inputA.SelectedFeature.Geometry.Coordinates;
        var secondValue = inputB.SelectedFeature.Geometry.Coordinates;

        list.Add(firstValue);
        list.Add(secondValue);

        Query(list);
    }

    void Query(List<Vector2d> vector2Ds) {
        var _directionResource = new DirectionResource(vector2Ds.ToArray(), RoutingProfile.Driving);
        _directionResource.Steps = true;
        _directions.Query(_directionResource, HandleDirectionsResponse);
    }


    void HandleDirectionsResponse(DirectionsResponse response) {
        if (response == null || null == response.Routes || response.Routes.Count < 1) {
            return;
        }
        latestResponse = response;
        GenerateIntermediateWayPoints();
        var meshData = new MeshData();
        var dat = new List<Vector3>();
        foreach (var point in response.Routes[0].Geometry) {
            dat.Add(_map.GeoToWorldPosition(point));
        }

        var feat = new VectorFeatureUnity();
        feat.Points.Add(dat);

        foreach (MeshModifier mod in MeshModifiers.Where(x => x.Active)) {
            mod.Run(feat, meshData, _map.WorldRelativeScale);
        }

        CreateGameObject(meshData);
    }

    GameObject CreateGameObject(MeshData data) {
        if (_directionsGO != null) {
            _directionsGO.Destroy();
        }
        _directionsGO = new GameObject("direction waypoint " + " entity");
        var mesh = _directionsGO.AddComponent<MeshFilter>().mesh;
        mesh.subMeshCount = data.Triangles.Count;

        mesh.SetVertices(data.Vertices);
        _counter = data.Triangles.Count;
        for (int i = 0; i < _counter; i++) {
            var triangle = data.Triangles[i];
            mesh.SetTriangles(triangle, i);
        }

        _counter = data.UV.Count;
        for (int i = 0; i < _counter; i++) {
            var uv = data.UV[i];
            mesh.SetUVs(i, uv);
        }

        mesh.RecalculateNormals();
        _directionsGO.AddComponent<MeshRenderer>().material = _material;
        return _directionsGO;
    }

    public void GenerateIntermediateWayPoints() {
        if (intermediateWayPoints.Count != 0)
            return;
        latestResponse.Waypoints.ForEach(w => intermediateWayPoints.Add(_map.GeoToWorldPosition(w.Location)));
        latestResponse.Routes.ForEach(r => {
            Debug.Log("Distance = " + r.Distance);
            r.Geometry.ForEach(v => {
                var wayPointGameObject = new GameObject("waypoint");
                wayPointGameObject.transform.position = _map.GeoToWorldPosition(v);
            });
            
        });

    }

}
