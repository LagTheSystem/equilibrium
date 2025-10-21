using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public GameObject map;
    private bool activated = false;

    void OnTriggerEnter(Collider other)
    {
        if (!activated) {
            var pos = gameObject.transform.position + new Vector3(90.5f, 11, 0);
            Instantiate(map, pos, gameObject.transform.rotation);
            activated = true;
        }
    }
}
