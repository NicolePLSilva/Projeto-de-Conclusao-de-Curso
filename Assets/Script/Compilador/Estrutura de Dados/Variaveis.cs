using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variaveis : Simbolos
{
    public static readonly int INTEIRO = 0;
    public static readonly int TEXTO = 1;

    private int type;
    private string id;

    public Variaveis(string name, int type, string id) 
        : base(name)
    {
        this.type = type;
        this.id = id;
    }

    public override string gerarCodigoCSharp()
    {
        string str;
        if (type == INTEIRO)
        {
            str = "int";
        }
        else
        {
            str = "string";
        }
        return str + " "+base.name+";";
    }

    public int MyType 
    {
        get { return this.type; }
        set { this.type = value; }
    }

    public string MyID
    {
        get { return this.id; }
        set { this.id = value; }
    }

   

    public override string ToString()
    {
        return "Simbolos[name=" + name + ", type="+type+", id="+id+"]";
    }


}
