using UnityEngine;
using System.Collections;

public class Animator_8DirectionalStandby : MonoBehaviour {
	public Sprite[] sprites;
	public int current_sprite;

	private SpriteRenderer sr;

	void Awake() {
		sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (current_sprite >= 7)
		{
			current_sprite = 0;
		} 

		sr.sprite = sprites[current_sprite];

	}
}
