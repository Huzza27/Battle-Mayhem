using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Unity.VisualScripting;

public class Bullet : MonoBehaviour
{
    public PhotonView view;

    public bool MoveDir = false;

    public float moveSpeed;

    public float destroyTime;

    SpriteRenderer spriteRenderer;

    public Sprite bullet;

    bool isBullet = false;

    [SerializeField] public Item gun;

    float kb;

    GameObject targetPlayer;

    private void Awake()
    {
        StartCoroutine("DestroyTimer");
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("changeSprite");

    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(destroyTime);
        GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void changeDir_Left()
    {
        //Debug.Log("Changing Direction");
        spriteRenderer.flipX = true;
        MoveDir = true;
    }

    [PunRPC]
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }

    private void Update()
    {
        if (isBullet)
        {
            if (!MoveDir)
            {
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!view.IsMine)
            return;
        PhotonView target = collision.gameObject.GetComponent<PhotonView>();

        if(target != null && (!target.IsMine || target.IsRoomView))
        {
            if(target.tag == "Player")
            {
                targetPlayer = collision.gameObject;
                int targetViewID = target.ViewID;
                target.RPC("ReduceHealth", RpcTarget.AllBuffered, gun.GetDamage(), targetViewID);
                //Debug.Log("Knock target back" + gun.GetHitKB());

                if(!MoveDir)
                {
                    kb = gun.GetHitKB() * 100;
                }
                else 
                { 
                    kb = -gun.GetHitKB() * 100;
                }
                
                targetPlayer.GetComponent<PhotonView>().RPC("TakeKnockBackFromBullet", RpcTarget.AllBuffered, kb);
            }
            this.GetComponent<PhotonView>().RPC("DestroyObject", RpcTarget.AllBuffered);
        }
    }

    IEnumerator changeSprite()
    {
        yield return new WaitForSeconds(0.02f);
        spriteRenderer.sprite = bullet;
        isBullet = true;
    }
}
