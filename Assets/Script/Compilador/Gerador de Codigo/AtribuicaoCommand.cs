using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtribuicaoCommand : AbstractCommand
{
    private string id;
    private string expr;

    public AtribuicaoCommand(string id, string expr )
    {
        this.id = id;
        this.expr = expr;
    }

    public override string gerarCodigoCSharp()
    {
        if (id.Equals("string"))//verificar se o id existe na tabela de variaveis e qual o tipo dela se for string adicionar "aspas"
        {
            return id + " = \"" + expr + "\";";
        }
        return "\t" + id + " = " + expr + ";\n";
    }

    public override string ToString()
    {
        return "AtribuicaoCommand [id="+ id + " ,expr="+expr+"]" ;
    }
}
