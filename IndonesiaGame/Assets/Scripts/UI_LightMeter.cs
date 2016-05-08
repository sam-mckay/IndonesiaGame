using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UI_LightMeter : MonoBehaviour {

    public float radialValue = 0.0f;

    private Image sliderImage;

	// Use this for initialization
	void Start ()
    {
        sliderImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        sliderImage.fillAmount = radialValue;
    }
}
