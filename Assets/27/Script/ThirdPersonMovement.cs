using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 moveDir;
    Vector3 velocity;
    public float gravity = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.25f;
    public LayerMask groundMask;
    bool isCollision;
    public float jumpHeight = 3f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (Input.GetButtonDown("Jump"))
        {
            //velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // not sure where Brackey's got this formula from but I like mine better
            velocity.y = jumpHeight + (gravity * Time.deltaTime)*(1f - 0.5f * Time.deltaTime);
        }

        isCollision = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (!isCollision)
        { //increase magnitude of downward y-velocity while player is in the air
            velocity.y += gravity * Time.deltaTime;
        }

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            controller.Move((moveDir.normalized * speed * Time.deltaTime) + (velocity * Time.deltaTime * 0.5f)); //adds movement vector to velocity vector
        }
    }
}
