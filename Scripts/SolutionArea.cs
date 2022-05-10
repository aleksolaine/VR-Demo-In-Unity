using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolutionArea : MonoBehaviour
{
    public int solutionIndex;
    public Material[] materials;
    public GameObject[] particles;

    private int wrongItemsInArea;
    private bool rightItemInArea;

    private void Start()
    {
        materials[2] = GetComponentInParent<MeshRenderer>().material;
        rightItemInArea = false;
    }
    private void OnTriggerEnter(Collider other)
    {

        Item item = other.gameObject.GetComponent<Item>();
        Debug.Log(other.gameObject.name);
        if (item.solutionIndex == solutionIndex)
        {
            rightItemInArea = true;
            if (!item.scoreGot)
            {
                item.scoreGot = true;
                GameManager.instance.GetPoints();
                //Instantiate(particles[0], transform.position, Quaternion.identity);
                item.Success();
            }
        }
        else
        {
            wrongItemsInArea++;
            GameManager.instance.respawnableItems.Add(item.gameObject);
            item.Fail();
            item.gameObject.SetActive(false);
            //Instantiate(particles[1], transform.position, Quaternion.identity);
        }
        UpdateColor();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Item>().solutionIndex == solutionIndex)
        {
            rightItemInArea = false;
        } 
        else
        {
            wrongItemsInArea--;
            if (wrongItemsInArea < 0) wrongItemsInArea = 0;
        }
        UpdateColor();
    }
    private void UpdateColor()
    {
        if (rightItemInArea)
        {
            GetComponentInParent<MeshRenderer>().material = materials[0];
        } else
        {
            if (wrongItemsInArea > 0)
            {
                GetComponentInParent<MeshRenderer>().material = materials[1];
            }
            else
            {
                GetComponentInParent<MeshRenderer>().material = materials[2];
            }
        }
    }
}
