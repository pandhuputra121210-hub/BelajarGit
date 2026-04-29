using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public PlayerData data;

    private float currentHP; 
    private PlayerInput playerInput;
    private Vector2 moveInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        if (data != null)
        {
            currentHP = data.maxHP;
        }
    }
    
    void Update()
    {
        if (GameManager.Instance.currentState != GameState.Playing) return;

        if (playerInput == null) return;
        
        moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
        float h = moveInput.x;
        float v = moveInput.y;

        transform.Translate(new Vector3(h, v, 0) * data.moveSpeed * Time.deltaTime);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            TakeDamage(0.1f);
        }
    }

    void TakeDamage(float dmg)
    {
        currentHP -= dmg;
        Debug.Log("Player HP: " + currentHP);

        if (currentHP <= 0)
        {
            GameManager.Instance.UpdateState(GameState.GameOver);
        }
    }
}