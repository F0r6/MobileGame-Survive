using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Paralax for infinite tilemap
public class Paralax : MonoBehaviour
{
    //Declare variables
    [SerializeField] private float length, startposX, startposY;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;

    void Start()
    {
        //Set variables
        startposX = transform.position.x;
        startposY = transform.position.y;
    }

    void Update()
    {
        //Calculate parallax effect
        float tempX = (cam.transform.position.x * (1 - parallaxEffect));
        float tempY = (cam.transform.position.y * (1 - parallaxEffect));
        float distX = (cam.transform.position.x * parallaxEffect);
        float distY = (cam.transform.position.y * parallaxEffect);

        //Move background
        transform.position = new Vector3(startposX + distX, startposY + distY, transform.position.z);

        //Loop background X AXIS
        if (tempX > startposX + length) startposX += length;
        else if (tempX < startposX - length) startposX -= length;

        //Loop background Y AXIS
        if (tempY > startposY + length) startposY += length;
        else if (tempY < startposY - length) startposY -= length;
    }
}