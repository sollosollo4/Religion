using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourObject : MonoBehaviour
{
    public bool IsMovable;
    public bool IsRotateble;

    public float MoveSpeed = 3f;
    public float RotationSpeed = 3f;

    public GameObject targetOne;
    public GameObject targetTwo;

    public bool MoveTo;

    // Start is called before the first frame update
    void Start()
    {
        MoveTo = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(IsMovable)
        {
            if (MoveTo)
            {
                GetComponent<Transform>().position = Vector3.MoveTowards(transform.position, targetOne.transform.position, MoveSpeed * Time.deltaTime);
            }
            else
            {
                GetComponent<Transform>().position = Vector3.MoveTowards(transform.position, targetTwo.transform.position, MoveSpeed * Time.deltaTime);
            }
            if (GetComponent<Transform>().position == targetOne.transform.position)
                MoveTo = false;
            
            if(GetComponent<Transform>().position == targetTwo.transform.position)
                MoveTo = true;
        }

        if(IsRotateble)
        {
            transform.Rotate(Vector3.right * (RotationSpeed * Time.deltaTime));
        }

        ServerSend.ParkourObjectData(gameObject.name, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
    }
}
