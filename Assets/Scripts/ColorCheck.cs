﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using System.Linq;
using UnityEngine;

public class ColorCheck : MonoBehaviour
{
    public RenderTexture colorCheckTexture;
    public Color groundColor;
    public PowerColor powerColor;
    public Vector3 shadowForce = new Vector3(0, 0, 0);

    private Texture2D tex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    double euclidianDistance(Color color1, Color color2)
    {
        double rDiff = color1.r - color2.r;
        double gDiff = color1.g - color2.g;
        double bDiff = color1.b - color2.b;
        // https://stackoverflow.com/questions/1847092/given-an-rgb-value-what-would-be-the-best-way-to-find-the-closest-match-in-the-d
        // Future improvement: use weighted approach?
        return Math.Sqrt((rDiff * rDiff) + (gDiff * gDiff) + (bDiff * bDiff));
    }

    // Update is called once per frame
    void Update()
    {
        // Remember currently active render texture
        RenderTexture currentActiveRT = RenderTexture.active;

        // Set the supplied RenderTexture as the active one
        RenderTexture.active = colorCheckTexture;

        // Create a new Texture2D and read the RenderTexture image into it
        tex = new Texture2D(colorCheckTexture.width, colorCheckTexture.height);
        tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);

        // Restore previously active render texture
        RenderTexture.active = currentActiveRT;

        // Read Color Pixel from texture 2D
        Color[] pixels = tex.GetPixels(0, 0, tex.width, tex.height);
        float[,] invertedGrayscale = new float[tex.width, tex.height];

        float forceMagnitude = 0;
        for (int i=0; i< tex.width; i++)
        {
            for (int j = 0; j < tex.height; j++)
            {
                invertedGrayscale[i, j] = 1 - pixels[tex.height * i + j].grayscale;
                forceMagnitude += invertedGrayscale[i, j] / (tex.width * tex.height);
            }
        }

        shadowForce = Vector3.up * forceMagnitude;

        groundColor = tex.GetPixel(tex.width/2, tex.height/2);


        Color[] colorArray = { 
            Color.white, 
            Color.black,
            Color.red,
            Color.green,
            Color.blue,
            Color.cyan,
            Color.magenta,
            Color.yellow
        };
        double[] distanceArray = new double[8];


        for (int i=0; i<8; i++)
        {
            distanceArray[i] = euclidianDistance(groundColor, colorArray[i]);
        }

        powerColor = (PowerColor) Array.IndexOf(distanceArray, distanceArray.Min());
    }
}