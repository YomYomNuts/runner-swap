using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public bool isQuitButton = false;
	private Color hoverColor = new Color (192/255f, 57/255f, 43/255f);

	void OnMouseEnter () {
		renderer.material.color = hoverColor;
	}

	void OnMouseExit () {
		renderer.material.color = Color.white;
	}

	void OnMouseDown () {
		if (isQuitButton) {
			Application.Quit();
		} else {
			Application.LoadLevel("Level01");
		}

	}
}
