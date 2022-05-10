using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkenScreen : MonoBehaviour
{
    OVRScreenFade screenFade;
    Coroutine fadeRoutine;
    int collidedWalls = 0;
    bool fade = false;
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
        screenFade = GetComponent<OVRScreenFade>();
    }
    private void Update()
    {
        if (GameManager.instance.doorClosed)
        {
            if (!fade)
            {
                if (transform.position.z < -9.8f)
                {
                    fade = true;
                    StartCoroutine(screenFade.Fade(0, 1, .1f));
                }
            }
            else
            {
                if (transform.position.z > -9.8f)
                {
                    fade = false;
                    StartCoroutine(screenFade.Fade(1, 0, .1f));
                }
            }
        } else
        {
            if (transform.position.z > -9f)
            {
                GameManager.instance.doorClosed = true;
                LockDoor();
            }
        }
    }
    void LockDoor()
    {
        entranceHinge.spring = closeSpring;
        entranceHinge.useSpring = true;
        //Rigidbody rb = entrance.GetComponent<Rigidbody>();
        //while (rb.angularVelocity.y > 0.01f)
        //{
        //    yield return new WaitForSeconds(.2f);
        //}
        grabbableHandle.layer = 8;
        //entrance.layer = 10;
    }
}
