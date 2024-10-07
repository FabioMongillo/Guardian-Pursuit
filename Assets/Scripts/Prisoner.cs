using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UIElements;

public class Prisoner : MonoBehaviour
{
    //Public variables
    public Transform[] hero;
    public Transform baseLoc;
    public float maxSpeed;


    //Private variables
    private Vector3 velocity;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Calling all required functions
        seek();


        //Updating movement
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        transform.position += velocity * Time.deltaTime;

        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }

    }


    void seek()
    {
        float distance = float.PositiveInfinity;
        Transform target = hero[0];

        for (int i = 0; i < hero.Length; i++)
        {
            if (Vector3.Distance(hero[i].position, transform.position) < distance)
            {
                distance = Vector3.Distance(hero[i].position, transform.position);
                target = hero[i];
            }
        }

        if (Vector3.Distance(target.position, transform.position) <= 0.75)
        {
            Vector3 desiredVel = target.transform.position - transform.position;
            desiredVel = desiredVel.normalized * maxSpeed;
            Vector3 steering = desiredVel - velocity;

            velocity += steering;

        }

        else
        {
            velocity = Vector3.zero;    
        }
    }


}
