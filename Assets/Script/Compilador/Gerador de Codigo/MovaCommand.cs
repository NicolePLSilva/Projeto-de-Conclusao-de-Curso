using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MovaCommand : AbstractCommand
{
    private string direcao;

    public MovaCommand(string d)
    {
        this.direcao = d;
    }
public override string gerarCodigoCSharp()
    {
        //StringBuilder str = new StringBuilder();
        //str.Append("ponte.MoverPlayer("+direcao+");");
        string s = "";
        switch (direcao)
        {
            case "direita":
                s = "player.direction = 0;";
                break;
            case "esquerda":
                s = "player.direction = 1;";
                break;
            case "cima":
                s = "player.direction = 2;";
                break;
            case "baixo":
                s = "player.direction = 3;";
                break;
        }
        //return "ponte.MoverPlayer(\"" + direcao + "\");";
        return s;
    }
}
