using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public GameObject[] items;
    public GameObject[] spawnAreas;
    public GameObject[] solutionAreas;
    public GameObject[] exampleAreas;

    int[] spawnArray;
    // Start is called before the first frame update
    public void Generate()
    {
        spawnAreas = GameObject.FindGameObjectsWithTag("SpawnArea");
        System.Random r = new System.Random();
        int[] solutionArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        spawnArray = new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        solutionArray = solutionArray.OrderBy(x => r.Next()).ToArray();
        spawnArray = spawnArray.OrderBy(x => r.Next()).ToArray();

        int j = 0;
        foreach (int i in solutionArray)
        {
            //Give random solutionindex to item, and a matching solutionindex to the solutionarea in question
            items[j].GetComponent<Item>().solutionIndex = i;
            solutionAreas[i].GetComponent<SolutionArea>().solutionIndex = i;

            //Instantiate item at matching example and destroy grabbable component
            GameObject exampleItem = Instantiate(items[j], exampleAreas[i].transform.position, Quaternion.identity);
            Destroy(exampleItem.GetComponent<OVRGrabbable>());

            //Instantiate grabbable item at a random spawn point
            GameObject spawnItem = Instantiate(items[j], spawnAreas[spawnArray[j]].transform.position, Quaternion.identity);
            spawnItem.GetComponent<Item>().spawnPoint = j;
            j++;
        }
    }
    public void RespawnItems(List<GameObject> items)
    {
        foreach (GameObject item in items)
        {
            GameObject spawnItem = Instantiate(item, spawnAreas[spawnArray[item.GetComponent<Item>().spawnPoint]].transform.position, Quaternion.identity);
            spawnItem.SetActive(true);
        }
        items.Clear();
    }
}
