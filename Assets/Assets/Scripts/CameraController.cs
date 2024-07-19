using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Referencia al objeto jugador
    public float minX, maxX, minY, maxY; // L�mites de la c�mara en el eje X e Y

    void Update()
    {
        // Calcula la posici�n deseada de la c�mara basada en la posici�n del jugador
        float desiredX = player.position.x;
        float desiredY = player.position.y;

        // Limita la posici�n de la c�mara dentro de los l�mites definidos
        float clampedX = Mathf.Clamp(desiredX, minX, maxX);
        float clampedY = Mathf.Clamp(desiredY, minY, maxY);

        // Establece la nueva posici�n de la c�mara
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
