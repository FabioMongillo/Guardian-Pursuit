using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : MonoBehaviour
{
    //Public variables
    public float maxZ;
    public float minZ;
    public bool moveZ;

    //Private variables
    private float moveSpeed = 1f;
    private bool movingPositive = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (moveZ)
        {
            if (movingPositive)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                if (transform.position.z >= maxZ)
                {
                    movingPositive = false;
                }
            }

            else
            {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

                if (transform.position.z <= minZ)
                {
                    movingPositive = true;
                }
            }
        }

        else
        {
            if (movingPositive)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                if (transform.position.x >= maxZ)
                {
                    movingPositive = false;
                }
            }

            else
            {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

                if (transform.position.x <= minZ)
                {
                    movingPositive = true;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hero")
        {
            PublicVars.hero.Remove(other.transform);
            Destroy(other.gameObject);
        }

    }
}
