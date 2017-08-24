using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
	public RawImage Source;
	public RawImage Target;


	public bool requestIsFrontFacing;
	public bool isVerticalMirror;
	private int angle;

	void Start ()
	{
		
	}


	int ConvertCode ()
	{
		angle = 360 - (int)Source.transform.localEulerAngles.z;
		angle = angle % 360;
		int flipCode = 0;
		if (isVerticalMirror) {
			if (requestIsFrontFacing) {

				if (angle == 0) {
					flipCode = 5;
				} else if (angle == 90) {
					flipCode = 6;
				}
				if (angle == 180) {
					flipCode = 7;
				} else if (angle == 270) {
					flipCode = 8;
				}
			} else {
				if (angle == 0) {
					flipCode = -5;
				} else if (angle == 90) {
					flipCode = -6;
				}
				if (angle == 180) {
					flipCode = -7;
				} else if (angle == 270) {
					flipCode = -8;
				}
			}
		} else {
			if (requestIsFrontFacing) {

				if (angle == 0) {
					flipCode = 1;
				} else if (angle == 90) {
					flipCode = 2;
				}
				if (angle == 180) {
					flipCode = 3;
				} else if (angle == 270) {
					flipCode = 4;
				}
			} else {
				if (angle == 0) {
					flipCode = -1;
				} else if (angle == 90) {
					flipCode = -2;
				}
				if (angle == 180) {
					flipCode = -3;
				} else if (angle == 270) {
					flipCode = -4;
				}
			}
		}



		Debug.Log ("FLIPCODE:  " + flipCode + "    ");

		return flipCode;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnGUI ()
	{
		if (GUI.Button (new Rect (10, 10, 100, 50), "Click")) {
			var rect = RectTransformToScreenSpace (Source.rectTransform, Camera.main);
			Debug.Log (rect);


			StartCoroutine (Capture (rect));
			Target.material.SetInt ("_FlipStyle", ConvertCode ());
		}
	}


	IEnumerator Capture (Rect rect)
	{
		 
		yield return new WaitForEndOfFrame ();
		yield return new WaitForEndOfFrame ();

		//有些Android设备截图是黑色，需要设置QualitySettings.antiAliasing = 0;
		Texture2D tex = new Texture2D ((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
		tex.ReadPixels (rect, 0, 0, false);
		tex.Apply ();

		Target.texture = tex;
		 

	}


	public static Rect RectTransformToScreenSpace (RectTransform transform, Camera camera)
	{
		var worldCorners = new Vector3[4];
		transform.GetWorldCorners (worldCorners);

		var screenPositions = new Vector3[4];

		Vector2 min = new Vector2 (Screen.width, Screen.height);
		Vector2 max = Vector2.zero;
		for (int i = 0; i < worldCorners.Length; i++) {
			var p = camera.WorldToScreenPoint (worldCorners [i]);

			if (p.x < min.x) {
				min.x = p.x;
			}

			if (p.y < min.y) {
				min.y = p.y;
			}

			if (p.x > max.x) {
				max.x = p.x;
			}

			if (p.y > max.y) {
				max.y = p.y;
			}

		}


		min.x = Mathf.Max (min.x, 0);
		min.y = Mathf.Max (min.y, 0);
		max.x = Mathf.Min (max.x, Screen.width);
		max.y = Mathf.Min (max.y, Screen.height);

		return new Rect (min.x, min.y, (max - min).x, (max - min).y);

	}

}
