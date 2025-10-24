using UnityEngine;

public class BumpLogic : MonoBehaviour
{

    public float bumpStrength = 25;
    public float minimumBump = .5f;
    public float maximumBump = 4f;
    public PlayerController player;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bomb" && player.isAlive)
        {
            player.transform.Rotate(new Vector3(Mathf.Sign(collision.GetContact(0).point.z) * Mathf.Clamp(Mathf.Abs(collision.GetContact(0).point.z), minimumBump, maximumBump) * bumpStrength, 0, 0));
        }
    }
}
