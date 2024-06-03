using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerPlatformTrigger : MonoBehaviour
{
    BoxCollider2D platform;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            platform = gameObject.GetComponentInParent<BoxCollider2D>();
            platform.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            platform = gameObject.GetComponentInParent<BoxCollider2D>();
            platform.enabled = true;
        }
    }
}
