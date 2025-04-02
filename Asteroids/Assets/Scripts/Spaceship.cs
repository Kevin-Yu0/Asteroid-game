using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public GameObject bulletPrefab; // Prefab for the bullet
    public GameObject explosionPrefab; // Prefab for the explosion effect (optional, can be used for visual effects)
    [SerializeField] private float maxX = 9f; // Maximum x position for wrapping around the screen
    [SerializeField] private float maxY = 5f; // Maximum y position for wrapping around the screen 
    [SerializeField] private float turnSpeed = 180; // Degrees per second for turning
    [SerializeField] private float thrust = 1f; // Speed at which the spaceship moves forward
    private float maxSpeed = 3f; // Maximum speed of the spaceship
    private Vector3 shipDirection = new Vector3(0, 1, 0); // Default direction is up
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    private GameController gameController;
    private AudioSource audioSource;
    public AudioClip shootSound; // Sound effect for shooting
    public AudioClip thrustSound; // Sound effect for thrusting

    private void Awake()
    {
        gameObject.tag = "Player"; // Set the tag for the spaceship
        gameObject.name = "Player"; // Set the name for the spaceship
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the GameObject

        audioSource = gameObject.GetComponent<AudioSource>(); // Add an AudioSource component to the spaceship
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float turnAngle;
        if (Input.GetKey(KeyCode.A))
        {
            // turn left
            turnAngle = turnSpeed * Time.deltaTime;
            transform.Rotate(0, 0, turnAngle);
            shipDirection = Quaternion.Euler(0, 0, turnAngle) * shipDirection; // Update ship direction
        }
        if (Input.GetKey(KeyCode.D))
        {
            // turn right
            turnAngle = -turnSpeed * Time.deltaTime;
            transform.Rotate(0, 0, turnAngle);
            shipDirection = Quaternion.Euler(0, 0, turnAngle) * shipDirection; // Update ship direction
        }
        if (Input.GetKey(KeyCode.W))
        {
            // move forward
            rb.AddForce(shipDirection * thrust); // Apply force in the current ship direction
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // play thrust sound
            audioSource.PlayOneShot(thrustSound); // Play thrust sound
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            // stop thrust sound
            audioSource.Stop(); // Stop thrust sound
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // shoot bullet
            audioSource.PlayOneShot(shootSound); // Play shooting sound
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity); // Create a new bullet
            bullet.transform.rotation = transform.rotation * Quaternion.Euler(0, 0, 90); // Set the bullet's rotation to match the ship's rotation
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the bullet
            bulletRb.AddForce(shipDirection * 10f); // Apply force to the bullet in the ship direction
        }

        // if ship goes off edge, wrap around to the other side of the screen
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

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed; // Limit the speed of the spaceship
        }
    }

    public void SetGameController(GameController controller)
    {
        gameController = controller; // Set the reference to the GameController if needed
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            gameController.timedied=Time.time; 
            Destroy(gameObject); 
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Create explosion effect (optional)
        }
    }
}
