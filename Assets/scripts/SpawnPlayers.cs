using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class SpawnPlayers : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public GameObject cameraPrefab;

    public GameObject[] healthBarList;

    public Transform canvas;
    public Transform camSpawn;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    GameObject player;

    //Quaternion rotation;
    public static int playerCount = 0;
    // Static dictionary to track player GameObjects
    private void Start()
    {
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        player = PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        playerCount++;
        //Debug.Log("Player instantiated");

        if (player.GetComponent<PhotonView>().IsMine)
        {
            GameObject playerCam = PhotonNetwork.Instantiate(cameraPrefab.name, camSpawn.position, Quaternion.identity);
            CameraMove cameraMoveScript = playerCam.GetComponent<CameraMove>();
            cameraMoveScript.player = player.transform;
            //Debug.Log("Camera setup for local player");
            //Debug.Log("Turnign on health bar for player " + playerCount);
            player.GetComponent<PhotonView>().RPC("AssignHealthBar", RpcTarget.AllBuffered, player.GetComponent<PhotonView>().ViewID, player.GetComponent<PhotonView>().Owner.ActorNumber);


            if (playerCount > 1)
            {
                SwapHealthBarPositions();
            }
        }
    }
    public void SwapHealthBarPositions()
    {
        Transform currentPos;
        switch(playerCount)
        {
            case 2:
                currentPos = healthBarList[playerCount - 1].transform;
                healthBarList[playerCount - 1].transform.position = healthBarList[playerCount - 2].transform.position;
                healthBarList[playerCount - 2].transform.position = currentPos.position;
                break;
            case 3:
                currentPos = healthBarList[playerCount - 2].transform;
                healthBarList[playerCount - 2].transform.position = healthBarList[playerCount - 2].transform.position;
                healthBarList[playerCount - 3].transform.position = currentPos.position;
                break;
            case 4:
                currentPos = healthBarList[playerCount - 3].transform;
                healthBarList[playerCount - 3].transform.position = healthBarList[playerCount - 2].transform.position;
                healthBarList[playerCount - 4].transform.position = currentPos.position;
                break;
        }
    }
}

