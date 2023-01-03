using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Simbolos 
{
    protected string name;

    public Simbolos(string name)
    {
        this.name = name;
    }

    public abstract string gerarCodigoCSharp();


    public string MyName
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public override string ToString()
    {
        return "Simbolos[name=" + name + "]";
    }
}
