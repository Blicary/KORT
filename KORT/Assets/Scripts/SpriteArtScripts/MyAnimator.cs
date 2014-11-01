using UnityEngine;
using System.Collections;

public class MyAnimator : MonoBehaviour {
	public float frames_per_second = 2.0f;
	public Sprite[] sprites;
	public int current_sprite;
	
	private float counter = 0.0f;
	private SpriteRenderer sr;
	
	void Awake() {
		sr = GetComponent<SpriteRenderer>();
	}
	void Update() {
		counter += Time.deltaTime;
		current_sprite += Mathf.FloorToInt(counter * frames_per_second);
		current_sprite %= sprites.Length;
		counter = (counter * frames_per_second) % 1.0f / frames_per_second;
		sr.sprite = sprites[current_sprite];
	}
}