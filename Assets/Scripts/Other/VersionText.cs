using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VersionText : MonoBehaviour
{
    TextMeshProUGUI text;
    
    private void Awake() {
        gameObject.name = DateTime.Now.ToString();
        text = GetComponent<TextMeshProUGUI>();
        text.text = $"v{Application.version}";
    }
}
