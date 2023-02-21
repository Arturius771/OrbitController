using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitController : MonoBehaviour {

    //Scene
    public Rigidbody rb;
    public static List<OrbitController> Bodies;

    //Physics
    public float g = 1;

    //Object instance settings
    public float attractionForce;
    public bool calculateRequireOrbitatlVelocity;
    public Rigidbody centralMass;
    public float orbitalVelocity;
    public Vector3 attractionVector3;


    void Start() {
        if (calculateRequireOrbitatlVelocity) {
            Vector3 direction = rb.position - centralMass.position;
            float distance = direction.magnitude;
            orbitalVelocity = Mathf.Sqrt(g * centralMass.mass / distance);
        }
        rb.AddRelativeForce(-orbitalVelocity, 0, 0, ForceMode.VelocityChange);
    }

    void FixedUpdate() {
        foreach (OrbitController objects in Bodies) {
            if (objects != this)
                Attract(objects);
        }
    }

    void OnEnable() {
        if (Bodies == null)
            Bodies = new List<OrbitController>();

        Bodies.Add(this);
    }

    void OnDisable() {
        Bodies.Remove(this);
    }

    void Attract(OrbitController otherObject) {
        Rigidbody otherRb = otherObject.rb;
        Vector3 direction = rb.position - otherRb.position;
        float distance = direction.magnitude;

        if (distance == 0f) {
            return;
        }

        //Relevant formulas from OU Galaxies and Cosmology p16
        attractionForce = g * otherRb.mass * rb.mass / Mathf.Pow(distance, 2);

        //Pull other objects closer
        attractionVector3 = direction.normalized * attractionForce;
        otherRb.AddForce(attractionVector3);
    }
}
