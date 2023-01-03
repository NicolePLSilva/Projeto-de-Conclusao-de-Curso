using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComandoSe : ComandoAbstrato
{
    private bool condicao;
    private List<bool> condicoesLista;
    private List<ComandoAbstrato> listaIf;//lista if
    private List<List<ComandoAbstrato>> listaElseIf;//lista else if
    private List<ComandoAbstrato> listaElse;//lista else

    public ComandoSe(bool c, List<bool> cl, List<ComandoAbstrato> lv, List<List<ComandoAbstrato>> lei, List<ComandoAbstrato> lf)
    {
        this.condicao = c;
        this.condicoesLista = cl;
        this.listaIf = lv;
        this.listaElseIf = lei;
        this.listaElse = lf;
        base.completo = false;
    }
    public override void Execultar()
    {
        bool exeElse = true;
        if (condicao)
        {
            foreach (ComandoAbstrato cmd in listaIf)
            {
                cmd.Execultar();
            }
        }
       
        if (listaElseIf != null)
        {

            IList list = listaElseIf;
            for (int i = 0; i < list.Count; i++)
            {
                if (condicoesLista[i])
                {
                    List<ComandoAbstrato> lcmd = listaElseIf[i];
                    //foreach (List<ComandoAbstrato> lcmd in listaElseIf)
                    //{ }
                    foreach (ComandoAbstrato cmd in lcmd)
                    {
                        cmd.Execultar();
                    }
                    exeElse = false;
                }
                
            }
        }

        if (listaElse != null)//listaElse.Count > 0
        {
            if (exeElse)
            {
                foreach (ComandoAbstrato cmd in listaElse)
                {
                    cmd.Execultar();
                }
            }

        }
        base.completo = true;
    }

    public override string ToString()
    {

        return "SeCommand [condição=" + condicao + ", lista verdadeira=" + listaIf + " , lista falsa=" + listaElse + "]";
    }
}
