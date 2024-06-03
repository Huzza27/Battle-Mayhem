using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    PhotonView view;

    GameObject player;
    public Image icon;
    public CapsuleCollider2D bc;
    public CapsuleCollider2D bc2;
    public SpriteRenderer sr_body, sr_arms, sr_legs;
    public Rigidbody2D rb;
    public SpriteRenderer gun;
    private Transform respawnArea;
    public float healthAmount;
    [SerializeField] public Image fillImage;
    public Sprite[] coloredIcons;

    public TMP_Text livesDisplay;
    int lives;

    public bool isDead;
    private void Start()
    {
        isDead = false;
        view = GetComponent<PhotonView>();
        respawnArea = GameObject.FindGameObjectWithTag("RespawnArea").transform;
        lives = (int)PhotonNetwork.CurrentRoom.CustomProperties["StartingLives"];
        livesDisplay.text = lives.ToString();
    }

    private void Update()
    {
        if (isDead == false)
        {
            CheckHealth();
        }
    }

    private void CheckHealth()
    {
        fillImage.fillAmount = healthAmount / 100;
        if (view.IsMine && healthAmount <= 0)
        {
            this.GetComponent<PhotonView>().RPC("Dead", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void AssignHealthBar(int playerViewID, int playerCount)
    {
        PhotonView targetPhotonView = PhotonView.Find(playerViewID);
        SpawnPlayers spawnManager = GameObject.FindGameObjectWithTag("SpawnPlayer").GetComponent<SpawnPlayers>();
        if (targetPhotonView != null)
        {
            player = targetPhotonView.gameObject;
            spawnManager.healthBarList[playerCount - 1].SetActive(true);
            player.GetComponent<Health>().fillImage = spawnManager.healthBarList[playerCount - 1].transform.GetChild(1).GetComponent<Image>();
            icon = spawnManager.healthBarList[playerCount - 1].transform.GetChild(2).GetComponent<Image>();

            object colorChoice;
            if (targetPhotonView.Owner.CustomProperties.TryGetValue("PlayerColor", out colorChoice))
            {
                int colorIndex = (int)colorChoice;
                targetPhotonView.RPC("SetUIColor", RpcTarget.All, colorIndex);
            }
            livesDisplay = spawnManager.healthBarList[playerCount - 1].transform.GetChild(3).GetComponent<TMP_Text>();
        }
        else
        {
            Debug.LogError("Player GameObject not found for the given PhotonView ID.");
        }
    }


    [PunRPC]
    private void Dead()
    {
        if (!isDead && view.IsMine)  // Ensure this runs only if not already marked dead
        {
            isDead = true; // Mark as dead to prevent re-entry
            healthAmount = 0;
            gun.enabled = false;
            player.GetComponent<GunMechanicManager>().enabled = false;
            bc.enabled = false;
            bc2.enabled = false;
            sr_body.enabled = false;
            sr_legs.enabled = false;
            sr_arms.enabled = false;
            rb.gravityScale = 0;
            fillImage.fillAmount = 0;
            player.GetComponent<Movement>().enabled = false;
            view.RPC("UpdateLifeCounterOnAllClients", RpcTarget.AllBuffered);
            view.RPC("Respawn", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Respawn()
    {
        StartCoroutine("RespawnTimer");
    }

    [PunRPC]
    public void ReduceHealth(float amount, int targetViewID)
    {
        healthAmount -= amount;
        // Broadcast the health update to all clients
        PhotonView.Get(this).RPC("UpdateHealthUI", RpcTarget.Others, targetViewID, healthAmount);
    }

    [PunRPC]
    public void UpdateHealthUI(int playerViewID, float newHealth)
    {
        PhotonView targetView = PhotonView.Find(playerViewID);
        if (targetView != null)
        {
            Health targetHealth = targetView.GetComponent<Health>();
            if (targetHealth != null)
            {
                // Update the health bar UI for this player
                targetHealth.fillImage.fillAmount = newHealth / 100;
            }
        }
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(1f); // Sufficient delay for respawn mechanics
        // Reset the player's position and health
        player.transform.position = respawnArea.position;
        healthAmount = 100;
        isDead = false;

        // Re-enable the player's components
        EnablePlayerComponents(true);
        fillImage.fillAmount = 1;
    }

    private void EnablePlayerComponents(bool isEnabled)
    {
        sr_body.enabled = isEnabled;
        sr_legs.enabled = isEnabled;
        sr_arms.enabled = isEnabled;
        bc.enabled = isEnabled;
        bc2.enabled = isEnabled;
        player.GetComponent<GunMechanicManager>().enabled = isEnabled;
        rb.gravityScale = isEnabled ? 3.0f : 0;
        player.GetComponent<Movement>().enabled = isEnabled;
        gun.enabled = true;
    }

    [PunRPC]
    public void SetUIColor(int colorIndex)
    {
        icon.sprite = coloredIcons[colorIndex];
    }

    [PunRPC]
    public void UpdateLifeCounterOnAllClients()
    {
        lives--;
        livesDisplay.text = lives.ToString();

        Hashtable props = new Hashtable
        {
        {"Lives", lives}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        if (lives <= 0)
        {
            // Find the last player with lives remaining
            Player lastAlive = FindLastAlivePlayer();
            if (lastAlive != null && view.IsMine)  // Only the master client should update the room properties
            {
                Hashtable winner = new Hashtable
            {
                {"Winner", lastAlive.ActorNumber}  // Using ActorNumber as the unique identifier
            };
                PhotonNetwork.CurrentRoom.SetCustomProperties(props);
            }
            PhotonNetwork.LoadLevel(4);
        }
    }

    private Player FindLastAlivePlayer()
    {
        Player lastAlive = null;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object playerLives;
            if (p.CustomProperties.TryGetValue("Lives", out playerLives) && (int)playerLives > 0)
            {
                lastAlive = p;  // Update last alive player
            }
        }
        return lastAlive;
    }
}


