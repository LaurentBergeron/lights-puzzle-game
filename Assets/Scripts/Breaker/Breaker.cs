﻿using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using UnityEngine;
 
public abstract class Breaker : MonoBehaviour
{
    public Transform[] lightTransforms;
    public bool isOpened = false;

    protected List<Light> associatedLights = new List<Light>();

    private MinimumDistance minimumDistance;
    private Transform arm;
    private AudioSource audioSource;
    private Vector3 closedAngle = new Vector3(-30, 0, 0);
    private Vector3 openedAngle = new Vector3(30, 0, 0);


    // Use this for initialization
    void Start ()
    {
        // Extract Light components from provided Light transforms
        foreach (Transform lightTransform in lightTransforms)
        {
            Light light = lightTransform.Find("Rotating").Find("Light").gameObject.GetComponent<Light>();
            associatedLights.Add(light);
        }
        // Reach for important components in this game object
        minimumDistance = gameObject.GetComponent<MinimumDistance>();
        audioSource = transform.Find("Body").Find("Sound").gameObject.GetComponent<AudioSource>();
        arm = transform.Find("Body").Find("ArmWrapper");
        // Starting position for the breaker
        if (isOpened)
        {
            Open();
        }
        else
        {
            Close();
        }
    }
	
    void OnMouseUp()
    {
        // If the minimum distance requirement wasn't met, do nothing
        if (!minimumDistance.Check())
        {
            return;
        }

        if (isOpened)
        {
            Close();
        }
        else
        {
            Open();
        }
        audioSource.Play();
        isOpened = !isOpened;
    }

    void Close()
    {
        arm.localEulerAngles = closedAngle;
        CloseSpecific();
    }

    void Open()
    {
        arm.localEulerAngles = openedAngle;
        OpenSpecific();
    }


    public abstract void CloseSpecific();
    public  abstract void OpenSpecific();

}
