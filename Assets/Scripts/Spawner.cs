using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public float minTime = 3f;
    public float maxTime = 4f;

    private void Start()
    {
        PlayAnimation();
    }

    private void PlayAnimation() {
        FindObjectOfType<DonkeyKong>().PlayThrow();

        Invoke(nameof(Spawn), 0.5f);
    }

    private void Spawn()
    {
        FindObjectOfType<DonkeyKong>().StopThrow();
        
        Instantiate(prefab, transform.position, Quaternion.identity);
        Invoke(nameof(PlayAnimation), Random.Range(minTime, maxTime));
    }

}
