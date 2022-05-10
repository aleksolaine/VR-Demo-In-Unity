using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryDetector : MonoBehaviour
{
    GameObject entrance;
    HingeJoint entranceHinge;
    GameObject grabbableHandle;
    JointSpring closeSpring;
    private void Start()
    {
        entrance = GameObject.FindGameObjectWithTag("Entrance");
        entranceHinge = entrance.GetComponent<HingeJoint>();
        grabbableHandle = entrance.GetComponentInChildren<DoorGrabbable>().gameObject;
        closeSpring.targetPosition = 0;
        closeSpring.spring = 999;
        closeSpring.damper = 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.instance.doorClosed)
        {
            GameManager.instance.doorClosed = true;
            StartCoroutine(LockDoor());
        }
    }

    IEnumerator LockDoor()
    {
        entranceHinge.spring = closeSpring;
        entranceHinge.useSpring = true;
        Rigidbody rb = entrance.GetComponent<Rigidbody>();
        while (rb.angularVelocity.y > 0.01f)
        {
            yield return new WaitForSeconds(.2f);
        }
        grabbableHandle.layer = 10;
        entrance.layer = 10;
    }
}
