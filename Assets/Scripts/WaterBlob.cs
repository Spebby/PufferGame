using UnityEngine;

public class WaterBlob : MonoBehaviour
{
    [SerializeField] float volume = 1.0f;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Water Pool"))
		{
			collision.gameObject.GetComponentInParent<WaterPool>().AddVolume(volume);
			Destroy(gameObject);
		}
	}
}
