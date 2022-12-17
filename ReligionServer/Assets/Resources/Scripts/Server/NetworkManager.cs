using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject projectilePrefab;

    public GameObject MainLocation;

    public GameObject PhycicsProcess;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Physics.IgnoreLayerCollision(6, 6);

        Server.Start(50, 26950);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Close debug session. close connections!");
        Server.Stop();
        Debug.Log("Server ends.");
    }

    public Player InstantiatePlayer(Vector3 position)
    {
        Player player = Instantiate(playerPrefab, position, Quaternion.identity).GetComponent<Player>();
        return player;
    }

    public void InstantiateEnemy(Vector3 _position)
    {
        Instantiate(enemyPrefab, _position, Quaternion.identity);
    }

    public Projectile InstantiateProjectile(Transform _shootOrigin)
    {
        return Instantiate(projectilePrefab, _shootOrigin.position + _shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Projectile>();
    }
}
