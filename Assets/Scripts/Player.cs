using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Public variables
    public Vector3 spawnLocation;


    //Private variables
    private Rigidbody rb;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.velocity = movement * PublicVars.maxSpeed;

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall" || other.tag == "Base")
        {
            transform.position = spawnLocation;

        }
    }
}
