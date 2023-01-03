using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabelaSimbolo 
{
    private Dictionary<string, Simbolos> dict;
    
    public TabelaSimbolo()
    {
        dict = new Dictionary<string, Simbolos>();
    }

    public void add(Simbolos simbolo)
    {
        dict[simbolo.MyName] = simbolo;
    }

    public bool exists(string nomeSimbolo)
    {
        return dict.ContainsKey(nomeSimbolo);
    }

    public Simbolos get(string nomeSimbolo)
    {
        return dict[nomeSimbolo];
    }

    
    public List<Simbolos> getTodosSimbolos()
    {
        //List<Simbolos> l = new List<Simbolos>(dict.Values) ;
        //l = dict.SelectMany(d => d.Value).ToList();
        return new List<Simbolos>(dict.Values);
    }
    
}
