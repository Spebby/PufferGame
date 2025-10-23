using UnityEngine;

public class MonsterSpawner : MonoBehaviour {
    [SerializeField] Monster prefab;
    [SerializeField] float radius = 150f;
    [SerializeField] float maxRandomOffset = 30f;
    [SerializeField] uint count = 8;
    
    void Start() {
        Vector3 center = GameObject.FindGameObjectWithTag("Player").transform.position;
        for (int i = 0; i < count; i++) {
            float   angle = i * Mathf.PI * 2f / count;
            Vector3 pos   = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (radius + maxRandomOffset * Random.value);
            Instantiate(prefab, center + pos, Quaternion.identity);
        }
    }
}
