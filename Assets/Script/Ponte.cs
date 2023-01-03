using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponte : MonoBehaviour //ponte entre o player e a código gerado
{
    //[SerializeField] Player player;
    private string direcao;
    // Start is called before the first frame update
    void Awake()
    {
        direcao = "";
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
