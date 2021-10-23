using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JourneySettings", menuName = "SO/Jouney/JourneySettings")]
public class JourneySettings : ScriptableObject
{
    public string startName;
    public string endName;
    public OnlineMapsGoogleDirections.Mode mode;
    public List<string> pointsOfInterest;
}
