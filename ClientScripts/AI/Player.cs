using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Player : MonoBehaviour
{
    public float speed = 10;
    private Vector3 lastPosition = new Vector3(0, 1, 0);
    private float posOffset = 0.1f;
    public string Username { get; set; }

    private SyncPositionRequest syncPositionRequest;
    private SyncPlayerRequest syncPlayerRequest;
    public GameObject playerPrefab;
    private GameObject player;

    private Dictionary<string, GameObject> playerDict = new Dictionary<string, GameObject>();

	void Start () 
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<MeshRenderer>().material.color = Color.green;
        syncPositionRequest = GetComponent<SyncPositionRequest>();
        syncPlayerRequest = GetComponent<SyncPlayerRequest>();
        //向服务器发送同步玩家请求
        syncPlayerRequest.OnSendRequest();
        //每秒调用20次位置同步
        InvokeRepeating("SyncPosition", 2, 0.05f);
	}

    private void SyncPosition()
    {
        //只有当前位置与上次位置的距离大于0.1f时才做位置同步（为了节省性能）
        if (Vector3.Distance(player.transform.position, lastPosition) < posOffset) return;
        lastPosition = player.transform.position;
        syncPositionRequest.Pos = player.transform.position;
        //向服务器发送同步位置请求
        syncPositionRequest.OnSendRequest();
    }

    public void OnSyncPlayerResponse(List<string> usernameList)
    {
        foreach (string username in usernameList)
        {
            OnSyncPlayerEvent(username);
        }
    }

    public void OnSyncPlayerEvent(string username)
    {
        GameObject playerGo = Instantiate(playerPrefab);
        playerDict.Add(username, playerGo);
    }

    public void OnSyncPositionEvent(List<PlayerData> playerDataList)
    {
        foreach (PlayerData playerdata in playerDataList)
        {
            GameObject go = DictionaryHelper.GetValue<string, GameObject>(playerDict, playerdata.Username);
            if (go != null) 
                go.transform.position = new Vector3() { x = playerdata.Pos.x, y = playerdata.Pos.y, z = playerdata.Pos.z };
        }
    }

	void Update () 
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        player.transform.Translate(hor * Time.deltaTime * speed, 0, ver * Time.deltaTime * speed);
	}
}
