using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComandoMova : ComandoAbstrato
{
    private string direcao;
    private Player player;
    private int passos;
    public ComandoMova(string d, int passos)
    {
        this.direcao = d;
        this.passos = passos;
        player = GameObject.Find("Player").GetComponent<Player>();
        base.completo = false;
    }
    public override void Execultar()
    {
       
        player.direction = direcao;
        player.numSteps = passos;
        player.move = true;

        base.completo = true;
    }
}
