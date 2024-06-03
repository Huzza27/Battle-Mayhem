using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrateCollision : MonoBehaviour
{

    public CrateFunctionality cf;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && cf.hasLanded)
        {
            cf.itemIndex = Random.Range(1, cf.spawner.items.Length + 1);
            Debug.Log("Player Detected");
            // Debug message to console
            Debug.Log("Swapping to item " + cf.itemIndex);
            collision.gameObject.GetComponent<PhotonView>().RPC("SwapItems", RpcTarget.AllBuffered, cf.itemIndex);

            // Call a function to handle the destruction across the network
            cf.spawner.canSpawn = true;
            cf.DestroyCrateNetworked();
            return;
        }
    }
}
