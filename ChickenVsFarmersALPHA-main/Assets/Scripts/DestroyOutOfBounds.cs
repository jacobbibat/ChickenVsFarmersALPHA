using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private Transform mainCameraTransform;
    public float maxDistanceFromCamera = 45f; // Set the distance limit

    void Start()
    {
        // Cache the Main Camera's transform
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Calculate the distance between this object and the Main Camera
        float distanceFromCamera = Vector3.Distance(transform.position, mainCameraTransform.position);

        // Destroy the object if it goes beyond the maximum distance
        if (distanceFromCamera > maxDistanceFromCamera)
        {
            Destroy(gameObject);
        }
    }
}