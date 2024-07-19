using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Referencia al objeto jugador
    public float minX, maxX, minY, maxY; // Límites de la cámara en el eje X e Y

    void Update()
    {
        // Calcula la posición deseada de la cámara basada en la posición del jugador
        float desiredX = player.position.x;
        float desiredY = player.position.y;

        // Limita la posición de la cámara dentro de los límites definidos
        float clampedX = Mathf.Clamp(desiredX, minX, maxX);
        float clampedY = Mathf.Clamp(desiredY, minY, maxY);

        // Establece la nueva posición de la cámara
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
