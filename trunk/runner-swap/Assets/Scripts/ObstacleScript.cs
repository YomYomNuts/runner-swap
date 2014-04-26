using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {

	public enum Type {WALL, WEAKED_WALL, SLOW_WALL}
	public Type type;
	public float slowRate;
	
	private string collisionTag;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision) {	
		collisionTag = collision.gameObject.tag;
		switch (type) {
			case Type.WALL:
				WallCollision(collision);
				break;
			case Type.WEAKED_WALL:
				WeakedWallCollision(collision);
				break;
		}
	}

	void OnTriggerEnter(Collider collider) {	
		if (type == Type.SLOW_WALL) {
			collisionTag = collider.gameObject.tag;
			if(collisionTag == "Hero") {		
				SlowWallTrigger(collider);
			}
		}
	}

	void OnTriggerExit(Collider collider) {
		if (type == Type.SLOW_WALL) {
			if(collider.gameObject.tag == "Hero") {			
				collider.gameObject.GetComponent<CharacterManagerScript>().ResetMaxVelocity();
			}
		}
	}

	void WallCollision(Collision collision) {
		if (collisionTag == "Hero" || collisionTag == "Bullet") {
			DestroyObject(collision.gameObject);
		}
	}

	void SlowWallTrigger(Collider collider) {
		if (collisionTag == "Hero") {
			collider.gameObject.GetComponent<CharacterManagerScript>().ChangeMaxVelocity(slowRate);
		}
	}

	void WeakedWallCollision(Collision collision) {
		if (collisionTag == "Hero") {
			//slow down hero
			collision.gameObject.GetComponent<CharacterManagerScript>().ChangeVelocity(-slowRate);
			DestroyObject (this.gameObject);
		}
		else if (collisionTag == "Bullet") {
			DestroyObject (collision.gameObject);			
			DestroyObject (this.gameObject);
		}
	}
}
