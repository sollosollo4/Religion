using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class TouchableStructures : MonoBehaviour
{
    [SerializeField] public string TouchEventName;
    [SerializeField] public Motion TouchMotion;
    [SerializeField] public GameObject TouchTool;

    [SerializeField] public Material NormalMaterial;
    [SerializeField] public Material HightLightMaterial;

    private bool isHightlated;

    private void Start()
    {
        NormalMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHightlated)
        {
            GetComponent<Renderer>().material = HightLightMaterial;
        }
        else
        {
            GetComponent<Renderer>().material = NormalMaterial;
        }
    }

    public void setNormalLightActive()
    {
        //isHightlated = true;
        GetComponent<GlowObjectCmd>().SetActive(false);
    }

    public void setHightLightActive()
    {
        //isHightlated = false;
        GetComponent<GlowObjectCmd>().SetActive(true);
    }
}
