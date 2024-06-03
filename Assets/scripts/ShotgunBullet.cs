using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;

public class ShotgunBullet : MonoBehaviour
{
    public PhotonView view;


    public bool MoveDir = false;

    public float moveSpeed;

    public float destroyTime;

    public SpriteRenderer spriteRenderer;

    public PhotonView shooterView;
    public float damage;
    public float hitkb;

    private void Awake()
    {
        StartCoroutine("DestroyTimer");
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
        transform.Rotate(0f, 180f, 0f);
        spriteRenderer.flipX = true;
        MoveDir = true;
    }

    [PunRPC]
    public void DestroyObject()
    {
        PhotonNetwork.Destroy(this.gameObject);
    }

    IEnumerator changeSprite()
    {
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.sprite = null;
    }

    void OnParticleCollision(GameObject other)
    {
        PhotonView targetView;
        if (other.tag == "Player")
        {
            Debug.Log("Hit player");
            if (other.transform.parent == null)
            {
                targetView = other.GetComponent<PhotonView>();
            }
            else
            {
                targetView = other.transform.root.GetComponent<PhotonView>();
            }

            if (targetView != null && targetView.ViewID != shooterView.ViewID)
            {
                targetView.RPC("TakeKnockBackFromBullet", RpcTarget.AllBuffered, hitkb * 100);
                targetView.RPC("ReduceHealth", RpcTarget.AllBuffered, damage, targetView.ViewID);
            }
            else
            {
                Debug.Log("No PhotonView found on the target or attempting to modify self.");
            }
        }
    }

}
