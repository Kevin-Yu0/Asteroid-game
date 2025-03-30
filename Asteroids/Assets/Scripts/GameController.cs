using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
    public GameObject AsteroidPrefab;
    public GameObject SpaceShipPrefab;
    public float timedied;
    private GameObject gameOverSign;
    private GameObject spaceship;
    private int numAsteroids = 5;
    private float minCollisionDistance = 1.0f;
    private int maxLives = 3;
    private int lives;
    private float respawnTime = 3f;

    private void Awake()
    {
        lives = maxLives;
        gameOverSign = GameObject.Find("GameOver"); // Find the GameOverSign in the scene
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        // Initialize the game level, spawn asteroids, etc.
        for (int i = 0; i < numAsteroids; i++)
        {
            SpawnAsteroid();
        }
        SpawnSpaceShip();
        Assert.IsNotNull(gameOverSign);
        gameOverSign.SetActive(false); // Hide the GameOverSign at the start of the game
    }

    private void SpawnAsteroid()
    {
        GameObject newAsteroid;
        bool valid;
        do
        {
            newAsteroid = Instantiate(AsteroidPrefab);
            newAsteroid.gameObject.tag = "Asteroid";
            valid = CheckObjectCollision(newAsteroid);
        } while (valid == false); // Loop until a valid position is found
    }

    private bool CheckObjectCollision(GameObject newObject)
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        foreach (GameObject a in asteroids)
        {
            if (a != newObject) // Avoid checking against itself
            {
                // Check if the new asteroid collides with any existing asteroids
                if (Vector3.Distance(newObject.transform.position, a.transform.position) < minCollisionDistance)
                {
                    Destroy(newObject); // Destroy the new asteroid if it collides with an existing one
                    return false; // Collision detected, return false
                }
            }
        }
        return true;
    }

    private void SpawnSpaceShip()
    {
        GameObject newSpaceShip;
        bool valid;
        do
        {
            newSpaceShip = Instantiate(SpaceShipPrefab); // Create a new spaceship instance
            newSpaceShip.transform.position = new Vector3(Random.Range(-9f, 9f), Random.Range(-5f, 5f), 0);
            newSpaceShip.gameObject.tag = "Player"; // Set the tag for the spaceship
            valid = CheckObjectCollision(newSpaceShip); // Check for collisions with existing objects
        } while (!valid);
        spaceship = GameObject.FindGameObjectWithTag("Player"); // Find the spaceship in the scene after spawning it
        spaceship.GetComponent<Spaceship>().SetGameController(this); // Set the reference to the GameController in the Spaceship script
        lives--;
    }

    void Start()
    {

    }   

    void Update()
    {
        if (spaceship == null)
        {
            if (lives > 0)
            {
                if(Time.time - timedied < respawnTime)
                {
                    return; // Wait for the respawn time before respawning the spaceship
                }
                // Respawn the spaceship if it was destroyed and lives are available
                SpawnSpaceShip();
            }
            else
            {
                gameOverSign.SetActive(true); // Show the GameOverSign if no lives left
            }
        }
    }
}
