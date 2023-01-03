using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComandoRepita : ComandoAbstrato
{
    private string expr;
    private List<ComandoAbstrato> lista;
    private int numLoop;
    
    public ComandoRepita(string expr, List<ComandoAbstrato> l, int numLoop)
    {
        this.expr = expr;
        this.lista = l;
        this.numLoop = numLoop;
        base.completo = false;
    }

    

    public override void Execultar()
    {
        //StringBuilder str = new StringBuilder();
        //str.Append("\tfor (" + expr + ") \n\t{\n");
        for (int i = 0; i < numLoop; i++)
        {
            foreach (ComandoAbstrato cmd in lista)
            {

                cmd.Execultar();
            }
        }

        base.completo = true;
        
    }

   

    public override string ToString()
    {
        return "RepitaCommand [expr=" + expr + "]";
    }
}
