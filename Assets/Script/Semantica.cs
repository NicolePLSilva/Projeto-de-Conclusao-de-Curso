using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Semantica : MonoBehaviour
{
    //[SerializeField] Player player;
    private string direcao;
    // Start is called before the first frame update
    void Start()
    {
        
    }

   

    public void MoverPlayer(string direcao)
    {

        this.direcao = direcao;
    }
    public string ReturnMoverPlayer()
    {

        return direcao;
    }
}
