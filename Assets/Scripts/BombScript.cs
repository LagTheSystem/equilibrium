using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour
{
    public Vector3 targetPosition;
    public float launchAngle = 45f;
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable() {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Kill bombs when off screen
        if (transform.position.y < -20)
        {
            ObjectPool.SharedInstance.destroyInstance(gameObject);
        }
    }

    public void Launch()
    {
        // ChatGPT magic code to allow for physics based projectile tracking
        Vector3 direction = targetPosition - transform.position;
        float h = direction.y; // height difference
        direction.y = 0;
        float distance = direction.magnitude;
        float angle = launchAngle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(angle);
        distance += h / Mathf.Tan(angle);

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * angle));
        Vector3 velocityVector = velocity * direction.normalized;

        rb.linearVelocity = velocityVector;
    }

    void OnCollisionEnter(Collision collision)
    {
        // BumpReceiver is the player, BombDestroyer is the rope
        if (collision.gameObject.CompareTag("BumpReceiver")) {
            StartCoroutine(explode());
        } else if (collision.gameObject.CompareTag("BombDestroyer")) {
            ObjectPool.SharedInstance.destroyInstance(gameObject);
        }
    }

    IEnumerator explode() {
        // Disable bomb model and run particle systems
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        // Wait for 2 seconds to allow the animation to finish
        yield return new WaitForSeconds(2);
        ObjectPool.SharedInstance.destroyInstance(gameObject);
    }
}
