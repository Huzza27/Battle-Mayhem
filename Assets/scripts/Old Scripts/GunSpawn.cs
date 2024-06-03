using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawn : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {

    }



    // Update is called once per frame
    void Update()
    {
        
    }

    
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Gun Change");
            GetComponent<Renderer>().sprite = null;
            
            handLoc.GetComponent<SpriteRenderer>().sprite = gunStats.artwork;

            AdjustGunTipPosition();

        }
    }
    */
}
