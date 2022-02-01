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

        ClientSend.PlayerUseTool(GetComponent<SpawnedGameObject>().spawnedObjectId, PlayerManager.GetAnimationStateName(TouchEventName));

        // ServerSend.
        // здесь мы отправляем инфу, что начали копать
        // НА СЕРВЕРЕ: запускаем курутину на MiningTime у этого объекта
        // на сервере
    }

    public void EndMining()
    {
        Destroy(GameManager.players[Client.instance.myId].CurrentWorkTool);
    }
}
