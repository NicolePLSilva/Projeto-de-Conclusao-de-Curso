using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token 
{
    public static readonly int TK_IDENTIFIER  = 0;
    public static readonly int TK_NUMBER      = 1;
    public static readonly int TK_OPERATOR    = 2;
    public static readonly int TK_PONCTUATION = 3;
    public static readonly int TK_ASSIGN      = 4;
    public static readonly int TK_MATHOP      = 5;
    public static readonly int TK_RESERVED    = 6;
    public static readonly int TK_LIMITER     = 7;
    public static readonly int TK_RELATIONAL  = 8;
    public static readonly int TK_TYPE        = 9;


    public static readonly string[] TK_TEXT =
    {
        "IDENTIFIER", "NUMBER", "OPERATOR", "PONCTUACTION", "ASSIGNMENT", "MATH OPERATOR", "RESERVERD KEY WORD", "LIMITER"
    };

    private int type;
    private string text;
 

    public int GetTypeOfToken() { return type; }
    public void SetType(int newType) { type = newType; }
    public string GetText() { return text; }
    public void SetText(string newText) { text = newText; }

    public override string ToString()
    {
        return "Token[type=" + type +", text=" + text + "]";
    }


}
