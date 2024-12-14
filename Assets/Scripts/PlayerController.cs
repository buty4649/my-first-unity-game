using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 4.0f;

    [SerializeField]
    private float rotationSpeed = 4.0f;

    [SerializeField]
    private float bulletSpeed = 8.0f;

    [SerializeField]
    private List<float> fireIntervals = new List<float> { 0.04f, 1f };
    private int fireIntervalsIndex = 0;
    private float fireIntervalTimer = 0f;

    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private GameObject explosionPrefab;

    [SerializeField]
    private Sprite[] spaceShipSptrites;

    private int playerIndex;
    private Vector2 moveAmount;
    private Rigidbody2D rigidbody2D;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerIndex = GetComponent<PlayerInput>().playerIndex;

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = spaceShipSptrites[playerIndex];

        Vector3 worldPosition = Camera.main.WorldToScreenPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, 1f));
        float angleZ = 0f;
        switch (playerIndex)
        {
            case 0:
                worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 4f, Screen.height / 2f, 1f));
                angleZ = -90f;
                break;
            case 1:
                worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - Screen.width / 4f, Screen.height / 2f, 1f));
                angleZ = 90f;
                break;
        }
        transform.position = worldPosition;
        transform.rotation = Quaternion.Euler(0f, 0f, angleZ);
    }


    void OnMove(InputValue value)
    {
        moveAmount = value.Get<Vector2>();
    }

    void OnFire()
    {
        if (fireIntervalTimer > 0f)
        {
            return;
        }
        fireIntervalTimer = fireIntervals[fireIntervalsIndex];
        if (++fireIntervalsIndex >= fireIntervals.Count)
        {
            fireIntervalsIndex = 0;
        }
        Bullet bullet = Instantiate<Bullet>(bulletPrefab, transform.position, transform.rotation);
        bullet.Move(transform.up * bulletSpeed, playerIndex);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null && bullet.playerIndex != playerIndex)
        {
            HudManager hudManager = GameObject.FindGameObjectWithTag("HudManager").GetComponent<HudManager>();
            hudManager.AddScoreText(bullet.playerIndex);

            Instantiate<GameObject>(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        Vector2 currentPosition = rigidbody2D.position;
        Vector2 deltaPosition = moveAmount * moveSpeed * Time.fixedDeltaTime;
        Vector2 newPosition = currentPosition + deltaPosition;
        rigidbody2D.MovePosition(newPosition);
        if (moveAmount.sqrMagnitude > 0.001f)
        {
            float angleZ = Mathf.Atan2(deltaPosition.y, deltaPosition.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angleZ);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        fireIntervalTimer -= Time.deltaTime;
    }
}
