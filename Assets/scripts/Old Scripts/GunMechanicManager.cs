using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using System.Security.Cryptography.X509Certificates;

public class GunMechanicManager : MonoBehaviour
{
    GameSetup setup;
    public SpawnCrate crateSpawner;
    [SerializeField] public Item[] items;
    public Item heldItem;
    public int currentItemIndex;
    public GameObject hand;
    public Transform gunTip;


    public PhotonView view;

    public GameObject bulletPrefab;

    SpriteRenderer bulletSpriteRenderer;
    public Sprite bullet;
    public float speed = 10f;
    public bool isRight;
    public Movement movement;
    public int itemIndex = 0;

    public float forceAmount = 10f;

    bool canUseItem = true;

    public Animator gunController;
    public Animator armController;

    public float resetHandDelay = 0.75f;
    float pistolRecoilAnimationtimer;
    bool timerActive = false;

    int bulletCount;

    int originalItemIndex;

    private void Awake()
    {
        
        crateSpawner = GameObject.FindGameObjectWithTag("Crate Spawner").GetComponent<SpawnCrate>();
        populateItemList();
        originalItemIndex = itemIndex;
        pistolRecoilAnimationtimer = 0f;
    }
    private void Start()
    {

        //access setup methods
        heldItem = items[itemIndex];
        bulletCount = heldItem.getBulletCount();
        view.RPC("updateBulletCount", RpcTarget.AllBuffered, bulletCount);
        setup = GetComponent<GameSetup>();
        bulletSpriteRenderer = bulletPrefab.GetComponent<SpriteRenderer>(); //get bullet sprite rtendere
        isRight = movement.facingRight; // set facing to right
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.T) && canUseItem)
            {
                Shoot();
                if(!heldItem.hasToBeGrounded)
                {
                    StartTimer();
                }
            }

            if (Input.GetKey(KeyCode.T) && canUseItem && !timerActive && heldItem.isAutomatic())
            {
                Shoot();
                StartTimer();
            }

            //Timer Logic

            if (timerActive)
            {
                pistolRecoilAnimationtimer -= Time.deltaTime;
                if (pistolRecoilAnimationtimer <= 0f)
                {
                    timerActive = false;  // Stop the timer
                    StartCoroutine(ResetAnimationTrigger("ResetHand", 0.1f));  // Reset after a short delay to allow animation to play
                }
            }
        }

        if (bulletCount == 0)
        {
            canUseItem = false;
            ReloadWeapon();
        }
    }

    /*
     * This method gets called from the SpawnCrate object to populate the list of Items
     */ 

    private void populateItemList()
    {
        items = crateSpawner.items;
    }

    IEnumerator ResetAnimationTrigger(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);
        armController.SetBool(triggerName, true);
    }

    [PunRPC]
    public void SwapItems(int itemIndex)
    {
        Item newItem = items[itemIndex];
        heldItem = newItem;
        hand.GetComponent<SpriteRenderer>().sprite = heldItem.icon;
        setup.AdjustGunTipPosition(heldItem.gunTipYOffset, heldItem);
        ResetBulletCount();  // Ensuring this is called after updating heldItem
    }

    IEnumerator weaponDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canUseItem = true;
    }

    void Shoot()
    {
        if (bulletCount > 0 && canUseItem)
        {
            if (heldItem.fireAnimation != null)
            {
                gunController.Play(heldItem.fireAnimation);
                armController.Play(heldItem.fireAnimation);
            }
            //If object has bullets
            isRight = movement.facingRight;
            heldItem.Use(isRight, gunTip, view);
            bulletCount--;
            view.RPC("updateBulletCount", RpcTarget.AllBuffered, bulletCount);
            canUseItem = false;
            //
            if (heldItem.useDelay > 0)
            {
                StartCoroutine(weaponDelay(heldItem.useDelay)); // Use delay from weapon if greater than zero
            }
            else
            {
                canUseItem = true; // If no delay, enable firing immediately
            }

            if (isRight == true)
            {
                forceAmount *= -1;
            }
            //Debug.Log("Knock Shooter back " + heldItem.GetRecoilKb());
            if (isRight)
            {
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(-heldItem.GetRecoilKb() * 100, 0f)); //Knockback in proper direction
            }
            else
            {
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(heldItem.GetRecoilKb() * 100, 0f));
            }

            if (bulletCount == 0)
            {
                ReloadWeapon(); // Trigger reload if bullet count reaches 0
            }
        }
    }


    void StartTimer()
    {
        if (heldItem.useDelay > 0)
        {
            pistolRecoilAnimationtimer = heldItem.useDelay; // Use the weapon's useDelay if specified
        }
        else
        {
            pistolRecoilAnimationtimer = 0.1f; // Minimum time to ensure the timer logic works, adjust as needed
        }
        timerActive = true;
    }

    [PunRPC]
    public void TakeKnockBackFromBullet(float kb)
    {
        this.GetComponent<Rigidbody2D>().AddForce(new Vector2(kb, 0f));
    }


    [PunRPC]
    public void TakeKnockBackFromBomb(Vector2 dir, float kb)
    {
        GetComponent<Rigidbody2D>().AddForce(dir * kb, ForceMode2D.Impulse);
    }



    public void ReloadWeapon()
    {
        if (heldItem.reusable == true)
        {
            canUseItem = false; // Prevent shooting during reload
            armController.Play(heldItem.getReloadAnim());
            float time = armController.GetCurrentAnimatorStateInfo(0).length;
            StartCoroutine(reloadTimer(time)); // Start reload coroutine without string reference
        }
        else
        {
            // Swap back logic
            view.RPC("SwapItems", RpcTarget.AllBuffered, originalItemIndex);
            ResetBulletCount(); // Reset bullet count immediately for non-reusable items
        }
    }

    private void ResetBulletCount()
    {
        bulletCount = heldItem.getBulletCount(); // Make sure this method accurately reflects the count for any item type
        view.RPC("updateBulletCount", RpcTarget.AllBuffered, bulletCount);
    }

    public IEnumerator reloadTimer(float time)
    {
        //Debug.Log("Reloading. Waiting for " + heldItem.getReloadTime() + " seconds");
        hand.GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(time);
        hand.GetComponent<SpriteRenderer>().enabled = true;
        bulletCount = heldItem.getBulletCount(); // Reset bullet count after reload
        armController.SetTrigger("ReturnToGun");
        view.RPC("updateBulletCount", RpcTarget.AllBuffered, bulletCount);
        canUseItem = true; // Allow shooting after reload
    }

}
