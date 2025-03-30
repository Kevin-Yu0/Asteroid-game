using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float maxX = 9f; // Maximum x position for wrapping around the screen
    private float maxY = 5f; // Maximum y position for wrapping around the screen
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position.x < -maxX) || (transform.position.x > maxX) || (transform.position.y < -maxY) || (transform.position.y >maxY)){
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.tag == "Asteroid"){
            Asteroid asteroid = collision.GetComponent<Asteroid>();
            asteroid.takeDamage();
            Destroy(gameObject);
        }
    }
}
