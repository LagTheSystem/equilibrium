using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerPos;
    public PlayerController player;
    public LogicSystem logic;
    public Vector3 offset = new(-26, 21.5f, 0);
    [Header("Camera Creep")]
    public bool creepEnabled = true;
    public float creepStartPos = 100;
    public float gainSpeed = 0.05f;
    public float gainAcceleration = 0.01f;
    public float recoverySpeed = 1f;
    public float maxGainSpeed = 12.5f;
    private float currentXOffset = 0;
    private float xMaximum = 5;
    private Vector3 targetPos;
    private float currentGainSpeed;



    void Start()
    {
        targetPos = offset + playerPos.position;

        if (logic.quickMode)
        {
            creepStartPos = 0;
            gainSpeed = 0.1f;
            gainAcceleration = 0.025f;
        }
    }

    void LateUpdate()
    {
        //targetPos = new Vector3(playerPos.position + offset.x, offset.y, offset.z);
        currentGainSpeed = (gainAcceleration * (logic.getScore() - creepStartPos)) + gainSpeed;
        if (logic.getScore() >= creepStartPos && creepEnabled)
        {
            if (player.inputVector.y > 0 && currentXOffset > -xMaximum)
            {
                currentXOffset -= recoverySpeed * Time.deltaTime;
            }
            else
            {
                currentXOffset += Mathf.Clamp(currentGainSpeed, 0, maxGainSpeed) * Time.deltaTime;
            }
        }
        targetPos.x = playerPos.position.x + currentXOffset + offset.x;
        transform.position = targetPos;
    }
}