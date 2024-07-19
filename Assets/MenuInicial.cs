using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class MenuInicial : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Salir  ()
    {
        Debug.Log("Salir");
        Application.Quit ();
    }
   
}
