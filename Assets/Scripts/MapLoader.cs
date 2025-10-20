using UnityEngine;

public class MapLoader : MonoBehaviour
{

    public GameObject map;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        var pos = gameObject.transform.position + new Vector3(90.5f, 11, 0);
        Instantiate(map, pos, gameObject.transform.rotation);
    }
}
