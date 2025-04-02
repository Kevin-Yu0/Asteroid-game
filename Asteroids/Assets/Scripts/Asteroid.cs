using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class Asteroid : MonoBehaviour
{
    public GameObject asteroidPreFab;
    public GameObject explosionPrefab;
    private GameController gameController;
    private float maxX = 9f;
    private float maxY = 5f;
    private float maxSpeed = 2f;
    private float maxHealth = 3;
    private Rigidbody2D rb;
    private int maxScale = 3;
    public float childOffset = 1f;
    public float health;
    public int scale;

    private void Awake()
    {
        gameObject.tag = "Asteroid"; 
        gameObject.name = "Asteroid"; 
        scale = maxScale;
        health = maxHealth;
        // Initialize the asteroid's position and velocity
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0); // Randomly position the asteroid within the screen bounds
        rb.linearVelocity = Quaternion.Euler(0, 0, Random.Range(0, 360)) * new Vector3(Random.Range(0, maxSpeed), 0.0f, 0.0f); // Randomly set the velocity of the asteroid
    }

    void Start()
    {

    }

    void Update()
    {
        // if the asteroid goes off edge, wrap around to the other side of the screen
        if (transform.position.x < -maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > maxX)
        {
            transform.position = new Vector3(-maxX, transform.position.y, transform.position.z);
        }
        if (transform.position.y < -maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
        else if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, -maxY, transform.position.z);
        }
    }

    public void SetGameController(GameController controller)
    {
        gameController = controller; // Set the reference to the GameController if needed
    }

    private void Die(){
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Create an explosion effect at the asteroid's position
        ParticleSystem explosionParticles = explosion.GetComponent<ParticleSystem>();
        explosionParticles.Stop();
        var main = explosionParticles.main;
        main.startSize = scale+1;
        main.simulationSpeed = 0.5f * (1+maxScale-scale);
        explosionParticles.Play(); 
        if (scale>0){
            spawnChildAsteroids();
        }
        Destroy(gameObject);
        gameController.numAsteroids--; 
    }

    private void spawnChildAsteroids(){
        Vector2[] newDirection = new Vector2[4];
        newDirection[0] = new Vector2(1,0);
        newDirection[1] = new Vector2(-1,0);
        newDirection[2] = new Vector2(0,1);
        newDirection[3] = new Vector2(0,-1);
        float randAngle = Random.Range(0,360);

        for (int i =0; i<4; i++){
            newDirection[i] = ((Vector2)(Quaternion.Euler(0, 0, randAngle) * (Vector3)newDirection[i]));
            GameObject newAsteroid = Instantiate(asteroidPreFab);
            Asteroid asteroidHandle = newAsteroid.GetComponent<Asteroid>();
            newAsteroid.transform.position=transform.position + (Vector3)(newDirection[i]*childOffset);
            newAsteroid.transform.localScale=transform.localScale/2;
            asteroidHandle.scale = scale - 1;
            Rigidbody2D childRb = newAsteroid.GetComponent<Rigidbody2D>();
            childRb.mass = rb.mass/8;
            childRb.AddForce((Vector3) newDirection[i]*childOffset);
            asteroidHandle.childOffset = childOffset/2;
            asteroidHandle.health = health/2; 
            newAsteroid.GetComponent<Asteroid>().SetGameController(gameController); // Set the reference to the GameController for the new asteroid
        }
        gameController.numAsteroids += 4; 
    }

    public void takeDamage(){
        health --;
        if (health <= 0){
            Die();
            gameController.IncreaseScore();
        }
    }
}
