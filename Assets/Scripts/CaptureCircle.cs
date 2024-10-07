using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureCircle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hero")
        {
            Destroy(other.gameObject);
        }
    }
}
