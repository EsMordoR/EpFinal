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
    // Tama�o del �rea de persecuci�n (en lugar de distancia m�nima)
    [SerializeField] private Vector2 chaseAreaSize;
    // Capa del jugador
    [SerializeField] private LayerMask playerLayer;

    // Transform del jugador
    private Transform player;
    // �ndice del punto de patrulla actual
    private int currentPatrolIndex;
    // Indicador de si el enemigo est� persiguiendo al jugador
    private bool chasingPlayer;
    // Indicador de si el enemigo est� muerto
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

        // Si no est� persiguiendo al jugador, realizar patrulla
        if (!chasingPlayer)
        {
            Patrol();
        }
        else // Si est� persiguiendo al jugador, seguirlo
        {
            ChasePlayer();
        }
    }

    // Realizar la patrulla entre los puntos designados
    private void Patrol()
    {
        // Obtener la posici�n del pr�ximo punto de patrulla
        Vector2 targetPosition = patrolPoints[currentPatrolIndex].position;

        // Moverse hacia el pr�ximo punto de patrulla
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, enemySpeed * Time.deltaTime);

        // Si se alcanza el punto de patrulla, pasar al siguiente
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            Girar();
        }

        // Si el jugador est� dentro del rango de persecuci�n, comenzar a perseguirlo
        if (IsPlayerInChaseArea())
        {
            chasingPlayer = true;
        }
    }

    // Perseguir al jugador
    public virtual void ChasePlayer()
    {
        // Moverse hacia la posici�n del jugador
        transform.position = Vector2.MoveTowards(transform.position, player.position, enemySpeed * Time.deltaTime);

        // Si el jugador est� fuera del rango de persecuci�n, dejar de perseguirlo
        if (!IsPlayerInChaseArea())
        {
            chasingPlayer = false;
        }
    }

    // Verificar si el jugador est� dentro del �rea de persecuci�n
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

    // Dibujar un gizmo para representar el rango de persecuci�n en el editor
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

    public void TomarDa�o(float da�o)
    {
        vida -= da�o;

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
