using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerPos;
    public PlayerController player;
    public LogicSystem logic;
    public Vector3 offset = new Vector3(-26, 21.5f, 0);
    [Header("Camera Creep")]
    public float creepStartPos = 100;
    public float gainSpeed = 0.05f;
    public float gainAcceleration = 0.01f;
    public float recoverySpeed = 1f;
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
            gainAcceleration = 0.05f;
        }
    }

    void LateUpdate()
    {
        //targetPos = new Vector3(playerPos.position + offset.x, offset.y, offset.z);
        currentGainSpeed = (gainAcceleration * (logic.getScore() - creepStartPos)) + gainSpeed;
        if (logic.getScore() >= creepStartPos)
        {
            if (player.inputVector.y > 0 && currentXOffset > -xMaximum)
            {
                currentXOffset -= (currentGainSpeed + recoverySpeed) * Time.deltaTime;
            }
            else
            {
                currentXOffset += currentGainSpeed * Time.deltaTime;
            }
        }
        targetPos.x = playerPos.position.x + currentXOffset + offset.x;
        transform.position = targetPos;
    }
}