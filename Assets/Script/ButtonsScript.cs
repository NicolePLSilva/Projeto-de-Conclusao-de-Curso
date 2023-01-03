using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonsScript : MonoBehaviour
{
    private void Update()
    {
        #if UNITY_STANDALONE
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        #endif
    }
   
    public void BackToStart()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CommandList()
    {
        Debug.Log("exibir lista de comandos");
    }

    public void QuitGame()
    {
        #if UNITY_STANDALONE
            Application.Quit();
        #endif

        #if UNITY_WEBGL	
            Application.OpenURL("https://pls-nick.itch.io");
        #endif
    
    }
}
