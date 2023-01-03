using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScript : MonoBehaviour
{
   [SerializeField] private float time = 4f;
   [SerializeField] GameObject finishScreen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(ShowFinishScreenCorourine());
    }

    IEnumerator ShowFinishScreenCorourine()
    {
        finishScreen.SetActive(true);
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }
}
