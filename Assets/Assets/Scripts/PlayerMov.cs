using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMov : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxJumps = 2;
    public int vida = 100; // Ejemplo de vida inicial

    private Rigidbody2D rb;
    private bool isGrounded;
    private int jumpCount;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetButtonDown("Vertical")) && jumpCount < maxJumps && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            jumpCount = 0; // Reiniciar el contador de saltos cuando se toca el suelo
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = false;
        }
    }

    // Método para restar vida al jugador
    public void DamagePlayer(int damageAmount)
    {
        vida -= damageAmount;

        // Comprobar si el jugador está vivo
        if (vida <= 0)
        {
            // Cargar la escena de Perdiste
            SceneManager.LoadScene("Loser");
        }
    }
}
