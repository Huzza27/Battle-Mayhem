using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviour
{
    public int StartingLives;
    public TMP_InputField livesInputField;
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent the manager from being destroyed on load
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
    public void SetLives()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (int.TryParse(livesInputField.text, out int result))
            {
                // Set a room property
                Hashtable props = new Hashtable
            {
                {"StartingLives", result}
            };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
            else
            {
                Debug.LogError("Input is not a valid integer");
            }
        }
    }


    public void SetPlayerColorChoice(int choice)
    {
        if (PhotonNetwork.LocalPlayer != null)
        {
            Hashtable props = new Hashtable { { "PlayerColor", choice } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            //Debug.Log("Set PlayerColor to " + choice);
        }
        else
        {
            Debug.LogError("PhotonNetwork.LocalPlayer is null");
        }
    }

}


