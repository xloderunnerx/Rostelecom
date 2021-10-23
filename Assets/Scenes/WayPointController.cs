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

    [SerializeField]
    Transform[] _waypoints;
    private List<Vector3> _cachedWaypoints;

    [SerializeField]
    [Range(1, 10)]
    private float UpdateFrequency = 2;

    private Directions _directions;
    private int _counter;

    GameObject _directionsGO;
    private bool _recalculateNext;
    private List<OnMapWaypoint> generatedWayPoints;
    private GameObject kolbaska;
    private DirectionsResponse latest;
    private void Awake() {
        generatedWayPoints = new List<OnMapWaypoint>();
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
        if(latest != null) {

            HandleDirectionsResponse(latest);
        }
    }

    public void generateRoutes() {

        generatedWayPoints = new List<OnMapWaypoint>();
        var list = new List<Vector2d>();

        var firstValue = inputA.SelectedFeature.Geometry.Coordinates;
        var secondValue = inputB.SelectedFeature.Geometry.Coordinates;

        var wayPointFirst = new GameObject("Waypoint");
        var wayPointSecond = new GameObject("Waypoint");

        var onMapFirst = wayPointFirst.AddComponent<OnMapWaypoint>();
        var onMapSecond = wayPointSecond.AddComponent<OnMapWaypoint>();

        onMapFirst.feature = inputA.SelectedFeature;
        onMapSecond.feature = inputB.SelectedFeature;
        onMapFirst.map = _map;
        onMapSecond.map = _map;
        generatedWayPoints.Add(onMapFirst);
        generatedWayPoints.Add(onMapSecond);

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
        latest = response;
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

}

public class OnMapWaypoint : MonoBehaviour {
    public Feature feature;
    public AbstractMap map;

    private void Start() {
        Convert();
    }

    private void Update() {
        Convert();
    }
    public void Convert() {
        transform.position = map.GeoToWorldPosition(feature.Geometry.Coordinates);
    }
}