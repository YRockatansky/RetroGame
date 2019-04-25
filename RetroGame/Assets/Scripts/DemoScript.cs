using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour {

    public GameObject[] ObjectPool;
    public GameObject Sun;
    public float floatSpeed = 1f;
    public Transform RefreshPivot;
    public Transform TerrainRefreshPivot;
    public float speed = 1f;

    // Adjust speed over time. Make game harder and harder each 10 seconds!
    public void Start()
    {
        InvokeRepeating("IncreaseSpeed", 10f, 10f);
    }


    // Make them closer each second. Let them come next to camera
    void Update () {
        foreach(GameObject ob in ObjectPool)
        {
            ob.transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        Sun.transform.Translate(Vector3.up * Time.deltaTime * floatSpeed * Mathf.Sin(Time.time) * 0.2f); 
    }

    // Super simple..
    public void IncreaseSpeed()
    {
        speed += 20f;
    }

    // Make sure that your terrain marked with layer 'Terrain' - Number 24
    // Position changing stuff
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 24) // If terrain use the TerrainRefreshPoint
        {
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, TerrainRefreshPivot.transform.position.z);
        }
        else if(other.gameObject.layer == 9) { // We have a bad boy.. I mean, we have an enemy.. Or I don't know.. Innocent retro driver that we shouldn't crash? It's your call..
            int RandomPosition = Random.Range(0, 3) - 1; // Generate a random number to set random lane
            Vector3 newPosition = new Vector3(RefreshPivot.transform.position.x + (RandomPosition * 10), RefreshPivot.transform.position.y, RefreshPivot.transform.position.z);
            other.gameObject.transform.position = newPosition;
        }
        else
        { // If tree..
            other.gameObject.transform.position = RefreshPivot.transform.position;
        }
    }

}
