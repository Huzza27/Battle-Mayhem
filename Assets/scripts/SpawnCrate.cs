using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class SpawnCrate : MonoBehaviour
{
    public Item[] items;
    public bool gameEnd = false;
    public GameObject crate;
    GameObject newCrate;
    int delay;
    CrateFunctionality functionality;
    public bool canSpawn = true;

    private void Start()
    {
        StartCoroutine("crateSpawnTimer");
        canSpawn = false;
    }
    private void Update()
    {
        if(canSpawn)
        {
            StartCoroutine("crateSpawnTimer");
        }
    }

    public IEnumerator crateSpawnTimer()
    {
        delay = Random.Range(10, 15);
        Debug.Log("Starting Timer");
        yield return new WaitForSeconds(delay);
        Spawn();
    }

    public void Spawn()
    {
        if (newCrate == null)
        {
            newCrate = PhotonNetwork.Instantiate(crate.name, GenerateRandomVector2(4, 30, -3, 18), Quaternion.identity);
            functionality = newCrate.GetComponent<CrateFunctionality>();
            functionality.spawner = this.gameObject.GetComponent<SpawnCrate>();
        }
        
    }

    public static Vector2 GenerateRandomVector2(float minX, float maxX, float minY, float maxY)
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        return new Vector2(x, y);
    }



}
