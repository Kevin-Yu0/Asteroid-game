using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{
    public GameObject AsteroidPrefab;
    public GameObject SpaceShipPrefab;
    public GameObject[] playerLives;
    public float timedied;
    private GameObject gameOverSign;
    private GameObject levelClearedSign;
    private GameObject spaceship;
    private int myScore = 0;
    private Score scoreText;
    public int numAsteroids = 1;
    private float minCollisionDistance = 1.0f;
    private int maxLives = 3;
    [SerializeField] private int lives;
    private float respawnTime = 3f;

    private void Awake()
    {
        lives = maxLives;
        scoreText = FindFirstObjectByType<Score>();
        gameOverSign = GameObject.Find("GameOver"); // Find the GameOverSign in the scene
        levelClearedSign = GameObject.Find("LevelCleared"); // Find the LevelClearedSign in the scene, if any
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

        Assert.IsNotNull(levelClearedSign);
        levelClearedSign.SetActive(false); 

        StartCoroutine(EnsureScoreInitialized()); // Ensure the score is initialized

    }

    private IEnumerator EnsureScoreInitialized()
{
    while (scoreText == null)
    {
        scoreText = FindAnyObjectByType<Score>();
        yield return null; // Wait a frame
    }

    scoreText.UpdateMyScore(myScore);
}

    private void SpawnAsteroid()
    {
        GameObject newAsteroid;
        bool valid;
        do
        {
            newAsteroid = Instantiate(AsteroidPrefab);
            newAsteroid.gameObject.tag = "Asteroid";
            newAsteroid.GetComponent<Asteroid>().SetGameController(this); // Set the reference to the GameController in the Asteroid script
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
            valid = CheckObjectCollision(newSpaceShip); // Check for collisions with existing objects
        } while (!valid);
        spaceship = GameObject.FindGameObjectWithTag("Player"); // Find the spaceship in the scene after spawning it
        spaceship.GetComponent<Spaceship>().SetGameController(this); // Set the reference to the GameController in the Spaceship script
        lives--;
    }

    IEnumerator delay(float seconds)
    {
       yield return new WaitForSeconds(seconds); 
    }

    public void IncreaseScore(){
        myScore += 10;
        scoreText.UpdateMyScore(myScore);
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
                if(Time.time - timedied > respawnTime)
                {
                    SpawnSpaceShip();
                    StartCoroutine(delay(0.5f)); 
                    Destroy(playerLives[lives+1]);

                }
            }
            else
            {
                Destroy(playerLives[0]); 
                gameOverSign.SetActive(true); // Show the GameOverSign if no lives left
            }
        }
        if (numAsteroids==0)
        {
            levelClearedSign.SetActive(true); // Show the LevelClearedSign
        }
    }
}
