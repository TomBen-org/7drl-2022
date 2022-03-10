using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorChange : MonoBehaviour
{
    public Color onColor = Color.white;
    private Color _originalColor;
    
    void Awake() {
        _originalColor = GetComponent<Image>().color;
    }
    
    public void ChangeColor(bool state) {
        GetComponent<Image>().color = state? onColor: _originalColor;
    }
}
