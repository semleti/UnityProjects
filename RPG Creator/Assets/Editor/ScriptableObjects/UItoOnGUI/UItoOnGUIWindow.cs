using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class UItoOnGUIWindow : EditorWindow {

	static public UItoOnGUI inspector;
	public Vector2 offset;
	float rotation = 0f;
	float value = 0f;
	bool valueBool = false;
	string textFieldString = "";
	int valueInt = 0;
	float valueFloat = 0f;
	string passwordString = "";
	// Use this for initialization
	
	void OnGUI()
	{
		if(inspector)
		{
			Debug.Log (inspector.canvas.gameObject.name);
			RectTransform rectTransform = (RectTransform)inspector.canvas.GetComponent<RectTransform>();
			Debug.Log (rectTransform.position);
			childToGUI(inspector.canvas.transform, new Rect(0,0,942,559));
		}
		else
		{
			Debug.Log ("OnGUI");
		}
	}
	
	void childToGUI(Transform parent, Rect GUIrect)
	{
		foreach(Transform child in parent)//loop through all children to transform them to GUI
		{
			RectTransform rectTransform = child.GetComponent<RectTransform> ();//get the recttransform to get info about UI
			Rect childRect = getRect(child, rectTransform, GUIrect);//transform the child to GUI and get it's GUI position and size, 
			if(rectTransform)
			{
				Vector2 pivotPoint = getPivotPoint(childRect, rectTransform);
				//draw GUI elements
				rotateStart(rectTransform.localEulerAngles.z, pivotPoint);
				DrawElementToGUI (child, childRect);

				childToGUI(child,childRect);//transform the child's children to GUI

				rotateEnd(rectTransform.localEulerAngles.z, pivotPoint);
				//Debug.Log(child.name + " " + childRect);
			}
			else
			{
				childToGUI(child,childRect);//transform the child's children to GUI
			}
		}
	}
	
	//void elementToGUI(Transform child, Rect parent)
	Rect getRect(Transform child, RectTransform rectTransform, Rect parent)//"transforms" a UI element to GUI, returns GUI position and size
	{

		if (!rectTransform) {//if not present, is not a UI element and therefore return null
			Debug.Log ("no rectTransform " + child.name);
			return new Rect (parent.x + child.localPosition.x,//calculate x: parent x + relative position from child to parent
			                parent.y - child.localPosition.y,//calculate y: parent y - relative position from child to parent (invert because coordinate systems are inverted)
			                parent.width,//keep the parents width because the element has no width (the UI works like this)
			                parent.height);//keep the parents height because the element has no height (the UI works like this)
		}



		//GUI position and size of the child
		//Debug.Log (child.name + " width: " + rectTransform.rect.width.ToString () + " height: " + rectTransform.rect.height +" rect: "+rectTransform.rect +" offsetminx: "+ rectTransform.offsetMin.x.ToString()+" offsetmaxx: "+ rectTransform.offsetMax.x.ToString());
		return new Rect (parent.x + parent.width * rectTransform.anchorMin.x + rectTransform.offsetMin.x, //calculate x: x from parent + anchor position x + offset x
		                           	parent.y + parent.height * (1 - rectTransform.anchorMax.y) - rectTransform.offsetMax.y,//calculate y: y from parent + anchor position y (invert because coordinate system is reversed) + offset y (invert because coordinate system is reversed)
	                				parent.width * (rectTransform.anchorMax.x - rectTransform.anchorMin.x) + rectTransform.rect.width*0 - rectTransform.offsetMin.x + rectTransform.offsetMax.x,//calculate width: parent width * anchor x difference + offset x (invert because is inverted in system)
		                 			parent.height * (rectTransform.anchorMax.y - rectTransform.anchorMin.y) + rectTransform.rect.height*0 - rectTransform.offsetMin.y + rectTransform.offsetMax.y);//calculate height: parent height * anchor y difference + offset y (invert because is inverted in system)

	}

	Vector2 getPivotPoint(Rect rectangle, RectTransform rectTransform )//pivot point of the child around which the rotation should be aplied
	{
		return new Vector2 (rectangle.x + rectangle.width * rectTransform.pivot.x,//calculate x: x from element + pivot x
		                                 rectangle.y + rectangle.height * (1 - rectTransform.pivot.y));//calculate y: element y + pivot y (invert because reversed coordinate system)
	}

	void rotateStart(float rotation, Vector2 pivotPoint)
	{
		GUIUtility.RotateAroundPivot (-rotation, pivotPoint);//rotate the whole GUI matrix around pivot (invert because is inverted in system)
	}

	void rotateEnd(float rotation, Vector2 pivotPoint)
	{
		GUIUtility.RotateAroundPivot (rotation, pivotPoint);//rotate the whole GUI matrix around pivot (invert because is inverted in system)
	}

	Texture2D tintTexture(Texture2D texture, Color color)
	{
		/*int width = texture.width;
		int count = 0;
		foreach(Color pixel in texture.GetPixels())
		{
			texture.SetPixel(count%width, count/width, new Color(pixel.r * color.r, pixel.g * color.g, pixel.b * color.b, pixel.a * color.a));
			count++;
		}
		texture.Apply ();*/
		return texture;
	}

	void DrawElementToGUI(Transform child, Rect rectangle)
	{
		//check for different UI components (nb: only a single rendering UI component can be attached )
		Text text;
		Button button;
		Image image;
		Slider slider;
		Scrollbar scrollbar;
		Toggle toggle;
		InputField inputField;

		Texture2D textureNormal;
		Texture2D textureHighlighted;
		Texture2D textureFocused;

		GUIStyle style = new GUIStyle();
		GUIStyle style2 = new GUIStyle();

		if ((button = child.gameObject.GetComponent<Button> ()) != null) {
			image = child.gameObject.GetComponent<Image>();
			if(image == null)
			{
				textureNormal = new Texture2D(1,1);
				textureNormal.SetPixel(0,0,button.colors.normalColor);
				textureNormal.Apply();

				textureHighlighted = new Texture2D(1,1);
				textureHighlighted.SetPixel(0,0,button.colors.highlightedColor);
				textureHighlighted.Apply();

				textureFocused= new Texture2D(1,1);
				textureFocused.SetPixel(0,0,button.colors.pressedColor);
				textureFocused.Apply();
			}
			else
			{
				textureNormal = tintTexture ((Texture2D)image.mainTexture, button.colors.normalColor);
				textureHighlighted = tintTexture ((Texture2D)image.mainTexture, button.colors.highlightedColor);
				textureFocused = tintTexture ((Texture2D)image.mainTexture, button.colors.pressedColor);
			}


			style.normal.background = textureNormal;
			style.onNormal.background = textureNormal;

			style.hover.background = textureHighlighted;
			style.onHover.background = textureHighlighted;

			style.active.background = textureFocused;
			style.onActive.background = textureFocused;

			style.focused.background = textureFocused;
			style.onFocused.background = textureFocused;

			style.border = new RectOffset(10,10,10,10);
			//GUI.backgroundColor = button.colors.normalColor;
			//GUI.DrawTexture(rectangle,textureNormal);
			if(GUI.Button (rectangle, "", style))
				Debug.Log ("pressed");
		} else if ((text = child.gameObject.GetComponent<Text> ()) != null) {
			style.alignment = text.alignment;
			style.fontStyle = text.fontStyle;
			style.fontSize = text.fontSize;
			style.font = text.font;
			style.normal.textColor = text.color;
			GUI.Label (rectangle, text.text, style);
		} else if ((slider = child.gameObject.GetComponent<Slider> ()) != null) {
			textureNormal = new Texture2D (1, 1);
			textureNormal.SetPixel (0, 0, Color.yellow);
			textureNormal.Apply ();

			textureHighlighted = new Texture2D (1, 1);
			textureHighlighted.SetPixel (0, 0, Color.green);
			textureHighlighted.Apply ();

			style.normal.background = textureNormal;
			style2.normal.background = textureHighlighted;
			//GUI.Slider(rectangle, slider.value, 0.1f, slider.minValue, slider.maxValue, style, style2, true, 0);
			value = GUI.HorizontalSlider (rectangle, value, slider.minValue, slider.maxValue);
		}
		else if ((scrollbar = child.gameObject.GetComponent<Scrollbar> ()) != null) 
		{
			value = GUI.HorizontalScrollbar(rectangle, value, scrollbar.size, 0f, 1f);
		}
		else if ((toggle = child.gameObject.GetComponent<Toggle> ()) != null)
		{
			valueBool = GUI.Toggle(rectangle, valueBool, "");
		}
		else if ((inputField = child.gameObject.GetComponent<InputField> ()) != null)
		{
			int charachterLimit = inputField.characterLimit;
			if (charachterLimit == 0)
				charachterLimit = -1;
			if(inputField.contentType == InputField.ContentType.Alphanumeric)
			{
				if(inputField.multiLine == false)
					textFieldString = Regex.Replace(GUI.TextField(rectangle, textFieldString, charachterLimit),@"[^a-zA-Z0-9]", string.Empty) ;
				else
					textFieldString = Regex.Replace(GUI.TextArea(rectangle, textFieldString, charachterLimit) ,@"[^a-zA-Z0-9]", string.Empty);
			}
			else if(inputField.contentType == InputField.ContentType.Autocorrected)//for now does the same as standard singleline
			{
				textFieldString = GUI.TextField(rectangle, textFieldString, charachterLimit);
			}
			else if (inputField.contentType == InputField.ContentType.IntegerNumber)
			{
				valueInt = EditorGUI.IntField(rectangle, valueInt);
				if(valueInt > Math.Pow(10, inputField.characterLimit))
					valueInt /= 10;

			}
			else if (inputField.contentType == InputField.ContentType.DecimalNumber)
			{
				valueFloat = EditorGUI.FloatField(rectangle, valueFloat);
				string valueFloatString = valueFloat.ToString();
				if(valueFloatString.Length > inputField.characterLimit)
					valueFloat = float.Parse(valueFloatString.Substring(0, inputField.characterLimit));
			}
			else if (inputField.contentType == InputField.ContentType.Name)//for now does the same as standard singleline
			{
				textFieldString = GUI.TextField(rectangle, textFieldString, charachterLimit);
			}
			else if (inputField.contentType == InputField.ContentType.EmailAddress)//for now does the same as standard singleline
			{
				textFieldString = GUI.TextField(rectangle, textFieldString, charachterLimit);
			}
			else if (inputField.contentType == InputField.ContentType.Password)
			{
				passwordString = EditorGUI.PasswordField(rectangle, passwordString);
				if( passwordString.Length > inputField.characterLimit)
					passwordString = passwordString.Substring(0, inputField.characterLimit);
			}
			else if (inputField.contentType == InputField.ContentType.Pin)
			{
				passwordString = Regex.Replace(EditorGUI.PasswordField(rectangle, passwordString), @"[^0-9]", string.Empty);
				if( passwordString.Length > inputField.characterLimit)
					passwordString = passwordString.Substring(0, inputField.characterLimit);
				Debug.Log ("password: " + passwordString);
			}
			else if (inputField.contentType == InputField.ContentType.Standard)
			{
				if(inputField.multiLine == false)
					textFieldString = GUI.TextField(rectangle, textFieldString, charachterLimit);
				else
					textFieldString = GUI.TextArea(rectangle, textFieldString, charachterLimit);
			}
			else if (inputField.contentType == InputField.ContentType.Custom)//does the same as standard
			{
				if(inputField.multiLine == false)
					textFieldString = GUI.TextField(rectangle, textFieldString, charachterLimit);
				else
					textFieldString = GUI.TextArea(rectangle, textFieldString, charachterLimit);
				Debug.Log ("custom not supported yet, using standard as alternative");
			}
			 
			
		}
		/*else if ((image = child.gameObject.GetComponent<Image> ()) != null)
		{
			GUI.DrawTexture(rectangle, (Texture)image.mainTexture);
		}*/
	}
}
