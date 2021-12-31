using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{
    public GameObject planet1;
    public GameObject planet2;
    FauxGravityAttractor p1Attractor;
    FauxGravityAttractor p2Attractor;
    public FauxGravityAttractor attractor;
    private Rigidbody rb;
    private Transform player;
    void Start()
    {
        p1Attractor = planet1.GetComponent<FauxGravityAttractor>();
        p2Attractor = planet2.GetComponent<FauxGravityAttractor>();
        rb = GetComponent<Rigidbody>();
        //player.constraints = RigidbodyConstraints.FreezeRotation;
        rb.freezeRotation = true;
        rb.useGravity = false;
        player = transform;
    }

    void Update()
    {
        float distanceToPlanet1 = Vector3.Distance(rb.position, planet1.transform.position);
        float distanceToPlanet2 = Vector3.Distance(rb.position, planet2.transform.position);
        float gForce1 = p1Attractor.gravity / distanceToPlanet1;
        float gForce2 = p2Attractor.gravity / distanceToPlanet2;
        if (gForce1 <= gForce2) p1Attractor.Attract(player);
        else p2Attractor.Attract(player);
    }
}
