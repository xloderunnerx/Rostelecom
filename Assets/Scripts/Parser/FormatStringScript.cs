using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Security.Cryptography;
using TMPro;

public struct JourneyModel
{
    public string from;
    public string to;
    public MovementType movementType;
}

public enum MovementType { 
    driving,
    walk,
    air,
    bicycle,
    transit
}


public class FormatStringScript : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public JourneyModel model;
    private string mockData = "Поеду в Анапу на машине с детьми сегодня";

    // Start is called before the first frame update
    void Start()
    {
        FormatText();
    }

    private void FormatText()
    {
        if (mockData == "") {
            return;
        }

        var chars = mockData.Split(' ');

        for (var i = 1; i < chars.Length; i++)
        {
            if (i + 1 >= chars.Length) { break; }

            switch (chars[i])
            {
                case "в":
                    model.to = chars[i + 1].ToString();
                    break;
                case "на":
                    var transport = chars[i + 1].ToString();
                    CheckMovementType(transport);
                    break;
                default:
                    break;

            }

        }

        print(model.to + model.movementType);

    }

    private void CheckMovementType(string movement) { 
        switch (movement) {

            case "машине":
                model.movementType = MovementType.driving;
                break;
            case "самолете":
                model.movementType = MovementType.air;
                break;
            case "велосипеде":
                model.movementType = MovementType.bicycle;
                break;
            case "трамвае":
                model.movementType = MovementType.transit;
                break;
            case "поезде":
                model.movementType = MovementType.transit;
                break;
            case "пешком":
                model.movementType = MovementType.walk;
                break;
            default:
                break;

        }
    
    }
}