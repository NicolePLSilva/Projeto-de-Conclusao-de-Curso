using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] ErroScript es;
    Scanner scanner;
    Parser parser;
    Token token;
    ParserInterpreter interpreter;

    public void PutCodeInsideScanner(string s)
    {
        scanner = new Scanner();
      
        scanner.StartScanner(s);
        
        parser = new Parser();
        parser.IsParser(scanner);
       
        
        interpreter = new ParserInterpreter();
        interpreter.IsParser(scanner);

        if (!interpreter.INICIO())
        {
            es.Show();
        }
        else
        {
            es.Hide();
            interpreter.ExecultarCodigo();
        }
        
    }

}
