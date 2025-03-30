using System.Collections;
using UnityEngine;

public class AsteroidExplode : MonoBehaviour
{
    private void Awake()
    {
        gameObject.name = "AsteroidExplode"; // Set the name of the object for debugging purposes
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(selfDestruct()); // Start the self-destruct coroutine to destroy the ship after 2 seconds
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject); // Destroy the ship object after 3 seconds
    }
}
