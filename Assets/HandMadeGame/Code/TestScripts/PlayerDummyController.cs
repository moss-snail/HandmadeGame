using UnityEngine;

public class PlayerDummyController : MonoBehaviour
{
    private void FixedUpdate()
    {
        const float speed = 0.1f;
        Vector3 offset = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
            offset.z += speed;
        else if (Input.GetKey(KeyCode.S))
            offset.z -= speed;

        if (Input.GetKey(KeyCode.A))
            offset.x -= speed;
        else if (Input.GetKey(KeyCode.D))
            offset.x += speed;

        transform.position += offset;
    }
}
