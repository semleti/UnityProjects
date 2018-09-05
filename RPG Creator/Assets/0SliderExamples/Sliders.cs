using UnityEngine;
using System.Collections;

public class Sliders : MonoBehaviour
{
	public GUISkin skin;
	
	// NOTE: I used this Start method ONCE in the editor. Since the GUISkin is an asset
	// the changes persists after you stop the game. This code isn't required at all, but
	// it simplifies the creation of the styles. When you create a new style from scratch
	// you easily miss a certain setting (padding, overflow, border, alignment, image position, ...)
	// NOTE#2: I changed a lot settings afterwards to demonstrate some settings.
	// feel free to press play and adjust some settings at runtime to see their effect.
	/*
    void Start ()
    {
        skin.customStyles = new GUIStyle[]
		{
			new GUIStyle(skin.horizontalSlider)
			{
				name = "Slider1",
				fixedHeight = 16,
			},
			new GUIStyle(skin.horizontalSliderThumb)
			{
				name = "Slider1Thumb",
				fixedHeight = 16,
				fixedWidth  = 16,
			},

			new GUIStyle(skin.horizontalSlider)
			{
				name = "Slider2",
				fixedHeight = 32,
			},
			new GUIStyle(skin.horizontalSliderThumb)
			{
				name = "Slider2Thumb",
				fixedHeight = 32,
				fixedWidth  = 32,
			},

			new GUIStyle(skin.horizontalSlider)
			{
				name = "Slider3",
				fixedHeight = 8,
			},
			new GUIStyle(skin.horizontalSliderThumb)
			{
				name = "Slider3Thumb",
				fixedHeight = 32,
				fixedWidth  = 8,
			},
		};
    }
    */
    
	private float val1 = 0.0f;
	private float val2 = 0.0f;
	private float val3 = 0.0f;
	
    void OnGUI()
    {
        GUI.skin = skin;
        val1 = GUI.HorizontalSlider (new Rect(10, 10, 200, 16), val1, 0.0f, 10.0f, "Slider1", "Slider1Thumb");
        val2 = GUI.HorizontalSlider (new Rect(10, 30, 200, 32), val2, 0.0f, 10.0f, "Slider2", "Slider2Thumb");
        val3 = GUI.HorizontalSlider (new Rect(10, 70, 200, 32), val3, 0.0f, 10.0f, "Slider3", "Slider3Thumb");
    }
}
