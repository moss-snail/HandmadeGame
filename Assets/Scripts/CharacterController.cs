using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // rotation limits for flying
    public float minX = 60f;
    public float maxX = 120f;
    public float speed = .5f;

    // movement for flying
    public float moveSpeed = 10f;
    public float moveSmoothing = 0.05f;
    private Vector3 zeroVec = Vector3.zero;

    // access to rigidbody
    public Rigidbody rb;

    // utility for flying rotation
    private float change;
    private float smooth;
    private float mouse;
    private float newX;
    private float newY;

    // utility for flying speed
    private float horizontalMove = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // update flying velocity
        horizontalMove = Input.GetAxisRaw("Vertical");
        if (horizontalMove < 0) horizontalMove = 0;

        // check if mouse is far enough from center to look up/down
        if (Input.mousePosition.y < Screen.height / 5) newX += speed;
        else if (Input.mousePosition.y > 4 * Screen.height / 5) newX -= speed;

        // check if mouse is far enough from center to look left/right
        if (Input.mousePosition.x < Screen.width / 5) newY -= speed;
        else if (Input.mousePosition.x > 4 * Screen.width / 5) newY += speed;
        
        // avoid big scary numbers
        if (newX < -360) newX = 360 + newX;
        if (newY < 0) newY = 360 + newY;
        if (newY > 360) newY = newY - 360;

        // clamp x rotation
        newX = Mathf.Clamp(newX, minX, maxX);

        // update player rotation
        transform.rotation = Quaternion.Euler(newX, newY, transform.rotation.z);

        // handle movement logic
        Vector3 targetVelocity = transform.forward * horizontalMove * moveSpeed;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref zeroVec, moveSmoothing);
    }
}
