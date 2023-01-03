using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comandos 
{
    protected string cmd;
    protected bool exe;

    public Comandos(string cmd, bool exe)
    {
        this.cmd = cmd;
        this.exe = exe;
    }

    public bool Exe() { return false; }


    public string MyCmd
    {
        get { return this.cmd; }
        set { this.cmd = value; }
    }
    public bool Execute
    {
        get { return this.exe; }
        set { this.exe = value; }
    }

    public override string ToString()
    {
        return "Simbolos[name=" + cmd + ", execute="+ exe+ "]";
    }
}
