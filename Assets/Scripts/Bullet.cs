using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    public int playerIndex { get; set;  }

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 velocity, int playerIndex)
    {
        rigidbody2D.linearVelocity = velocity;
        this.playerIndex = playerIndex;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < 0f || screenPosition.x > Screen.width || screenPosition.y < 0f || screenPosition.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }
}
