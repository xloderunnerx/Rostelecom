using InfinityCode.OnlineMapsExamples;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Map.SmartPrediction.Component {
    public class SmartPredictionInput : MonoBehaviour {
        [SerializeField] private GameObject smartSearchItemPrefab;
        [SerializeField] private SmartSearchPrediction smartSearchPrediction;
        [SerializeField] private TMP_InputField input;
        private List<SmartSearchPredictionItem> smartSearchPredictionItems;
        private bool selected;
        private RectTransform rectTransform;
        public OnlineMapsGooglePlacesAutocompleteResult selectedOnlineMapsGooglePlacesAutocompleteResult;
        public bool from;
        [SerializeField] private JourneySettings journeySettings;

        private void Awake() {
            rectTransform = GetComponent<RectTransform>();
            smartSearchPredictionItems = new List<SmartSearchPredictionItem>();
            input = GetComponent<TMP_InputField>();
            input.onValueChanged.AddListener(ValueChange);
        }

        void Start() {
            
        }

        void Update() {

        }

        private void ValueChange(string value) {
            if(selected) {
                selected = false;
                return;
            }
            smartSearchPrediction.StartSearch(value, OnPredictionComplete);
        }

        public void OnPredictionComplete(OnlineMapsGooglePlacesAutocompleteResult[] places) {
            GenerateGeocoderResponsItems(places);
            foreach (OnlineMapsGooglePlacesAutocompleteResult result in places) {
                //Debug.Log(result.description);
            }
        }

        private void GenerateGeocoderResponsItems(OnlineMapsGooglePlacesAutocompleteResult[] places) {
            ClearAndDestroy();
            for (int i = 0; i < places.ToList().Count; i++) {
                var smartSearchItemGameObject = Instantiate(smartSearchItemPrefab);
                var smartSearchItem = smartSearchItemGameObject.GetComponent<SmartSearchPredictionItem>();
                var smartSearchItemRectTransform = smartSearchItemGameObject.GetComponent<RectTransform>();

                smartSearchItemRectTransform.position = new Vector3(rectTransform.anchoredPosition.x, (rectTransform.transform.position.y - rectTransform.sizeDelta.y * 1.5f) - smartSearchItemRectTransform.sizeDelta.y * i);
                smartSearchItemGameObject.transform.SetParent(transform, false);
                smartSearchItem.Init(places[i], this);
                smartSearchPredictionItems.Add(smartSearchItem);
            }
        }


        public void SelectPlace(OnlineMapsGooglePlacesAutocompleteResult place) {
            if (from)
                journeySettings.startPlace = place;
            else journeySettings.endPlace = place;
            selected = true;
            input.text = place.description;
            ClearAndDestroy();
            selectedOnlineMapsGooglePlacesAutocompleteResult = place;
        }

        public void ClearAndDestroy() {
            smartSearchPredictionItems.ForEach(gri => Destroy(gri.gameObject));
            smartSearchPredictionItems.Clear();
        }
    }
}