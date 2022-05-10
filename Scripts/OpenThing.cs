using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenThing : MonoBehaviour
{
    GameObject entrance;
    HingeJoint entranceHinge;
    GameObject grabbableHandle;
    JointSpring openSpring;
    private void Start()
    {
        entrance = GameObject.FindGameObjectWithTag("Entrance");
        entranceHinge = entrance.GetComponent<HingeJoint>();
        grabbableHandle = entrance.GetComponentInChildren<DoorGrabbable>().gameObject;
        openSpring.targetPosition = 160;
        openSpring.spring = 1;
        openSpring.damper = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.doorClosed)
        {
            GameManager.instance.doorClosed = false;
            GameManager.instance.TryAgain();
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        entranceHinge.spring = openSpring;
        grabbableHandle.layer = 11;
        //entrance.layer = 11;
        yield return new WaitForSeconds(1);
        entranceHinge.useSpring = false;
    }
}
