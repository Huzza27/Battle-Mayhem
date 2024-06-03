using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PickColor : MonoBehaviour
{
    public Image container;
    public Image playerDis;

    public bool isShowing = false;

    public Sprite[] colors;

    public PhotonView view;
    public void ToggleColorContainer()
    {
        if (isShowing == false)
        {
            container.gameObject.SetActive(true);
            isShowing = true;
        }

        else
        {
            container.gameObject.SetActive(false);
            isShowing = false;
        }
    }
    public void SetColor(int index)
    {
        view.RPC("ChangeColor", RpcTarget.All, index);
    }

    [PunRPC]
    public void ChangeColor(int index)
    {
        playerDis.sprite = colors[index];
    }


    
}
