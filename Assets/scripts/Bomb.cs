using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bomb : MonoBehaviour
{
    public float tossForce = 10f;
    private Rigidbody2D rb;
    public Vector2 dir;
    public float lifetTime = 2.5f;
    public float knockBackForce = 20f;
    public PhotonView thrower_view;

    public float damage = 60f;

    public GameObject explosionPartlicles;

    public GameObject target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(dir * tossForce, ForceMode2D.Impulse);
        StartCoroutine("lifeTimer");
    }

    private IEnumerator lifeTimer()
    {
        yield return new WaitForSeconds(lifetTime);
        Explode();
    }

    public void Explode()
    {
        PhotonNetwork.Instantiate(explosionPartlicles.name, transform.position, Quaternion.identity);
        CheckForHit();
        if (target != null)
        {
            target.GetComponent<PhotonView>().RPC("TakeKnockBackFromBomb", RpcTarget.AllBuffered, dir, knockBackForce);
            target.GetComponent<PhotonView>().RPC("ReduceHealth", RpcTarget.AllBuffered, damage, target.GetComponent<PhotonView>().ViewID);
        }
        Destroy(this.gameObject);
    }

    private void CheckForHit()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
        foreach (Collider2D collider in colliders)
        {
            if(collider.tag == "Player")
            {
                if(collider.gameObject.transform.parent == null)
                {
                    target = collider.gameObject;
                }
                else
                {
                    target = collider.gameObject.transform.root.gameObject;
                }
            }
        }
    }


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    /*
    if(collision.gameObject.tag == "Player" && canExplode)
    {
        if(collision.gameObject.transform.parent == null)
        {
            target = collision.gameObject;
        }

        else
        {
            target = collision.gameObject.transform.parent.gameObject;
        }
        Explode();
    }
}
private void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.tag == "Player")
    {
        canExplode = true;
    }
}
*/



}