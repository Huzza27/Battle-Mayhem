using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SemiAuto", menuName = "Gun/SemiAuto")]
public class SemiAutoGun : Item
{
    public GameObject bulletPrefab;
    GameObject obj;
    public float recoilKB;
    public float hitKb;
    public float damage;
    public float shotDelay;
    public bool automatic;

    public override void Use(bool isRight, Transform gunTip, PhotonView view)
    {
        //Debug.Log("Using " + this.itemName);
        if (isRight)
        {
            obj = PhotonNetwork.Instantiate(bulletPrefab.name, gunTip.transform.position, Quaternion.identity, 0);
        }

        else
        {
            obj = PhotonNetwork.Instantiate(bulletPrefab.name, gunTip.transform.position, Quaternion.identity, 0);
            obj.GetComponent<PhotonView>().RPC("changeDir_Left", RpcTarget.AllBuffered);
        }

        obj.GetComponent<Bullet>().gun = this;
    }

    public override float GetRecoilKb()
    {
        return recoilKB;
    }
    public override float GetDamage()
    {
        return damage;
    }

    public override float GetHitKB()
    {
        return hitKb;
    }

    public override bool isAutomatic()
    {
        return automatic;
    }
}
