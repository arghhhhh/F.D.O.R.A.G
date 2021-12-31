using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 15;
    public float jumpHeight = 4;
    Vector3 moveDir;
    bool jumpAllowed;
    float distanceToGround;
    float x = 0, z = 0;
    Rigidbody player;
    string oldPlanet;
    string newPlanet;

    private void Start()
    {
        player = GetComponent<Rigidbody>();
    }
    void Update()
    {
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10))
        {
            distanceToGround = hit.distance;
            oldPlanet = hit.collider.gameObject.name;

            if (newPlanet != oldPlanet)
            {
                jumpAllowed = false;
                Invoke("AllowJumping", 1.0f);
            }

            if (distanceToGround <= 0.6f && jumpAllowed)
            {
                
                if (Input.GetKey(KeyCode.Space)) player.AddForce(transform.up * 1000 * jumpHeight * Time.deltaTime);
                x = Input.GetAxisRaw("Horizontal");
                z = Input.GetAxisRaw("Vertical");
            }

            newPlanet = hit.collider.gameObject.name;
        }

        moveDir = new Vector3(x, 0, z).normalized;

        if (Input.GetKey(KeyCode.E)) transform.Rotate(0, 150 * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.Q)) transform.Rotate(0, -150 * Time.deltaTime, 0);

    }
    private void FixedUpdate()
    {
        player.MovePosition(player.position + transform.TransformDirection(moveDir) * moveSpeed * Time.deltaTime);
    }
    void AllowJumping()
    {
        jumpAllowed = true;
    }
}
