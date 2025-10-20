using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public GameObject map;

    void OnTriggerEnter(Collider other)
    {
        var pos = gameObject.transform.position + new Vector3(90.5f, 11, 0);
        Instantiate(map, pos, gameObject.transform.rotation);
    }
}
