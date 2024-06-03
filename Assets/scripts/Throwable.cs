using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Throwable", menuName = "Throwable")]
public class Throwable : Item
{
    public GameObject throwablePrefab;
    GameObject obj;
    public float hitKb;
    public float damage;
    public float throwDelay;
    public bool customAnim = true;
    public int throwableAmount;

    public override void Use(bool isRight, Transform gunTip, PhotonView view)
    {
        Debug.Log("Using " + this.itemName);
        if (isRight)
        {
            obj = PhotonNetwork.Instantiate(throwablePrefab.name, gunTip.transform.position, Quaternion.identity, 0);
        }

        else
        {
           obj = PhotonNetwork.Instantiate(throwablePrefab.name, gunTip.transform.position, Quaternion.identity, 0);
           obj.GetComponent<PhotonView>().RPC("changeDir_Left", RpcTarget.AllBuffered);
        }
        Vector2 dir = new Vector2(2, 1);
        if(isRight == false)
        {
            dir.x = -dir.x;
        }
        obj.GetComponent<Bomb>().dir = dir;
        obj.GetComponent<Bomb>().thrower_view = view;
    }

    public override float GetDamage()
    {
        return damage;
    }

    public override float GetHitKB()
    {
        return hitKb;
    }

    public override bool CustomAnim()
    {
        return customAnim;
    }

    public override int getBulletCount()
    {
        return throwableAmount;
    }
}
