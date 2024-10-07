using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prisoner")
        {
            PublicVars.prisoners.Remove(other.transform);
            Destroy(other.gameObject);
            PublicVars.score += 25;
        }

        if (other.tag == "Guard")
        {
            PublicVars.guards.Remove(other.transform);
            Destroy(other.gameObject);
            PublicVars.score += 1;
            PublicVars.numOfEnemies -= 1;
            PublicVars.visibleHero = new List<Transform>();
        }



    }




}
