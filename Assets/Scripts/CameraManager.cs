using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerPos;
    public Vector3 offset = new Vector3(-26, 21.5f, 0);

    void LateUpdate()
    {
        transform.position = playerPos.position + offset;
    }
}