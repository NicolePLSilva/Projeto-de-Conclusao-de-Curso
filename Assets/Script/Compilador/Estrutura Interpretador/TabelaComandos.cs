using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//excluir
public class TabelaComandos 
{
    //private Dictionary<string, bool> dict;
    private List<Comandos> cmdList;

    public TabelaComandos()
    {
        //dict = new Dictionary<string, bool>();
        cmdList = new List<Comandos>();
    }

    public void add(string cmd, bool b)
    {
        Comandos c = new Comandos(cmd, b);
        // dict.Add(cmd.MyCmd, b);
        cmdList.Add(c);
    }

    public bool exists(string nomeCmd)
    {
        //return dict.ContainsKey(nomeCmd);
        //Comandos c ;
        bool exists = false;
        foreach (Comandos cmd in cmdList)
        {
            if (nomeCmd.Equals(cmd.MyCmd))
            {
                // c = new Comandos(cmd.MyCmd, cmd.Execute);
                exists = true;
            }
        }
        return exists;
    }

    public Comandos get(string nomeCmd)
    {
        return null ;
    }

    /*
    public List<Comandos> getTodosSimbolos()
    {
        //List<Simbolos> l = new List<Simbolos>(dict.Values) ;
        //l = dict.SelectMany(d => d.Value).ToList();
        //return new List<Simbolos>(dict.Values);
    }*/
}
