using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition = null;

    public float Euler = 10f;

    private void Start()
    {
        if (cameraPosition != null)
        {
            transform.position = cameraPosition.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraPosition != null)
        {
            transform.position = Vector3.Lerp(transform.position, cameraPosition.position, Time.deltaTime*Euler);
        }
    }
}
