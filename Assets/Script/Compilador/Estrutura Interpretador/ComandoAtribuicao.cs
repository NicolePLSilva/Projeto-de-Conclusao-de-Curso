using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComandoAtribuicao : ComandoAbstrato
{
    private string id;
    private string expr;

    public ComandoAtribuicao(string id, string expr)
    {
        this.id = id;
        this.expr = expr;
    }

    public override void Execultar()
    {
        id = expr;
        //return "\t" + id + " = " + expr + ";\n";
    }

    public override string ToString()
    {
        return "AtribuicaoCommand [id=" + id + " ,expr=" + expr + "]";
    }
}
