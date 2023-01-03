using System.Collections;
using System.Collections.Generic;
using System.Text; //importar para o StringBuilder 
using UnityEngine;

public class SeCommand : AbstractCommand
{
    private string condicao;
    private List<string> condicoesLista;
    private List<AbstractCommand> listaIf;//lista if
    private List<List<AbstractCommand>> listaElseIf;//lista else if
    private List<AbstractCommand> listaElse;//lista else

    public SeCommand(string c, List<string> cl, List<AbstractCommand> lv, List<List<AbstractCommand>> lei, List<AbstractCommand>lf)
    {
        this.condicao = c;
        this.condicoesLista = cl;
        this.listaIf = lv;
        this.listaElseIf = lei;
        this.listaElse = lf;
    }
    public override string gerarCodigoCSharp()
    {
        StringBuilder str = new StringBuilder();
        str.Append("\tif (" + condicao + ")\n\t{\n");
        foreach (AbstractCommand cmd in listaIf)
        {
            str.Append("\t\t"+cmd.gerarCodigoCSharp());
        }
        str.Append("\n\t}\n");

        if (listaElseIf != null)
        {
            
            IList list = listaElseIf;
            for (int i = 0; i < list.Count; i++)
            {
                //List<List<AbstractCommand>> ll = (List<List<AbstractCommand>>)list[i];
                str.Append("\telse if ("+condicoesLista[i]+")\n\t{\n");
                foreach (List<AbstractCommand> lcmd in listaElseIf)
                {
                    foreach (AbstractCommand cmd in lcmd)
                    {
                        str.Append("\t\t" + cmd.gerarCodigoCSharp());
                    }
                }
                str.Append("\n\t}\n");
            }
            
        }

        if (listaElse != null)//listaElse.Count > 0
        {
            str.Append("\telse \n\t{\n");
            foreach(AbstractCommand cmd in listaElse)
            {
                str.Append("\t\t" + cmd.gerarCodigoCSharp());
            }
            str.Append("\n\t}\n");
        }
        
        return str.ToString() ;
    }

    public override string ToString()
    {
       
        return "SeCommand [condição="+condicao+", lista verdadeira="+listaIf+" , lista falsa="+ listaElse+"]";
    }
}
