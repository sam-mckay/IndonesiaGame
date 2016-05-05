using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ColourChanger : MonoBehaviour 
{
	Color imageColor;
	float duration;
	float t = 0;
	bool hasPulsed;
	
	//publicly accessible variables
	public Light targetLight;
	public Color Stage1 = new Color(1.0f, 0.0f, 0.0f, 1.0f);
	public Color Stage2 = new Color(1.0f, 0.0f, 0.0f, 0.7f);

    [Range(0.0f,10.0f)]
    public float intesity1;
    [Range(0.0f, 10.0f)]
    public float intesity2;
    [Range(0.0f, 10.0f)]
    public float fadeDuration = 2.0f;
	
	
	
	// Use this for initialization
	void Awake () 
	{
		hasPulsed = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//check if image has pulsed 
		if(hasPulsed)
		{
			Pulse(targetLight, Stage1, Stage2, intesity1, intesity2, fadeDuration);
		}
		else
		{
			Pulse(targetLight, Stage2, Stage1, intesity2, intesity1, fadeDuration);	
		}
		
	}
	
	//function to steadily change from one colour to another over specified time
	void Pulse(Light light, Color startColor, Color endColor, float x, float y, float duration)
	{
		light.color = Color.Lerp(startColor, endColor, t);
        light.intensity = Mathf.Lerp(x, y, t);
        if (t<1)
		{
			// increase time factor (t) by time divided by specified duration
			t += Time.deltaTime/duration;
		}
		if(t>1)
		{
			t=0;
			hasPulsed=!hasPulsed;
		}
	}
	
	public void initialiseColourChanger(Light newLight, Color color1, Color color2, float fadeTime)
	{
		targetLight = newLight;
		Stage1 = color1;
		Stage2 = color2;
		fadeDuration = fadeTime;
	}
	
	
}
