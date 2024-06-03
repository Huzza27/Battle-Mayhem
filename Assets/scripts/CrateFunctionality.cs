using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;

public class CrateFunctionality : MonoBehaviour 
{
    [SerializeField] public SpawnCrate spawner;
    public int itemIndex;
    public bool hasLanded = false;
    // This function is called when this GameObject collides with another GameObject
    private void Update()
    {
        if(transform.position.y < -13)
        {
            DestroyCrateNetworked();
        }
    }
private void OnTriggerEnter2D(Collider2D collision)
{

        if(collision.gameObject.CompareTag("Ground") && !hasLanded)
        {
            hasLanded = true;
            this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;  // Stops the velocity
            this.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;       // Makes the object not be affected by physics
            return;
        }

        if (collision.gameObject.CompareTag("Player") && hasLanded)
        {
            itemIndex = Random.Range(1, spawner.items.Length + 1);
            Debug.Log("Player Detected");
            // Debug message to console
            if (collision.gameObject.GetComponent<PhotonView>() != null)
            {
                Debug.Log("Swapping to item " + itemIndex);
                collision.gameObject.GetComponent<PhotonView>().RPC("SwapItems", RpcTarget.AllBuffered, itemIndex);
            }

            // Call a function to handle the destruction across the network
            spawner.canSpawn = true;
            DestroyCrateNetworked();
            return;
        }
    }
        
    public void DestroyCrateNetworked()
    {
        if (this.gameObject != null)
        {
            this.GetComponent<PhotonView>().RPC("DestroyCrateOnAllClients", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    void DestroyCrateOnAllClients()
    {
        // Destroy this crate
        spawner.StartCoroutine("crateSpawnTimer");
        PhotonNetwork.Destroy(gameObject);
    }

}

