using JetBrains.Annotations;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerUpdate : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playersCountText; // Assign this in the inspector with your UI Text

    public GameObject blueImage; 
    public GameObject redImage;

    public Transform p1BannerLoc;

    public  GameObject p1Buttons;

    public GameObject p2Buttons;
    public TMP_InputField livesInput;
    private int lives;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        UpdatePlayerCount();
        AssignImages();
    }

    public override void OnJoinedRoom()
    {
        UpdatePlayerCount();
        AssignImages();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();
        AssignImages();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();
    }

    void UpdatePlayerCount()
    {
        // Assuming you're inside a room, update the text element to show the current player count.
        // PhotonNetwork.CurrentRoom will give you the current room you're in
        if (PhotonNetwork.CurrentRoom != null)
        {
            playersCountText.text = "Players in Lobby: " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
        }
    }
    void AssignImages()
    {
        // Check if you're the first player in the room
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            // As Player 1, set your image to blue and position it on the left
            Debug.Log("You are player number " + PhotonNetwork.LocalPlayer.ActorNumber.ToString());
            blueImage.SetActive(true);
            p2Buttons.SetActive(false);
            redImage.SetActive(false);
            // Optionally adjust positions if needed

        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            // As Player 2, set your image to red and position it on the left
            redImage.SetActive(true);

            blueImage.SetActive(false);
            p1Buttons.SetActive(false);
            // Optionally adjust positions if needed
            swapBannerPositions();
        }

        // If there are two players, ensure both images are active for both players
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            blueImage.SetActive(true);
            redImage.SetActive(true);

            // Adjust positions based on the player's actor number
            if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
            {
                // Optionally adjust positions to ensure the local player's image is on the left
            }
            else
            {
                // Optionally adjust positions to ensure the local player's image is on the left
            }
        }

        void SetLives()
        {
            // Attempt to parse the text as an integer
            if (int.TryParse(livesInput.text, out int result))
            {
                lives = result;
                if(PhotonNetwork.IsMasterClient)
                {

                }
            }
            else
            {
                Debug.LogError("Input is not a valid integer");
            }
        }

        void swapBannerPositions()
        {
            blueImage.transform.position = redImage.transform.position;
            redImage.transform.position = p1BannerLoc.position;
        }

    }

    public void LoadGameScene()
    {
        if (PhotonNetwork.IsMasterClient) // Only the Master Client can initiate the scene change
        {
            PhotonNetwork.LoadLevel("Game");
        }
    }

}
