using System.Collections;
using System.Collections.Generic;
using System.Text; //importar para o StringBuilder 
using System.IO; //importar para o StreamWriter
using UnityEngine;

public class Gerador 
{
    private TabelaSimbolo tabelaVar;
    private List<AbstractCommand> comandos;
    private string nomeGerador;

    public void GerarObjeto()
    {
        StringBuilder str = new StringBuilder();
        str.Append("using System.Collections;\n");//"using System...."
        str.Append("using System.Collections.Generic;\n");//"using System...."
        str.Append("using UnityEngine;\n");//"using System...."
        str.Append("public class CodigoObjeto \n{\n");
        str.Append("\tprivate Ponte ponte ;\n\n");
        str.Append("\tprivate Player player ;\n\n");
        str.Append("\tpublic void Main(){\n");
        str.Append("\tponte = GameObject.Find(\"Ponte\").GetComponent<Ponte>();\n");
        str.Append("\tplayer = GameObject.Find(\"Player\").GetComponent<Player>();\n");
        //Console.Read(); ????????
        foreach (Simbolos s in tabelaVar.getTodosSimbolos())
        {
            str.Append(s.gerarCodigoCSharp()+"\n");
        }
        
        foreach (AbstractCommand command in comandos)
        {
            str.Append(command.gerarCodigoCSharp()+"\n") ;
        }
        
        str.Append("}\n");
       
        str.Append("}");

        StreamWriter sw = File.CreateText("C:\\Users\\Nicole M\\GameDev\\estudo\\Compiladores Estudo\\Assets\\Script\\Compilador\\CodigoObjeto.cs");
        sw.Write(str.ToString()) ;
        sw.Close();
    }

    public TabelaSimbolo MyTabelaVar
    {
        get { return this.tabelaVar; }
        set { this.tabelaVar = value; }
    }
    public List<AbstractCommand> MyComandos
    {
        get { return this.comandos; }
        set { this.comandos = value; }
    }
    public string MyNomegerador
    {
        get { return this.nomeGerador; }
        set { this.nomeGerador = value; }
    }

   
}
