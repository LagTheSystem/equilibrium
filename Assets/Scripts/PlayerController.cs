using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject map;
    [Header("Movement")]
    public float moveSpeed = 10;
    public float rotateSpeed = 20;
    public float stationaryMult = 2;
    [Header("Gravity")]
    public float gravityStrength = 175;
    public float gravityRandomness = 30;
    internal bool isAlive = true;
    private LogicSystem logic;
    internal Vector2 inputVector;
    private Animator animator;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicSystem>();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }
    void Update()
    {
        float rot = gameObject.transform.rotation.x;

        float gravityValue = (gravityStrength * Mathf.Pow(rot, 2)) + Random.Range(0, gravityRandomness);
        float direction = Mathf.Sign(rot);

        animator.SetFloat("moveDirection", inputVector.y);
        animator.SetBool("isAlive", isAlive);

        transform.Rotate(new Vector3(gravityValue * direction, 0, 0) * Time.deltaTime);
        if ((rot > .60 || rot < -.60) && isAlive)
        {
            die();
        }

        if (!(gameObject.transform.position.x < -24 && inputVector.y < 0) && isAlive)
        {
            transform.Translate(new Vector3(inputVector.y, 0, 0) * moveSpeed * Time.deltaTime);
        }

        transform.Rotate(new Vector3(-inputVector.x, 0, 0) * rotateSpeed * (-Mathf.Abs((stationaryMult - 1) * inputVector.y) + stationaryMult) * Time.deltaTime);
    }

    public void die()
    {
        isAlive = false;
        var rigidbody = gameObject.transform.GetChild(0).GetComponent<Rigidbody>();
        transform.GetChild(0).transform.parent = null;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationZ;
        rigidbody.useGravity = true;
        logic.gameOver();
    }
}
