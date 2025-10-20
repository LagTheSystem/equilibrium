using UnityEngine;

public class BumpLogic : MonoBehaviour
{

    public float bumpStrength = 25;
    public float minimumBump = .1f;
    public PlayerController player;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bomb" && player.isAlive)
        {
            player.transform.Rotate(new Vector3(Mathf.Clamp(collision.GetContact(0).point.z, .5f, 2) * bumpStrength, 0, 0));
        }
    }
}
