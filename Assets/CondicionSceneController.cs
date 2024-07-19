using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CondicionSceneController : MonoBehaviour
{
    public void IrALevel1()
    {
        SceneManager.LoadScene("Level1"); // Cargar la escena "Level"
    }

    public void Menu()
    {
        SceneManager.LoadScene("MenúInicial"); // Cargar la escena "Menu"
    }

    public void Level2()
    {
        SceneManager.LoadScene("Level2"); // Cargar la escena "Menu"
    }
}

