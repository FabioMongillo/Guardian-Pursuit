using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task9_Hero : MonoBehaviour
{
    //Public variables
    private float maxSpeed = PublicVars.maxSpeed;
    public Transform baseLoc;
    public float stopRadius;
    public float slowRadius;
    public LayerMask avoidanceMask;
    public Transform playerLoc;
    public Transform respawnLocation;
    public GameObject guardPrefab;
    public Vector3 spawnPosition;

    //Private variables
    private Vector3 velocity;
    private Transform target;
    private int numRays = 80;
    private float angle = 30;
    private float rayRange = 2f;
    private float detectedAvoidSpeed = 1f;
    private bool wallSpotted = false;
    private bool spottedByPlayer = false;


    //Detection
    public bool isDetected = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Checking if detected
        if (isDetected && !wallSpotted && !spottedByPlayer)
        {
            detectedAvoidSpeed = 0.2f;
            maxSpeed = PublicVars.maxSpeed / 2;

        }
        else
        {
            detectedAvoidSpeed = 0.2f;
            maxSpeed = PublicVars.maxSpeed;
        }


        //Determining target
        findPlayer();
        if (!spottedByPlayer)
        {
            findTarget();
        }

        //Determining correct movement functions and calling them
        if (spottedByPlayer)
        {
            flee();
        }
        else if (PublicVars.numOfEnemies == 0)
        {
            seek();
        }

        else if (!isDetected)
        {
            seek();

        }

        else if (isDetected)
        {
            flee();
            seekBase();
        }

        else
        {
            flee();
            seekBase();
        }
        avoid();



        //Updating movement
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }



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

    public void findTargetGuard()
    {
        if (!isDetected)
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

    public void findPlayer()
    {
        float distance = Vector3.Distance(playerLoc.transform.position, transform.position);

        if (distance <= 2)
        {
            spottedByPlayer = true;
            target = playerLoc.transform;
        }

        else
        {
            spottedByPlayer = false;
        }
    }


    void seek()
    {

        Vector3 desiredVel = target.transform.position - transform.position;
        desiredVel = desiredVel.normalized * maxSpeed;
        Vector3 steering = desiredVel - velocity;

        velocity += steering * detectedAvoidSpeed;

    }


    public void seekBase()
    {
        Vector3 desiredVel = baseLoc.transform.position - transform.position;
        desiredVel = desiredVel.normalized * (maxSpeed);
        Vector3 steering = desiredVel - velocity;

        velocity += steering * detectedAvoidSpeed;
    }


    void flee()
    {
        Vector3 desiredVel = transform.position - target.transform.position;
        desiredVel = desiredVel.normalized * maxSpeed;
        Vector3 steering = desiredVel - velocity;

        velocity.x += steering.x * detectedAvoidSpeed;
        velocity.z += steering.z * detectedAvoidSpeed;
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
                wallSpotted = true;
                deltaPos -= Vector3.Cross(hit.normal, Vector3.up);

            }

            else
            {
                wallSpotted = false;
                deltaPos += Vector3.zero;
            }
        }

        velocity += deltaPos * 5f;
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

        velocity += steering * detectedAvoidSpeed;

    }


    private void OnTriggerEnter(Collider other)
    {
        //Stop detection
        if (other.tag == "Base")
        {
            StartCoroutine(leaveBase());
        }

        //Respawn
        if (other.tag == "Player")
        {
            transform.position = respawnLocation.position;

            if (PublicVars.score <= 9)
            {
                PublicVars.score = 0;
            }
            else
            {
                PublicVars.score -= 10;
            }

            Instantiate(guardPrefab, spawnPosition, Quaternion.identity);


        }
    }


    IEnumerator leaveBase()
    {
        yield return new WaitForSeconds(1f);

        isDetected = false;
    }




}
