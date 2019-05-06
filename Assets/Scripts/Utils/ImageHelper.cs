using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageHelper {

	public static void SetAlpha(this UnityEngine.UI.Image image,float alpha)
	{
		image.color = new Color (image.color.r, image.color.g, image.color.b, alpha);
	}
}
