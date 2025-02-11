using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private Asteroid smallAsteroid;
    [SerializeField] private Asteroid largeAsteroid;

    // The minimum and maximum amount of time between each big asteroid spawn.
    [SerializeField] private float minSpawnDelay = 5.0f;
    [SerializeField] private float maxSpawnDelay = 10.0f;

    // The minimum and maximum amount of small asteroids a big asteroids spawns when it gets destroyed.
    [SerializeField] private int _minAsteroidSplinters = 3;
    [SerializeField] private int _maxAsteroidSplinters = 6;
    // The timer controls the time lapse between spawns.
    private float _spawnDelayTimer;
    private float _spawnDelay;

    // The spawn boundaries are based of the camera settings
    private float _screenBoundaryX = 0.0f;
    private float _screenBoundaryY = 0.0f;

    private List<Asteroid> _asteroids = new List<Asteroid>();

    private void Start()
    {
        // Triggers the delay until the first spawn.
        SetSpawnDelay();

        // The spawn boundaries are based of the camera settings
        _screenBoundaryX = Camera.main.orthographicSize * Camera.main.aspect;
        _screenBoundaryY = Camera.main.orthographicSize;
    }
    private void Update()
    {
        _spawnDelayTimer += Time.deltaTime;
        if (_spawnDelayTimer >= _spawnDelay)
        {
            SpawnAsteroid();
        }
        CheckOutOfBoundsAsteroids();
    }
    private void SpawnAsteroid()
    {

        Vector2 direction = Random.insideUnitCircle;
        Vector3 position = Vector3.zero;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Make it appear on the left/right side
            position = new Vector3(Mathf.Sign(direction.x) * _screenBoundaryX, direction.y * _screenBoundaryY, 0);
        }
        else
        {
            // Make it appear on the top/bottom
            position = new Vector3(direction.x * _screenBoundaryX, Mathf.Sign(direction.y) * _screenBoundaryY, 0);
        }

        // Offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
        position -= position.normalized * 0.1f;

        var rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));

        var asteroid = Instantiate(largeAsteroid, position, rotation);
        asteroid.Setup(this);
        SpinBigAsteroid(asteroid);

        _asteroids.Add(asteroid);

        // Sets the delay until the next spawn.
        SetSpawnDelay();
    }

    private void SetSpawnDelay()
    {
        _spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        _spawnDelayTimer = 0;
    }
    // Checks whether any asteroid left the game boundaries. If it has, the asteroid gets despawned.
    private void CheckOutOfBoundsAsteroids()
    {
        for (int i = _asteroids.Count - 1; i >= 0; i--)
        {
            if (_asteroids[i] == null || !IsWithinScreenBoundary(_asteroids[i].transform.position))
            {
                Destroy(_asteroids[i].gameObject);
                _asteroids.RemoveAt(i);
            }
        }
    }
    public void RemoveAsteroid(Asteroid asteroid)
    {
        _asteroids.Remove(asteroid);
    }
    private void SpinBigAsteroid(Asteroid asteroid)
    {
        Vector2 force = -((Vector2)asteroid.transform.position).normalized * 1000.0f;
        float torque = Random.Range(500.0f, 1500.0f);

        var rb = asteroid.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Force);
        rb.AddTorque(torque, ForceMode2D.Force);

        asteroid.IsBig = true;
    }

    // Adds a random spin to small asteroids
    private void SpinSmallAsteroid(Asteroid asteroid, Vector2 force, float torque)
    {
        var rb = asteroid.GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Force);
        rb.AddTorque(torque, ForceMode2D.Force);

        asteroid.IsBig = false;
    }

    // Spawns a random amount of small asteroids when a big asteroid is destroyed.
    public void BreakUpBigAsteroid(Vector2 position)
    {
        int splintersToSpawn = Random.Range(_minAsteroidSplinters, _maxAsteroidSplinters);

        for (int counter = 0; counter < splintersToSpawn; ++counter)
        {
            float angle = counter * 360.0f / splintersToSpawn;
            Vector2 force = Quaternion.Euler(0, 0, angle) * Vector2.up * Random.Range(0.5f, 1.5f) * 300.0f;
            float torque = Random.Range(500.0f, 1500.0f);
            Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f)); // Ensure rotation only on Z-axis

            var asteroid = Instantiate(smallAsteroid, position + force.normalized * 10.0f, rotation);
            asteroid.Setup(this);
            SpinSmallAsteroid(asteroid, force, torque);

            _asteroids.Add(asteroid);
        }
    }

    // Checks whether a position is inside the screen boundaries
    private bool IsWithinScreenBoundary(Vector3 asteroidPosition)
    {
        return Mathf.Abs(asteroidPosition.x) < _screenBoundaryX && Mathf.Abs(asteroidPosition.z) < _screenBoundaryY;
    }
}

