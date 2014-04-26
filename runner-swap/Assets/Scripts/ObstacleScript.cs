using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {

	public enum Type {WALL, WEAKED_WALL, SPIDER_NET, POISON}
	public Type type;
	
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
			case Type.POISON:
				PoisonCollision(collision);
				break;
		}
	}

	void WallCollision(Collision collision) {
		if (collisionTag == "Hero" || collisionTag == "Bullet") {
			DestroyObject(collision.gameObject);
		}
	}

	void WeakedWallCollision(Collision collision) {
		if (collisionTag == "Hero") {
			//slow down hero
		}
		else if (collisionTag == "Bullet") {
			DestroyObject (collision.gameObject);
		}

		DestroyObject (this.gameObject);
	}

	void PoisonCollision(Collision collision) {
		if (collisionTag == "Hero") {
			DestroyObject(collision.gameObject);
		}
	}
}
