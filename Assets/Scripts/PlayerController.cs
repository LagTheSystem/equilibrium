using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public GameObject map;
    [Header("Movement")]
    public float moveSpeed = 10;
    public float rotateSpeed = 20;
    public float stationaryMult = 2;
    private float fallAssist = 1;
    private float holdCounter = 0;
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

    void OnEmote()
    {
        animator.SetBool("isEmoting", true);
    }

    void Update()
    {
        float rot = gameObject.transform.rotation.x;
        // How much gravity is applied based off of current rotation angle
        float gravityValue = (gravityStrength * Mathf.Pow(rot, 2)) + Random.Range(0, gravityRandomness);
        float direction = Mathf.Sign(rot);

        // Pass player data along to animator
        animator.SetFloat("moveDirection", inputVector.y);
        animator.SetBool("isAlive", isAlive);

        // Apply gravity
        transform.Rotate(new Vector3(gravityValue * direction, 0, 0) * Time.deltaTime);
        
        // Check if player has lost balance
        if ((rot > .70 || rot < -.70) && isAlive)
        {
            die();
        }

        // Forwards/backwards movement input
        // Prevents player from moving backwards past the starting point
        if (!(gameObject.transform.position.x < -24 && inputVector.y < 0) && isAlive)
        {
            transform.Translate(new Vector3(inputVector.y * moveSpeed * Time.deltaTime, 0, 0));
        }

        // Fall assistance
        // When at extreme angles (72 degrees or higher), the longer you hold in the same direction, the more of a rotational speed boost you will get
        // This makes recovery easier and a lot more satisfying
        if (inputVector.x != 0 && Mathf.Abs(rot) > .4)
        {
            holdCounter += Time.deltaTime;
        }
        else
        {
            holdCounter = 0;
        }
        // Rotational speed boost is represented as log(2x) + 1, clamped between 1 and 3
        // After 5 seconds your rotational speed will have doubled
        fallAssist = Mathf.Clamp(Mathf.Log10(2 * holdCounter) + 1, 1, 3);
        
        // Rotational movement input
        transform.Rotate(new Vector3(-inputVector.x * rotateSpeed * (-Mathf.Abs((stationaryMult - 1) * inputVector.y) + stationaryMult) * fallAssist * Time.deltaTime, 0, 0));
    }

    public void die()
    {
        isAlive = false;
        var rb = gameObject.transform.GetChild(0).GetComponent<Rigidbody>();
        // Detach player model from pivot point and enable gravity to allow it to fall
        transform.GetChild(0).transform.parent = null;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = true;
        logic.gameOver();
    }

    public bool isMoving()
    {
        return inputVector.y != 0;
    }
}
