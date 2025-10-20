using UnityEngine;

public class BombScript : MonoBehaviour
{

    public Vector3 targetPosition;
    public float launchAngle = 45f;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable() {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < -20)
        {
            gameObject.SetActive(false);
        }
    }

    public void Launch()
    {
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
        if (collision.gameObject.tag == "BumpReceiver") {
            gameObject.SetActive(false);
        }
    }
}
