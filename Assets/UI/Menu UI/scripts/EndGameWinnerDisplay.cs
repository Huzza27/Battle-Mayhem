using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameWinnerDisplay : MonoBehaviour
{
    public Image winnerDis;

    public Sprite[] colors;

    private void Start()
    {
        winnerDis = GetComponent<Image>();
        Player winner = (Player)PhotonNetwork.CurrentRoom.CustomProperties["Winner"];
        winnerDis.sprite = colors[(int)winner.CustomProperties["PlayerColor"]];
    }
}
