using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerPos;
    public Vector3 offset = new Vector3(-26, 21.5f, 0);
    public float gainPerFrame = 0.01f;
    private Vector3 targetPos;
    private float currentXOffset;
    public LogicSystem logic;
    
    void Start() {
        targetPos = offset + playerPos.position;
    }

    void LateUpdate()
    {
        //targetPos = new Vector3(playerPos.position + offset.x, offset.y, offset.z);
        if (logic.getScore() > 99) {
            currentXOffset += gainPerFrame * Time.deltaTime;
        }
        targetPos.x = playerPos.position.x + currentXOffset + offset.x;
        transform.position = targetPos;
    }
}