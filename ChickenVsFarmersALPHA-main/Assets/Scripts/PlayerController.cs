using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Power-up fields
    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 8;

    // Projectile (Egg) fields
    public GameObject eggPrefab;
    private float eggCd = 0.6f;
    private float eggDefaultCd = 0.6f;
    private float eggSpawn = 0.0f;

    // Player movement fields
    public float speed = 5.0f;
    public float rotationSpeed = 10.0f; // Controls the smoothness of rotation
    private Vector3 shootDirection = Vector3.forward;

    // Reference to the SpawnManager
    private SpawnManager spawnManager;

    void Start()
    {
        // Locate and reference the SpawnManager in the scene
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        if (spawnManager.isGameActive) // Check if the game is active
        {
            HandleMovement();
            HandleProjectile();
            UpdatePowerupIndicator();
        }
    }

    private void HandleMovement()
    {
        // Capture player input for movement
        float speedX = Input.GetAxis("Horizontal");
        float speedZ = Input.GetAxis("Vertical");

        if (speedX != 0 || speedZ != 0)
        {
            // Calculate direction based on input and normalize it
            shootDirection = new Vector3(speedX, 0, speedZ).normalized;

            // Smooth rotation towards the target direction
            Quaternion targetRotation = Quaternion.LookRotation(shootDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Move the player based on input and speed
        Vector3 movement = new Vector3(speedX, 0, speedZ) * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);
    }

    private void HandleProjectile()
    {
        // Increment the cooldown timer
        eggSpawn += Time.deltaTime;

        // Spawn projectile (egg) if cooldown period has passed
        if (eggSpawn >= eggCd)
        {
            Instantiate(eggPrefab, transform.position, Quaternion.LookRotation(shootDirection));
            eggSpawn = 0.0f;
        }
    }

    private void UpdatePowerupIndicator()
    {
        // Position the powerup indicator slightly above the player
        powerupIndicator.transform.position = transform.position + new Vector3(0, 1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle power-up and experience pickups
        if (other.gameObject.CompareTag("Experience"))
        {
            Destroy(other.gameObject);
            spawnManager.UpdateScore(5);
        }
        else if (other.gameObject.CompareTag("Powerup"))
        {
            Debug.Log("Powerup ACTIVATED");
            ActivatePowerup(other);
        }
    }

    private void ActivatePowerup(Collider powerup)
    {
        hasPowerup = true;
        powerupIndicator.SetActive(true);
        Destroy(powerup.gameObject);

        // Increase firing rate while power-up is active
        eggCd = eggDefaultCd / 10;
        StartCoroutine(PowerupCooldown());
    }

    IEnumerator PowerupCooldown()
    {
        // Wait for the power-up duration, then reset properties
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        eggCd = eggDefaultCd;
        powerupIndicator.SetActive(false);
    }
}