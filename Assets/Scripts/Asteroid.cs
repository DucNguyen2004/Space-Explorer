using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private AsteroidSpawner _spawner;
    private Rigidbody2D _rb;
    public bool IsBig { get; set; }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    public void Setup(AsteroidSpawner spawner)
    {
        _spawner = spawner;
        GameController.Instance.OnSpeedUp += SpeedUp;
    }

    private void SpeedUp()
    {
        Vector2 force = -((Vector2)transform.position).normalized * 1000.0f;
        _rb.AddForce(force, ForceMode2D.Force);
    }
    public void Break()
    {
        if (IsBig)
        {
            _spawner.BreakUpBigAsteroid(transform.position);
        }

        _spawner.RemoveAsteroid(this);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameController.Instance.EndGame();
        }
    }

    private void OnDestroy()
    {
        GameController.Instance.OnSpeedUp -= SpeedUp;
    }
}
