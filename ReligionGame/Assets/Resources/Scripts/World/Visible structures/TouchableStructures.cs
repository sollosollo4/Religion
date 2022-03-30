using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class TouchableStructures : MonoBehaviour
{
    [SerializeField] public string TouchEventName;
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
        isHightlated = false;
        GetComponent<GlowObjectCmd>().SetActive(false);
        PlayerPanelTool.gameObject.SetActive(false);
        GameManager.players[Client.instance.myId].SetWorkName("");
        GameManager.players[Client.instance.myId].lastTouchableStructure = null;
    }

    public void setHightLightActive()
    {
        isHightlated = true;
        GetComponent<GlowObjectCmd>().SetActive(true);
        PlayerPanelTool.gameObject.SetActive(true);
        GameManager.players[Client.instance.myId].SetWorkName(TouchEventName);
        GameManager.players[Client.instance.myId].lastTouchableStructure = this;
    }

    public void StartMining()
    {
        if(GameManager.players[Client.instance.myId].CurrentWorkTool == null)
            GameManager.players[Client.instance.myId].CurrentWorkTool = Instantiate(TouchTool, GameManager.players[Client.instance.myId].ToolHand.transform);

        ClientSend.PlayerUseTool(GetComponent<SpawnedGameObject>().spawnedObjectId);
    }

    public void StartMining(int _byPlayer)
    {
        if (GameManager.players[_byPlayer].CurrentWorkTool == null)
            GameManager.players[_byPlayer].CurrentWorkTool = Instantiate(TouchTool, GameManager.players[_byPlayer].ToolHand.transform);
        
        GameManager.players[_byPlayer].SetWorkName(TouchEventName);
        GameManager.players[_byPlayer].lastTouchableStructure = this;

        ClientSend.PlayerUseTool(GetComponent<SpawnedGameObject>().spawnedObjectId);
    }

    public void EndMining()
    {
        Destroy(GameManager.players[Client.instance.myId].CurrentWorkTool);

    }

    public void EndMining(int _byPlayer)
    {
        Destroy(GameManager.players[_byPlayer].CurrentWorkTool);
    }

    public bool PlayerRaycast(Camera playerCamera)
    {
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit _hit, 30f))
        {
            if (_hit.collider.TryGetComponent(out SpawnedGameObject _spawnedObject))
            {
                return _spawnedObject.spawnedObjectId == GetComponent<SpawnedGameObject>().spawnedObjectId;
            }
            else 
                return false;
        }
        else
            return false;
    }

    public void setHightLight(bool touch)
    {
        if (touch)
            setHightLightActive();
        else
            setNormalLightActive();
    }
}
