using System.Collections;
using System.Collections.Generic;
using System.Text; //importar para o StringBuilder 
using UnityEngine;

public class RepitaCommand : AbstractCommand
{
    private string expr;
    private List<AbstractCommand> lista;

    public RepitaCommand(string expr, List<AbstractCommand> l)
    {
        this.expr = expr;
        this.lista = l; 
    }
    public override string gerarCodigoCSharp()
    {
        StringBuilder str = new StringBuilder();
        str.Append("\tfor (" + expr + ") \n\t{\n");
        foreach (AbstractCommand cmd in lista)
        {
            str.Append("\t\t" + cmd.gerarCodigoCSharp());
        }
        str.Append("\n\t}\n");
        return str.ToString();
    }

    public override string ToString()
    {
        return "RepitaCommand [expr=" + expr + "]";
    }
}
