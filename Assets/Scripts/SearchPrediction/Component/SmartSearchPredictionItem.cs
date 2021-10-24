using Map.SmartPrediction.Component;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SmartSearchPredictionItem : MonoBehaviour
{
    [SerializeField] private SmartPredictionInput smartPredictionInput;
    [SerializeField] private OnlineMapsGooglePlacesAutocompleteResult onlineMapsGooglePlacesAutocompleteResult;
    [SerializeField] private TextMeshProUGUI outputTMP;
    public void Init(OnlineMapsGooglePlacesAutocompleteResult onlineMapsGooglePlacesAutocompleteResult, SmartPredictionInput smartPredictionInput) {
        this.smartPredictionInput = smartPredictionInput;
        this.onlineMapsGooglePlacesAutocompleteResult = onlineMapsGooglePlacesAutocompleteResult;
        outputTMP.text = onlineMapsGooglePlacesAutocompleteResult.description;
    }

    public void Select() {
        smartPredictionInput.SelectPlace(onlineMapsGooglePlacesAutocompleteResult);
    }
}
