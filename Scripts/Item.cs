using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int solutionIndex;
    public bool scoreGot;
    public int spawnPoint;
    public GameObject[] particles;

    private void Start()
    {
        scoreGot = false;
    }
    public void Success()
    {
        Instantiate(particles[0], transform.position, Quaternion.identity);
    }
    public void Fail()
    {
        Instantiate(particles[1], transform.position, Quaternion.identity);
    }
}