using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset = new Vector3(15f, 60f, 0f);

    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
