using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task2_Guardian : MonoBehaviour
{
    //Public variables
    public float maxSpeed;
    public Transform target;
    public LayerMask guardLayer;
    public LayerMask avoidanceMask;


    //Private variables
    private Vector3 velocity;
    private float keepChasing = 0f;
    private int numRays = 17;
    private float angle = 45;
    private float rayRange = 1f;
    private float wallRayRange = 3f;

    //For wandering
    private Vector3 moveSpot;
    private float waitTime = 0.5f;
    private float startWaitTime = 0.5f;


    //FOV variables
    public float radius;
    [Range(0, 360)]
    public float fovAngle;
    public GameObject playerRef;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    public bool canSeePlayer;
    private Hero lastSeenHero;




    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Hero");
        StartCoroutine(FOVRoutine());
        moveSpot = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        //Calling movement functions
        if (!canSeePlayer && keepChasing <= 0f)
        {
            wander();
            avoidGuards();
            avoidWalls();
        }
        else
        {
            findTarget();
            seek();
            avoidGuards();
            avoidWalls();
        }



        //Updating movement
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;
        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity * Time.deltaTime);
        }

        keepChasing -= Time.deltaTime;


    }


    void wander()
    {
        Vector3 desiredVel = moveSpot - transform.position;
        desiredVel = desiredVel.normalized * maxSpeed;
        Vector3 steering = desiredVel - velocity;

        velocity += steering * 0.25f;

        if (Vector3.Distance(transform.position, moveSpot) < 0.25f)
        {
            velocity = Vector3.zero;

            if (waitTime <= 0)
            {
                moveSpot = new Vector3(Random.Range(PublicVars.minx1, PublicVars.maxx1), 0.3f, Random.Range(PublicVars.minz1, PublicVars.maxz1));
                waitTime = startWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }


    //Find target
    void findTarget()
    {
        float distance = float.PositiveInfinity;

        for (int i = 0; i < PublicVars.hero.Count; i++)
        {
            if (Vector3.Distance(PublicVars.hero[i].position, transform.position) < distance)
            {
                distance = Vector3.Distance(PublicVars.hero[i].position, transform.position);
                target = PublicVars.hero[i];
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


    void avoidWalls()
    {
        //Collision avoidance
        var deltaPos = Vector3.zero;
        for (int i = 0; i < numRays; i++)
        {
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / ((float)numRays - 1)) * angle * 2 - angle, this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward * wallRayRange;

            var ray = new Ray(this.transform.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, wallRayRange, avoidanceMask))
            {
                deltaPos -= Vector3.Cross(hit.normal, Vector3.up) * (1.0f / numRays);
            }

            else
            {
                deltaPos = Vector3.zero;
            }
        }

        velocity += deltaPos * 5;
    }


    //Avoid other guardians
    void avoidGuards()
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

            if (Physics.Raycast(ray, out hit, rayRange, guardLayer))
            {
                deltaPos -= Vector3.Cross(hit.normal, Vector3.up);

            }

            else
            {
                deltaPos += Vector3.zero;
            }
        }

        velocity += deltaPos * 3;
    }



    //FOV functions
    private IEnumerator FOVRoutine()
    {
        float delay = 0.0f;
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            fieldOfViewCheck();
        }
    }


    private void fieldOfViewCheck()
    {
        PublicVars.visibleHero.Clear();
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform fovTarget = rangeChecks[0].transform;
            Vector3 directionToTarget = (fovTarget.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, fovTarget.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    //PublicVars.visibleHero.Add(fovTarget);
                    canSeePlayer = true;
                    keepChasing = 1f;

                    Hero heroVar = fovTarget.GetComponent<Hero>();
                    if (heroVar != null)
                    {
                        heroVar.flee();
                        heroVar.isDetected = true;
                        lastSeenHero = heroVar;
                    }
                }

                else
                {
                    canSeePlayer = false;
                }
            }

            else
            {
                canSeePlayer = false;
            }
        }

        else if (canSeePlayer)
        {
            //PublicVars.visibleHero.Clear();
            canSeePlayer = false;
            lastSeenHero.isDetected = false;
            lastSeenHero = null;
        }
    }


    //Remove any seen heros on destruction
    private void OnDestroy()
    {
        lastSeenHero.isDetected = false;
        lastSeenHero = null;
    }


}
