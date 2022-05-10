using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public int maxLength = 10;
    public GameObject cursor;
    public LayerMask layerMask;

    LineRenderer laser;
    OVRPlayerController playerController;
    bool axisInUse = false;
    GameObject handheldUI;
    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<LineRenderer>();
        playerController = GetComponentInParent<OVRPlayerController>();
        cursor.GetComponent<MeshRenderer>().material.color = Color.gray;
        handheldUI = GetComponentInChildren<Canvas>().gameObject;
        handheldUI.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            handheldUI.SetActive(!handheldUI.activeSelf);
        }
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxLength, layerMask))
        {
            Vector3[] line = new Vector3[2];

            line[0] = transform.position;
            line[1] = line[0] + gameObject.transform.forward * hit.distance;

            cursor.SetActive(true);
            laser.enabled = true;
            laser.SetPositions(line);
            cursor.transform.position = line[1];
            //cursor.GetComponent<MeshRenderer>().material.color = Color.gray;
            laser.startColor = Color.green;
            if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > .8f)
            {
                if (!axisInUse)
                {
                    axisInUse = true;
                    UIButton button = hit.collider.GetComponent<UIButton>();
                    StartCoroutine(CursorFlash());
                    if (button.ID == 0)
                    {
                        playerController.SnapRotation = !playerController.SnapRotation;
                        if (playerController.SnapRotation)
                        {
                            hit.collider.GetComponentInChildren<Text>().text = "Smooth Rotation: OFF";
                            button.GetComponent<MeshRenderer>().material.color = Color.gray;
                            button.transform.localPosition = button.transform.localPosition - Vector3.forward * 0.8f;
                        }
                        else
                        {
                            hit.collider.GetComponentInChildren<Text>().text = "Smooth Rotation: ON";
                            button.GetComponent<MeshRenderer>().material.color = Color.green;
                            button.transform.localPosition = button.transform.localPosition + Vector3.forward * 0.8f;
                        }
                    }
                    else if (button.ID == 1)
                    {
                        GameManager.instance.ResetLevel();
                        StartCoroutine(ButtonPress(button));
                    }
                    else if (button.ID == 2)
                    {
                        playerController.EnableLinearMovement = !playerController.EnableLinearMovement;
                        if (playerController.EnableLinearMovement)
                        {
                            hit.collider.GetComponentInChildren<Text>().text = "Smooth Movement: ON";
                            button.GetComponent<MeshRenderer>().material.color = Color.green;
                            button.transform.localPosition = button.transform.localPosition + Vector3.forward * 0.8f;
                        }
                        else
                        {
                            hit.collider.GetComponentInChildren<Text>().text = "Smooth Movement: OFF";
                            button.GetComponent<MeshRenderer>().material.color = Color.gray;
                            button.transform.localPosition = button.transform.localPosition - Vector3.forward * 0.8f;
                        }
                    }
                    else if (button.ID == 3)
                    {
#if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
#else
                            Application.Quit();
#endif
                    }
                }
            }
            else
            {
                axisInUse = false;
            }
        }
        else
        {
            cursor.SetActive(false);
            laser.enabled = false;
        }
    }

    IEnumerator ButtonPress(UIButton button)
    {
        button.GetComponent<MeshRenderer>().material.color = Color.gray;
        button.transform.localPosition = button.transform.localPosition + Vector3.forward * 0.8f;
        yield return new WaitForSeconds(.2f);
        button.GetComponent<MeshRenderer>().material.color = Color.red;
        button.transform.localPosition = button.transform.localPosition - Vector3.forward * 0.8f;
        
    }
    IEnumerator CursorFlash()
    {
        cursor.GetComponent<MeshRenderer>().material.color = Color.green;
        yield return new WaitForSeconds(.2f);
        cursor.GetComponent<MeshRenderer>().material.color = Color.gray;
    }
}

