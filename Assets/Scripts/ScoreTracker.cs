using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    float tTime;


    // Start is called before the first frame update
    void Start()
    {
        text.text = "Score: " + PublicVars.score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " + PublicVars.score.ToString();

    }
}
