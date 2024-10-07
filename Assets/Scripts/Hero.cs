using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class Hero : MonoBehaviour
{
    //Public variables
    public float maxSpeed;
    public Transform baseLoc;
    public float stopRadius;
    public float slowRadius;
    public bool isDetected = false;



    //Private variables
    private Vector3 velocity;
    private Transform target;
    private float fleeTimer = 0;
    private int numRays = 4;
    private float angle = 45;
    private float rayRange = 2f;

    //Random
    public LayerMask avoidanceMask;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Calling all required functions
        findTarget();
        if (!isDetected && fleeTimer <= 0)
        {
            seek();
            avoid();
        }

        else if (isDetected)
        {
            fleeTimer = 1.0f;
            flee();
            avoid();
        }

        else if (!isDetected && !(fleeTimer <= 0))
        {
            flee();
            avoid();
        }



        //Updating movement
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        fleeTimer -= Time.deltaTime;

    }


    //Finds the closest prisoner
    void findTarget()
    {
        if (!isDetected)
        {
            float distance = float.PositiveInfinity;

            for (int i = 0; i < PublicVars.prisoners.Count; i++)
            {
                if (Vector3.Distance(PublicVars.prisoners[i].position, transform.position) < distance)
                {
                    distance = Vector3.Distance(PublicVars.prisoners[i].position, transform.position);
                    target = PublicVars.prisoners[i];
                }
            }

            if (distance <= 0.5f)
            {
                target = baseLoc;
            }
        }

        else if (isDetected)
        {
            float distance = float.PositiveInfinity;

            for (int i = 0; i < PublicVars.guards.Count; i++)
            {
                if (Vector3.Distance(PublicVars.guards[i].position, transform.position) < distance)
                {
                    distance = Vector3.Distance(PublicVars.guards[i].position, transform.position);
                    target = PublicVars.guards[i];
                }
            }
        }


    }


    void seek()
    {

        Vector3 desiredVel = target.transform.position - transform.position;
        desiredVel = desiredVel.normalized * maxSpeed;
        Vector3 steering = desiredVel - velocity;

        velocity += steering * 0.25f;

    }


    public void flee()
    {
        Vector3 desiredVel = transform.position - target.transform.position;
        desiredVel = desiredVel.normalized * maxSpeed;
        Vector3 steering = desiredVel - velocity;

        velocity += steering;
    }

    void avoid()
    {
        //Collision avoidance
        var deltaPos = Vector3.zero;
        for (int i = 0; i < numRays; i++)
        {
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / ((float)numRays - 1)) * angle * 2 - angle, this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward * rayRange;

            var ray = new Ray(this.transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayRange, avoidanceMask))
            {
                velocity = Vector3.zero;
                deltaPos -= Vector3.Cross(hit.normal, Vector3.up);
                break;
            }

            else
            {
                deltaPos = Vector3.zero;
            }
        }

        velocity += deltaPos * 3;
    }

    void arrive()
    {
        Vector3 desiredVel = target.position - transform.position;
        float distance = desiredVel.magnitude;
        desiredVel = desiredVel.normalized * maxSpeed;

        if (distance <= stopRadius)
        {
            desiredVel *= 0;
        }

        else if (distance < slowRadius)
        {
            desiredVel *= (distance / slowRadius);
        }


        Vector3 steering = desiredVel - velocity;

        velocity += steering;

    }
}
