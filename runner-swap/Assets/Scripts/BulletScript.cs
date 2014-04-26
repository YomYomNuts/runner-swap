using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	public enum Type {FIRE, FROST}
	public Type type;
	public int speed;
	public int maxDistance;

	private float traveledDistance;

	// Use this for initialization
	void Start () {
		traveledDistance = 0;
		this.rigidbody.AddForce (Vector3.right * speed);
	}
	
	// Update is called once per frame
	void Update () {
		traveledDistance += speed;
		if (traveledDistance >= maxDistance) {
			DestroyObject(this.gameObject);
		}
	}
}
