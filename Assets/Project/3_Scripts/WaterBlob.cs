using UnityEngine;

public class WaterBlob : MonoBehaviour
{
    [SerializeField] float volume = 1.0f;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Water Pool"))
		{
			collision.gameObject.GetComponent<WaterPool>().AddVolume(volume);
			Destroy(gameObject);
		}
	}
}
