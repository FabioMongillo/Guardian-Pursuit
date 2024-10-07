using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private int numOfRays = 4;
    private float angle = 45;
    private float rayRange = 10f;
    private float targetVel = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Collision detection
        var deltaPos = Vector3.zero;
        for (int i = 0; i < numOfRays; i++)
        {
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / ((float)numOfRays - 1)) * angle * 2 - angle, this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward * rayRange;

            var ray = new Ray(this.transform.position, direction);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, rayRange))
            {
                deltaPos -= (1.0f / numOfRays) * targetVel * direction;
            }

            else
            {
                deltaPos += (1.0f / numOfRays) * targetVel * direction;
            }

        }//End of for loop


        this.transform.position += deltaPos * Time.deltaTime;
    }


    //Draw rays
    void OnDrawGizmos()
    {
        for (int i = 0; i < numOfRays; i++)
        {
            var rotation = this.transform.rotation;
            var rotationMod = Quaternion.AngleAxis((i / ((float)numOfRays - 1)) * angle * 2 - angle, this.transform.up);
            var direction = rotation * rotationMod * Vector3.forward * rayRange;
            Gizmos.DrawRay(this.transform.position, direction);
        }
    }
}
