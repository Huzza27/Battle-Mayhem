using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public PhotonView view;
    public GameObject hand;
    Item gunData;
    new SpriteRenderer renderer;
    public GunMechanicManager gunManager;
    public GameObject gunTip;
    public int startingGunIndex = 4;
    public GameObject bodyObject;
    public Sprite[] colors;
    public BoxCollider2D collider;
    // Start is called before the first frame update
    void Awake()
    {
        gunData = gunManager.items[startingGunIndex];
        renderer = hand.GetComponent<SpriteRenderer>();
        renderer.sprite = gunData.icon;
        AdjustGunTipPosition(gunData.gunTipYOffset, gunData);
        if (view.IsMine)
        {
            view.RPC("SetPlayerColorForAllClients", RpcTarget.All);
        }
    }

    public void AdjustGunTipPosition(float yOffset, Item heldItem)
    {
        // Calculate offset
        // Assuming the gun tip is at the right edge of the sprite
        float xOffset;


        if (hand.GetComponent<SpriteRenderer>() != null)
        {
            xOffset = hand.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;
            gunTip.transform.localPosition = new Vector2(xOffset, yOffset);
            MoveGunCollider(heldItem);
            
        }
        else
        {
            return;
        }
    }

    public void MoveGunCollider(Item heldItem)
    {
        collider = view.gameObject.transform.GetChild(7).GetComponent<BoxCollider2D>();
        collider.offset = new Vector2(heldItem.GuncolliderOffsetX, heldItem.GuncolliderOffsetY);
        collider.size = new Vector2(heldItem.GunColliderSizeX, heldItem.GunColliderSizeY);


    }

    public Vector3 GetGunTipPosition()
    {
        return gunTip.transform.position;
    }

    [PunRPC]
    public void SetPlayerColorForAllClients()
    {
        Player localPlayer = PhotonNetwork.LocalPlayer;
        object colorChoice;
        if (localPlayer.CustomProperties.TryGetValue("PlayerColor", out colorChoice))
        {
            int colorIndex = (int)colorChoice;
            bodyObject.GetComponent<SpriteRenderer>().sprite = colors[(int)colorChoice];
        }
    }
}
