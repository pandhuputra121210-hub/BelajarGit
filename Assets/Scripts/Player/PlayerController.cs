using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerData data;

    private float currentHP;
    private PlayerInput playerInput;
    private Vector2 moveInput;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput tidak ditemukan di Player!");
        }
    }

    void Start()
    {
        if (data != null)
        {
            currentHP = data.maxHP;
        }
        else
        {
            Debug.LogError("PlayerData belum di-assign di Inspector!");
        }
    }

    void Update()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager tidak ditemukan!");
            return;
        }

        if (GameManager.Instance.currentState != GameState.Playing) return;
        if (playerInput == null) return;
        if (data == null) return;

        var moveAction = playerInput.actions["Move"];

        if (moveAction == null)
        {
            Debug.LogError("Action 'Move' tidak ditemukan di Input System!");
            return;
        }

        moveInput = moveAction.ReadValue<Vector2>();

        float h = moveInput.x;
        float v = moveInput.y;
        transform.Translate(new Vector3(h, v, 0) * data.moveSpeed * Time.deltaTime);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            TakeDamage(0.1f * Time.deltaTime);
        }
    }

    void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        Debug.Log("Player HP: " + currentHP);

        if (currentHP <= 0)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UpdateState(GameState.GameOver);
            }
        }
    }
}