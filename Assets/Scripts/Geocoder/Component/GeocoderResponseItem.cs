using Mapbox.Geocoding;
using Mapbox.Unity.Map;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Map.Geocoder.Component {
    public class GeocoderResponseItem : MonoBehaviour {
        [SerializeField] private GeocoderInput geocoderInput;
        [SerializeField] private TextMeshProUGUI geocodeTMP;
        private Feature feature;
        public void Init(Feature feature, GeocoderInput geocoderInput) {
            this.geocoderInput = geocoderInput;
            this.feature = feature;
            geocodeTMP.text = feature.PlaceName + " " + feature.Address;
        }

        public void SelectFeature() {
            geocoderInput.SelectFeature(feature);
        }
    }
}