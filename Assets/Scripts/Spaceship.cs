using UnityEngine;

public class Spaceship : MonoBehaviour
{
    // Game Session AGNOSTIC Settings
    [SerializeField] private float _rotationSpeed = 90.0f;
    [SerializeField] private float _movementSpeed = 2000.0f;
    [SerializeField] private float _maxSpeed = 200.0f;
    [SerializeField] public Laser laserPrefab;
    public Transform firePoint;

    private Rigidbody2D _rb;

    private float _screenBoundaryX;
    private float _screenBoundaryY;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _screenBoundaryX = Camera.main.orthographicSize * Camera.main.aspect;
        _screenBoundaryY = Camera.main.orthographicSize;
    }
    void Update()
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector2 input = new Vector2(moveX, moveY);
        Move(input);
        CheckExitScreen();

        // Shooting
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }
    // Moves the spaceship RB using the input for the client with InputAuthority over the object
    private void Move(Vector2 input)
    {
        float newRotation = _rb.rotation + -input.x * _rotationSpeed * Time.deltaTime;
        _rb.MoveRotation(newRotation);

        Vector2 force = transform.up * input.y * _movementSpeed * Time.deltaTime;
        _rb.AddForce(force);

        if (_rb.linearVelocity.magnitude > _maxSpeed)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * _maxSpeed;
        }
    }

    // Moves the ship to the opposite side of the screen if it exits the screen boundaries.
    private void CheckExitScreen()
    {
        var position = _rb.position;

        if (Mathf.Abs(position.x) < _screenBoundaryX && Mathf.Abs(position.y) < _screenBoundaryY) return;

        if (Mathf.Abs(position.x) > _screenBoundaryX)
        {
            position = new Vector3(-Mathf.Sign(position.x) * _screenBoundaryX, position.y, 0);
        }

        if (Mathf.Abs(position.y) > _screenBoundaryY)
        {
            position = new Vector3(position.x, -Mathf.Sign(position.y) * _screenBoundaryY, 0);
        }

        position -= position.normalized *
                    0.1f; // offset a little bit to avoid looping back & forth between the 2 edges 
        transform.position = position;
    }
    void Shoot()
    {
        Instantiate(laserPrefab, firePoint.position, Quaternion.Euler(0, 0, _rb.rotation));
    }
}
