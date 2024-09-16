using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    // rotation limits for flying
    public float minX = 60f;
    public float maxX = 120f;
    public float speed = .5f;

    // speed limits for birds
    public float lowSpeedLimit = 0.6f;
    public float highSpeedLimit = 1.4f;
    public float speedAdjustment = 0.01f;

    // movement for flying
    public float moveSpeed = 10f;
    public float internalMoveSpeed;
    public float moveSmoothing = 0.05f;
    private Vector3 zeroVec = Vector3.zero;

    // access to rigidbody and model
    public Rigidbody rb;
    public Transform model;
    public Transform animModel;

    // utility for flying rotation
    private float change;
    private float smooth;
    private float mouse;
    private float newX;
    private float newY;

    // utility for flying speed
    private float horizontalMove = 0.0f;

    // utility for stopping tilt & hovering
    public float tiltSmooth = 0.3f;
    private float sinCount = 0f;
    private Vector3 startingPos = Vector3.zero;
    public bool hovering = true;

    // utility for mouse shtuff
    private float lastMousePositionX;
    private float lastMousePositionY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        internalMoveSpeed = moveSpeed;
        lastMousePositionX = Input.GetAxisRaw("Mouse Y");
        lastMousePositionY = Input.GetAxisRaw("Mouse X");
    }

    // Update is called once per frame
    void Update()
    {
        // update flying velocity
        horizontalMove = Input.GetAxisRaw("Vertical");
        if (horizontalMove < 0) horizontalMove = 0;

        // mousePositionY corresponds to left/right and X to up/down
        // will go back and update variable names at some point

        // find change in mouse position
        float newPositionX = Input.GetAxisRaw("Mouse Y");
        float newPositionY = Input.GetAxisRaw("Mouse X");
        float deltaX = lastMousePositionX - newPositionX;
        float deltaY = lastMousePositionX - newPositionY;
        lastMousePositionX = newPositionX;
        lastMousePositionY = newPositionY;

        newX += 1000 * Time.deltaTime * speed * deltaX;
        newY -= 50 * Time.deltaTime * speed * deltaY;

        // hide cursor again when clicking into game again
        // WILL NEED TO CHANGE THIS! to show cursor when decorating nest
        if (Input.GetMouseButtonDown(0)) Cursor.visible = false;
        
        // avoid big scary numbers
        if (newX < -360) newX = 360 + newX;
        if (newY < 0) newY = 360 + newY;
        if (newY > 360) newY = newY - 360;

        // clamp x rotation
        newX = Mathf.Clamp(newX, minX, maxX);

        // update player rotation
        transform.rotation = Quaternion.Euler(newX, newY, transform.rotation.z);
        animModel.rotation = Quaternion.Euler(newX, newY, animModel.rotation.z);

        // handle movement logic
        Vector3 targetVelocity = transform.forward * horizontalMove * internalMoveSpeed;
        Vector3 curVelocity = rb.velocity;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref zeroVec, moveSmoothing);
        // make numbers nicer
        if (rb.velocity.magnitude < .01){
            if (startingPos == Vector3.zero) {
                startingPos = this.transform.position;
            }
            hovering = true;
            rb.velocity = Vector3.zero;
            sinCount = sinCount + 0.01f;
            this.transform.position = new Vector3(startingPos.x, startingPos.y + (Mathf.Sin(sinCount) / 15), startingPos.z);
        } else {
            sinCount = 0;
            startingPos = Vector3.zero;
            hovering = false;
        }
        // check if decelerating to tilt model down
        if (horizontalMove == 0 && rb.velocity.magnitude != 0) {
            Quaternion target = Quaternion.Euler(-15, 0, 0);
            model.rotation = Quaternion.Slerp(model.rotation, target, tiltSmooth);
        } else {
            Quaternion target = Quaternion.Euler(0, 0, 0);
            model.rotation = Quaternion.Slerp(model.rotation, target, tiltSmooth);
        }
    }

    public bool GetHovering() {
        return hovering;
    }
}
