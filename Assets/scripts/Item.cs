using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(fileName = "Items/New Item", menuName = "")]

public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public float gunTipYOffset;
    public float useDelay;
    public int bulletCount;
    public bool reusable;
    public float reloadTime;
    public string fireAnimation;
    public string type;
    public string idle_animation;
    public string run_animation;
    public string reload_anim;
    public float GuncolliderOffsetX, GuncolliderOffsetY;
    public float GunColliderSizeX, GunColliderSizeY;
    public bool hasToBeGrounded = false;

    // The virtual method for using the item, which can be overridden in subclasses
    public virtual void Use(bool isRight, PhotonView view)
    {
        // Use the item
        // Example: Debug.Log(itemName + " used.");
    }

    public virtual void Use(bool isRight, Transform gunTip, PhotonView view)
    {
        // Use the item
        // Example: Debug.Log(itemName + " used.");
    }

    public virtual float GetRecoilKb()
    {
        return 0f; // Default recoil for general items
    }

    public virtual float GetDamage()
    {
        return 0f;
    }

    public virtual float GetHitKB()
    {
        return 0f;
    }

    public virtual bool CustomAnim()
    {
        return false;
    }

    public virtual string getType()
    {
        return type;
    }
    public virtual string getIdle()
    {
        return idle_animation;
    }

    public virtual string getRun()
    {
        return run_animation;
    }

    public virtual int getBulletCount()
    {
        return bulletCount;
    }    

    public virtual bool isReusable()
    {
        return reusable;
    }

    public virtual float getReloadTime()
    {
        return reloadTime;
    }

    public virtual string getReloadAnim()
    {
        return reload_anim;
    }

    public virtual bool isAutomatic()
    {
        return false;
    }

    public virtual float GetGuncolliderOffsetX()
    {
        return GuncolliderOffsetX;
    }
    public virtual float GetGuncolliderOffsetY()
    {
        return GuncolliderOffsetY;
    }
    public virtual float GetGuncolliderSizeX()
    {
        return GunColliderSizeX;
    }
    public virtual float GetGuncolliderSizeY()
    {
        return GunColliderSizeY;
    }
}
