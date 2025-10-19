using UnityEngine;

public class KillZone : MonoBehaviour {
	// fine for a prototype
	void OnTriggerEnter2D(Collider2D collision) {
		Destroy(collision.gameObject);
	}
}
