using Mapbox.Geocoding;
using Mapbox.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Map.Geocoder.Component {
    public class GeocoderInput : MonoBehaviour {
        [SerializeField] private GameObject geocoderResponseItemPrefab;
        [SerializeField] private TMP_InputField placeName;
        private List<Feature> features;
        private ForwardGeocodeResource _resource;
        private List<GeocoderResponseItem> geocoderResponseItems;
        private RectTransform rectTransform;
        [SerializeField] private Feature selectedFeature;
        private bool selected;

        public Feature SelectedFeature { get => selectedFeature; set => selectedFeature = value; }

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            geocoderResponseItems = new List<GeocoderResponseItem>();
            placeName = GetComponent<TMP_InputField>();
            _resource = new ForwardGeocodeResource("");
            placeName.onValueChanged.AddListener(ValueChanged);
        }

        void Start() {

        }


        void Update() {

        }

        private void ValueChanged(string value) {
            if(selected) {
                selected = false;
                return;
            }
            features = new List<Feature>();
            if (!string.IsNullOrEmpty(value)) {
                _resource.Query = value;
                MapboxAccess.Instance.Geocoder.Geocode(_resource, HandleGeocoderResponse);
            }
        }

        private void HandleGeocoderResponse(ForwardGeocodeResponse res) {
            if (res != null) {
                if (res.Features != null) {
                    features = res.Features;
                    GenerateGeocoderResponsItems(features);
                }
            }
        }

        private void GenerateGeocoderResponsItems(List<Feature> features) {
            ClearAndDestroy();
            for (int i = 0; i < features.Count; i++) {
                var geocoderResponseItemGameObject = Instantiate(geocoderResponseItemPrefab);
                var geocoderResponseItem = geocoderResponseItemGameObject.GetComponent<GeocoderResponseItem>();
                var geocoderResponseItemRectTransform = geocoderResponseItemGameObject.GetComponent<RectTransform>();
                
                geocoderResponseItemRectTransform.position = new Vector3(transform.position.x, (rectTransform.transform.position.y - rectTransform.sizeDelta.y) - geocoderResponseItemRectTransform.sizeDelta.y * i);
                geocoderResponseItemGameObject.transform.SetParent(transform, false);
                geocoderResponseItem.Init(features[i], this);
                geocoderResponseItems.Add(geocoderResponseItem);
            }
        }

        public void SelectFeature(Feature feature) {
            selected = true;
            placeName.text = feature.PlaceName + " " + feature.Address;
            ClearAndDestroy();
            SelectedFeature = feature;
        }

        public void ClearAndDestroy() {
            geocoderResponseItems.ForEach(gri => Destroy(gri.gameObject));
            geocoderResponseItems.Clear();
        }
    }
}