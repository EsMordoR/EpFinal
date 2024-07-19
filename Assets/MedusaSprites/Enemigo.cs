using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemigo : MonoBehaviour
{
    Animator animator;
    public float vida;
    public int damage;
    // Velocidad del enemigo
    [SerializeField] private float enemySpeed;
    // Puntos de patrulla del enemigo
    [SerializeField] private Transform[] patrolPoints;
    // Tamaño del área de persecución (en lugar de distancia mínima)
    [SerializeField] private Vector2 chaseAreaSize;
    // Capa del jugador
    [SerializeField] private LayerMask playerLayer;

    // Transform del jugador
    private Transform player;
    // Índice del punto de patrulla actual
    private int currentPatrolIndex;
    // Indicador de si el enemigo está persiguiendo al jugador
    private bool chasingPlayer;
    // Indicador de si el enemigo está muerto
    private bool isDead;
    public SpriteRenderer render;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Encontrar el jugador al inicio
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        // Iniciar la patrulla
        Patrol();
    }

    void Update()
    {
        if (isDead)
        {
            return;
        }

        // Si no está persiguiendo al jugador, realizar patrulla
        if (!chasingPlayer)
        {
            Patrol();
        }
        else // Si está persiguiendo al jugador, seguirlo
        {
            ChasePlayer();
        }
    }

    // Realizar la patrulla entre los puntos designados
    private void Patrol()
    {
        // Obtener la posición del próximo punto de patrulla
        Vector2 targetPosition = patrolPoints[currentPatrolIndex].position;

        // Moverse hacia el próximo punto de patrulla
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemySpeed * Time.deltaTime);

        // Si se alcanza el punto de patrulla, pasar al siguiente
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            Girar();
        }

        // Si el jugador está dentro del rango de persecución, comenzar a perseguirlo
        if (IsPlayerInChaseArea())
        {
            chasingPlayer = true;
        }
    }

    // Perseguir al jugador
    public virtual void ChasePlayer()
    {
        // Moverse hacia la posición del jugador
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemySpeed * Time.deltaTime);

        // Si el jugador está fuera del rango de persecución, dejar de perseguirlo
        if (!IsPlayerInChaseArea())
        {
            chasingPlayer = false;
        }
    }

    // Verificar si el jugador está dentro del área de persecución
    private bool IsPlayerInChaseArea()
    {
        Vector2 enemyPosition = transform.position;
        Vector2 playerPosition = player.position;
        Vector2 chaseAreaHalfSize = chaseAreaSize / 2;

        return playerPosition.x >= enemyPosition.x - chaseAreaHalfSize.x &&
               playerPosition.x <= enemyPosition.x + chaseAreaHalfSize.x &&
               playerPosition.y >= enemyPosition.y - chaseAreaHalfSize.y &&
               playerPosition.y <= enemyPosition.y + chaseAreaHalfSize.y;
    }

    // Dibujar un gizmo para representar el rango de persecución en el editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 position = transform.position;
        Vector3 size = new Vector3(chaseAreaSize.x, chaseAreaSize.y, 0);
        Gizmos.DrawWireCube(position, size);
    }

    private void Girar()
    {
        if (transform.position.x <= patrolPoints[currentPatrolIndex].position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    public void TomarDaño(float daño)
    {
        vida -= daño;

        if (vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        isDead = true;
        animator.SetTrigger("Muerte");
        Invoke("destruir", 1f);
    }

    private void destruir()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out vidajugador vidaJugador))    //sistema de quitar vida al hacer colision
        {
            vidaJugador.TomarDamage(damage);
            Destroy(gameObject);
        }

    }
}
