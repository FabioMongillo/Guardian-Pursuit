using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PublicVars : MonoBehaviour
{
    public List<Transform> prisonersList;
    public static List<Transform> prisoners;
    public List<Transform> guardList;
    public static List<Transform> guards;
    public List<Transform> heroList;
    public static List<Transform> hero;
    public static int score = 0;
    public static float speed = 3;
    public static int numOfEnemies;
    public static List<Transform> visibleHero = new List<Transform>();

    public float setMaxSpeed;
    public static float maxSpeed;

    //Wander range
    [Header("Wander ranges 1")]
    public int setminx1;
    public int setmaxx1;
    public int setminz1;
    public int setmaxz1;
    public static int minx1;
    public static int maxx1;
    public static int minz1;
    public static int maxz1;

    //Wander range 2

    public static int minx2 = -20;
    public static int maxx2 = -10;
    public static int minz2 = 0;
    public static int maxz2 = 10;

    //Wander range 3

    public static int minx3 = 0;
    public static int maxx3 = 10;
    public static int minz3 = -20;
    public static int maxz3 = -10;

    //Wander range 4
    public static int minx4 = -20;
    public static int maxx4 = -10;
    public static int minz4 = -20;
    public static int maxz4 = -10;

    //Wander range 5
    public static int minx5 = -10;
    public static int maxx5 = 0;
    public static int minz5 = -10;
    public static int maxz5 = 0;


    private void Start()
    {
        prisoners = prisonersList;
        guards = guardList;
        hero = heroList;
        numOfEnemies = guardList.Count;

        //Setting wander values 1
        minx1 = setminx1;
        maxx1 = setmaxx1;
        minz1 = setminz1;
        maxz1 = setmaxz1;


        maxSpeed = setMaxSpeed;

    }
}
