using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private string MainMenu;
    private Rigidbody rb; 
    private int count;
    private float movementX;
    private float movementY;
    
    public float maxAcceleration = 0;  // Maximum possible acceleration, set manually in Unity
    private float acceleration;  // Acceleration will be initialized to maxAcceleration at runtime
    
    public TextMeshProUGUI countText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI speedText;  // To display the player's current speed (velocity magnitude)
    public TextMeshProUGUI maxAccelerationText;  // To display the max acceleration on the screen
    public GameObject winTextObject;
    
    private float timer;
    private bool hasWon = false;
    private bool alive = true;

    private float speedReductionTimer = 0;  // Timer to track when to reduce acceleration

    // Audio sources for sounds
    public AudioSource pickUpSound; // Sound for picking up items
    public AudioSource deathSound;  // Sound for player death

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        timer = 0;
        speedReductionTimer = 0; // Initialize speed reduction timer
        winTextObject.SetActive(false);

        // Initialize acceleration to the value of maxAcceleration
        acceleration = maxAcceleration;

        // Display the max acceleration in m/s² when the game starts
        maxAccelerationText.text = "Max Acceleration: " + acceleration.ToString("F2") + " m/s²";
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

    private void FixedUpdate() 
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * acceleration);  // Apply acceleration force to move the player
        Die();

        // Update real-time speed (velocity magnitude) on the screen in km/h
        float currentSpeed = rb.velocity.magnitude;  // Get the player's current speed in m/s
        speedText.text = "Speed: " + ConvertSpeedToKmPerH(currentSpeed).ToString("F2") + " km/h";  // Display speed in km/h

        // Increment both timers
        if (!hasWon) 
        {
            timer += Time.deltaTime;
            speedReductionTimer += Time.deltaTime;  // Increment the speed reduction timer

            // Check if it's time to reduce acceleration
            if (speedReductionTimer >= 10 && acceleration > 5) 
            {
                acceleration -= 3;  // Reduce the acceleration
                maxAcceleration = acceleration;  // Sync maxAcceleration with the current acceleration
                speedReductionTimer = 0;  // Reset the reduction timer
                Debug.Log("Acceleration and Max Acceleration have been reduced.");

                // Update the displayed max acceleration in m/s²
                maxAccelerationText.text = "Max Acceleration: " + maxAcceleration.ToString("F2") + " m/s²";
            }

            timerText.text = "Time: " + timer.ToString("F2") + " s";
        }
    }

    // Convert speed from m/s to km/h (multiply by 3.6)
    float ConvertSpeedToKmPerH(float speedInMetersPerSecond)
    {
        return speedInMetersPerSecond * 3.6f;
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag("PickUp")) 
        {
            if (pickUpSound != null)
            {
                pickUpSound.Play();
            }

            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
    }

    void Die()
    {
        if (transform.position.y < -10 && alive)
        {
            alive = false;
            Debug.Log("Player has died, playing death sound.");

            if (deathSound != null)
            {
                deathSound.Play();
            }

            StartCoroutine(DieAndReturnToMenu());
        }
    }

    IEnumerator DieAndReturnToMenu()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(MainMenu);
    }

    void SetCountText() 
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 13)
        {
            winTextObject.SetActive(true);
            hasWon = true;
            StartCoroutine(WinAndReturnToMenu());
        }
    }

    IEnumerator WinAndReturnToMenu()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(MainMenu); 
    }
}
