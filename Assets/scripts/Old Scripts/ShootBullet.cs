using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    /*
    
    public Sprite bullet;
    new SpriteRenderer renderer;
    public float speed = 10f;
    
    bool isRight;
    public Movement movement;
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        StartCoroutine("changeSprite");
        isRight = movement.facingRight;

    }

    private void Update()
    {
        if(isBullet)
        {
            if(isRight)
            {
                renderer.flipX = true;
                transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
            }
            else
            {
                renderer.flipX = true;
                transform.Translate(Vector3.left * speed * Time.deltaTime, Space.Self);
            }
        }
    }

    IEnumerator changeSprite()
    {
        yield return new WaitForSeconds(0.05f);
        renderer.sprite = bullet;
        isBullet = true;
    }

    */
}
