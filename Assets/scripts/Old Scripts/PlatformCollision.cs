using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    public Movement movementScript;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if(movementScript.gameObject.GetComponent<Rigidbody2D>().velocity.y > 0) 
            { return; }
            //Debug.Log("Player Has Landed");
            movementScript.isGrounded = true;
            movementScript.bodyController.SetBool("IsGrounded", movementScript.isGrounded);
            movementScript.armController.SetBool("IsGrounded", movementScript.isGrounded);
            movementScript.legController.SetBool("IsGrounded", movementScript.isGrounded);
            movementScript.eyesController.SetBool("IsGrounded", movementScript.isGrounded);
            movementScript.currentPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        movementScript.currentPlatform = null;
    }
}
