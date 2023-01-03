using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private float time = 0.5f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(LoadNextSceneCorourine());
    }

    IEnumerator LoadNextSceneCorourine()
    {
        yield return new WaitForSeconds(time);

        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
