using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class TouchableStructures : MonoBehaviour
{
    [SerializeField] public string TouchEventName;
    [SerializeField] public Motion TouchMotion;
    [SerializeField] public GameObject TouchTool;

    GameObject PlayerPanelTool;

    private bool isHightlated;

    private void Start()
    {
        PlayerPanelTool = UIManager.instance.toolPanel;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setNormalLightActive()
    {
        //isHightlated = true;
        GetComponent<GlowObjectCmd>().SetActive(false);
        PlayerPanelTool.gameObject.SetActive(false);
    }

    public void setHightLightActive()
    {
        //isHightlated = false;
        GetComponent<GlowObjectCmd>().SetActive(true);
        PlayerPanelTool.gameObject.SetActive(true);
    }
}
