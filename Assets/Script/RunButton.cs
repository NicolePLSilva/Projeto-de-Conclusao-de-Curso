using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunButton : MonoBehaviour 
{
    private string code;
    [SerializeField] private GameObject inputField;
    [SerializeField] private Main main;

    private CodigoObjeto co;

    public void StoreCode()
    {
        co = new CodigoObjeto();
        //Ponte p = new Ponte();
        code = inputField.GetComponent<Text>().text;
        //Debug.Log(code);
        main.PutCodeInsideScanner(code);

        
        //co.Main();

    }


    
}
