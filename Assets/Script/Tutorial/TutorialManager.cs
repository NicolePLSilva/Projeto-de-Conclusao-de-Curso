using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public List<Tutorial> tutorials = new List<Tutorial>();

    public Text expText;

    private static TutorialManager instance;
    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<TutorialManager>();
            }
            if (instance == null)
            {
                Debug.Log("Sem TutorialManager");
            }
            return instance;
        } 
    }

    private Tutorial currentTutorial;
    void Start()
    {
        SetNextTutorial(0);
    }

    void Update()
    {
        if (currentTutorial)
        {
            currentTutorial.CheckIfHappening();
        }
    }

    public void CompletedTutorial()
    {
        SetNextTutorial(currentTutorial.order + 1);
    }
    public void SetNextTutorial(int currentOrder)
    {
        currentTutorial = GetTutorialByOrder(currentOrder);

        if (!currentTutorial)
        {

            return;
        }
        expText.text = currentTutorial.explanation;
    }

    public void CompletedAllTutorials()
    {
        expText.text = "parabéns! Você completou todos os tutoriais.";
        //leadnextscene
    }
    public Tutorial GetTutorialByOrder(int order)
    {
        for (int i = 0; i < tutorials.Count; i++)
        {
            if (tutorials[i].order == order)
            {
                return tutorials[i];
            }
        }
        return null;
    }

}
