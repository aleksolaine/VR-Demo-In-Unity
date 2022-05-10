using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorGrabbable : OVRGrabbable
{
    public Transform handler;
    private Rigidbody rbHandler;

    protected override void Start()
    {
        base.Start();
        rbHandler = handler.GetComponent<Rigidbody>();
    }

    public override void GrabBegin(OVRGrabber hand, Collider grabPoint)
    {
        base.GrabBegin(hand, grabPoint);

        //rbHandler.drag = 0;
        //rbHandler.angularDrag = 0;
        rbHandler.mass = 1;
    }

    public override void GrabEnd(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        base.GrabEnd(Vector3.zero, Vector3.zero);

        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;

        //rbHandler.velocity = Vector3.zero;
        //rbHandler.angularVelocity = Vector3.zero;
        //rbHandler.drag = 1;
        //rbHandler.angularDrag = 1;
        rbHandler.mass = 0.01f;
    }

    private void Update()
    {
        if (Vector3.Distance(handler.position, transform.position) > 0.4f)
        {
            grabbedBy.ForceRelease(this);
        }
    }
}