using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchableStructures : MonoBehaviour
{
    public uint id;
    public ushort PrefabId;
    [SerializeField] public string TouchEventName;
    [SerializeField] public Motion TouchMotion;
    [SerializeField] public GameObject TouchTool;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
