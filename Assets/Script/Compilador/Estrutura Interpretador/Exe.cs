using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exe : MonoBehaviour
{
    private TabelaSimbolo tabelaVar;
    private List<ComandoAbstrato> comandos;
    private string nomeGerador;

    public void ExecultarComandos()
    {
        /*
        foreach (ComandoAbstrato command in comandos)
        {

            command.Execultar();
        }*/
        
        StartCoroutine(ExeCoroutine());
    }

    IEnumerator ExeCoroutine()
    {
        foreach (ComandoAbstrato command in comandos)
        {

            command.Execultar();
            yield return new WaitForSeconds(1);
           
        }

    }

    public TabelaSimbolo MyTabelaVar
    {
        get { return this.tabelaVar; }
        set { this.tabelaVar = value; }
    }
    public List<ComandoAbstrato> MyComandos
    {
        get { return this.comandos; }
        set { this.comandos = value; }
    }
    public string MyNomegerador
    {
        get { return this.nomeGerador; }
        set { this.nomeGerador = value; }
    }

}
