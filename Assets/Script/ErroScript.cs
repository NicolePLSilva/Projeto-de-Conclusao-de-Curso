using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ErroScript : MonoBehaviour
{
    bool areThereSomeError;
    
    void Start()
    {
        areThereSomeError = false;
    }

    void Update()
    {
        if (areThereSomeError)
        {
           gameObject.GetComponent<TMP_Text>().enabled = true;
        }
        else 
        {
            gameObject.GetComponent<TMP_Text>().enabled = false;
        }
    }

    public void Show()
    {
        areThereSomeError = true;
    }

    public void Hide()
    {
        areThereSomeError = false;
    }
}
