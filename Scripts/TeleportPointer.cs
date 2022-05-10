using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPointer : MonoBehaviour
{
    public int maxLength = 10;
    public GameObject teleportArea;
    public LayerMask layerMask;

    LineRenderer laser;
    CharacterController characterController;
    OVRPlayerController playerController;
    bool axisInUse = false;
    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        characterController = GetComponentInParent<CharacterController>();
        playerController = GetComponentInParent<OVRPlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!playerController.EnableLinearMovement && OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != Vector2.zero)
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxLength, layerMask))
            {
                Vector3[] line = new Vector3[2];
                laser.enabled = true;

                line[0] = transform.position;
                line[1] = line[0] + gameObject.transform.forward * hit.distance;

                laser.SetPositions(line);
                if (hit.collider.gameObject.layer == 12)
                {
                    teleportArea.SetActive(true);
                    teleportArea.transform.position = hit.point;
                    Vector2 inputRotation = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).normalized;
                    Quaternion rotationDelta = Quaternion.FromToRotation(Vector3.left, new Vector3(transform.forward.x, 0, transform.forward.z));
                    teleportArea.transform.rotation = rotationDelta * Quaternion.Euler(0f, Mathf.Rad2Deg * Mathf.Atan2(inputRotation.y, -inputRotation.x), 0f);
                    laser.startColor = Color.green;
                    //laser.endColor = Color.clear;
                    if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > .8f)
                    {
                        if (!axisInUse)
                        {
                            axisInUse = true;
                            characterController.enabled = false;
                            characterController.transform.position = line[1] + Vector3.up;
                            //characterController.transform.rotation = Quaternion.Euler(-teleportArea.transform.rotation.x, 0, -teleportArea.transform.rotation.z);
                            characterController.transform.Rotate(Vector3.up, Mathf.Rad2Deg * Mathf.Atan2(inputRotation.y, -inputRotation.x) - 90);
                            characterController.enabled = true;
                        }
                    }
                    else
                    {
                        axisInUse = false;
                    }
                }
                else
                {
                    laser.startColor = Color.red;
                    teleportArea.SetActive(false);
                }
            }
            else
            {
                laser.enabled = false;
                teleportArea.SetActive(false);
            }
        } 
        else
        {
            laser.enabled = false;
            teleportArea.SetActive(false);
        }
    }
}
