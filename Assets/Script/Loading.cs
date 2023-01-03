using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private float time = 10f;
    void Start()
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
