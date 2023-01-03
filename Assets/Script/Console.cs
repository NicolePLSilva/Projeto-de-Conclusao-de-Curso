using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{

    private string code { get; set; }
    [SerializeField] private GameObject inputField;


    public void StoreCode()
    {
        code = inputField.GetComponent<Text>().text;
    }

}
