using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    float totalTime;
    private int minutes;
    private int seconds;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "0:00";
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(totalTime / 60);
        seconds = Mathf.FloorToInt(totalTime % 60);
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }
}
