using System.Collections.Generic;
using UnityEngine;

public class BumpLogic : MonoBehaviour
{

    public float bumpStrength = 10;
    public float minimumBump = .5f;
    public float maximumBump = 4f;
    public PlayerController player;
    private List<GameObject> objects = new List<GameObject>();
    private int counter = 0;

    void LateUpdate()
    {
        if (player.isAlive)
        {
            transform.localRotation = Quaternion.Euler(0, 90, 0);
            transform.localPosition = new Vector3(0, 0.09f, 0);
        }
    }

    void FixedUpdate()
    {
        counter++;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bomb") && player.isAlive && !objects.Contains(collision.gameObject))
        {
            objects.Add(collision.gameObject);
            //player.transform.Rotate(new Vector3(Mathf.Sign(collision.GetContact(0).point.z) * Mathf.Clamp(Mathf.Abs(collision.GetContact(0).point.z), minimumBump, maximumBump) * bumpStrength / Mathf.Clamp((-.75f * counter) + 10, 1, 10), 0, 0));
            player.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(bumpStrength, 0, 0) * Mathf.Sign(collision.GetContact(0).point.z) * Mathf.Clamp(Mathf.Abs(collision.GetContact(0).point.z), minimumBump, maximumBump) / Mathf.Clamp((-.75f * counter) + 10, 1, 10);
            counter = 0;
        }
    }
}
