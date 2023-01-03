using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner
{
    /* TOKENS
     1 - Identificadores: 
        (a..z) (A..Z|0..9|a..z)*
     2 - Números
        (0..9)+
     3 - Pontuação
        ;
     4 - Operadores relacionais
        > | >= | < | <= | == | !=
     5 - Operadores atribuição
        =
    Obs: a..z = intervalo da letra 'a' a letra 'z'
    '*' = 0 ou mais repetições
    '+' = 1 ou mais repetições
     */

    private char[] content;
    private int estado;
    private int pos;

    private int numEspacos;
    private int tamanhoToken;
    private string lastToken;
    private char lastChar;

    private bool end;
    
    public void StartScanner(string filename)
    {
        string txtConteudo;
        txtConteudo = filename;
        Debug.Log("DEBUG---------");
        Debug.Log(txtConteudo);
        Debug.Log("DEBUG---------");

        content = txtConteudo.ToCharArray();

        pos = 0;

        numEspacos = 0;
        tamanhoToken = 0;

        end = false;

    }
    string term;

    public Token NextToken()
    {
       
        Token token ;
        term = "";
        char currentChar;
       
        if (IsEOF())
        {
            end = true;
            return null;
        }

        estado = 0;
        while (true)
        {
            
         
            currentChar = NextChar();
            switch (estado)
            {
                case 0: // estado inicial e espaços
                    if (IsSpace(currentChar))
                    {

                        estado = 0;
                    }
                    else if (IsChar(currentChar))
                    {
                        term += currentChar;
                        estado = 1;
                    }
                    else if (IsDigit(currentChar))
                    {
                        term += currentChar;
                        estado = 3;
                    }
                    else if (currentChar.Equals('='))
                    {
                        term += currentChar;
                        estado = 5;
                    }
                    else if (IsOperator(currentChar) && !(currentChar.Equals('=')))
                    {
                        term += currentChar;
                        estado = 8;
                    }
                    else if (IsMath(currentChar))
                    {
                        term += currentChar;
                        //estado = 9;
                        token = new Token();
                        token.SetType(Token.TK_MATHOP);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                    }
                    else if (IsLimiter(currentChar))
                    {
                        term += currentChar;
                        //estado = 10;
                        token = new Token();
                        token.SetType(Token.TK_LIMITER);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                    }
                    else if (IsPontuaction(currentChar))
                    {
                        term += currentChar;
                        //estado = 12;
                        token = new Token();
                        token.SetType(Token.TK_PONCTUATION);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                    }
                    else
                    {
                        Debug.LogError("<color=red>Simbolo Não Identificado!</color>( " + currentChar + " )");
                        return null;
                    }
                    break;
                case 1: // permanece até != a..z ou !=0..9
                    if (IsChar(currentChar) || IsDigit(currentChar))
                    {
                        term += currentChar;
                        estado = 1;

                    }
                    else if (!(IsChar(currentChar) || IsDigit(currentChar)) && IsReservedWord(term))
                    {
                        //estado = 11;
                        Back();
                        token = new Token();
                        token.SetType(Token.TK_RESERVED);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                    }
                    else if (!(IsChar(currentChar) || IsDigit(currentChar)) && IsType(term))
                    {
                        Back();
                        token = new Token();
                        token.SetType(Token.TK_TYPE);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                    }
                    else if (IsOperator(currentChar) || IsMath(currentChar) || IsPontuaction(currentChar) || IsLimiter(currentChar) || IsSpace(currentChar))
                    {
                        Back();
                        token = new Token();
                        token.SetType(Token.TK_IDENTIFIER);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                        //estado = 2;
                    }
                    else
                    {
                        Debug.LogError("<color=red>ID Não Identificado!</color>( " + currentChar + " )");
                        return null;
                    }
                    break;
                case 2: // token id e back
                    Back();
                    token = new Token();
                    token.SetType(Token.TK_IDENTIFIER);
                    token.SetText(term);
                    lastToken = term;
                    return token;
                case 3: // permanece até != 0..9
                    if (IsDigit(currentChar) || currentChar.Equals('.'))
                    {
                        term += currentChar;
                        estado = 3;
                    }
                    else if (IsOperator(currentChar) || IsMath(currentChar) || IsPontuaction(currentChar) || IsLimiter(currentChar) || IsSpace(currentChar))
                    {
                        Back();
                        token = new Token();
                        token.SetType(Token.TK_NUMBER);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                        //estado = 4;
                    }
                    else
                    {
                        Debug.LogError("<color=red>Numero Não Identificado!</color>( " + currentChar + " )");
                        return null;
                    }
                    break;
                case 4: // token numero e back
                    Back();
                    token = new Token();
                    token.SetType(Token.TK_NUMBER);
                    token.SetText(term);
                    lastToken = term;
                    return token;
                case 5: // se next char = a..z ou 0..9 chama estado 6
                    if (IsChar(currentChar) || IsDigit(currentChar) || IsSpace(currentChar))
                    {
                        //estado = 6;
                        Back();
                        token = new Token();
                        token.SetType(Token.TK_ASSIGN);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                    }
                    else if (currentChar.Equals('='))
                    {
                        term += currentChar;
                        //estado = 7;
                        token = new Token();
                        token.SetType(Token.TK_RELATIONAL);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                    }
                    else
                    {
                        Debug.LogError("<color=red>Insira Simbolo Valido após o \"=\"!</color>Simbolo Atual:( " + currentChar + " )");
                        return null;
                    }
                case 6: // token atribuição e back
                    Back();
                    token = new Token();
                    token.SetType(Token.TK_ASSIGN);
                    token.SetText(term);
                    lastToken = term;
                    return token;
                case 7: // token operador relacional 
                    token = new Token();
                    token.SetType(Token.TK_RELATIONAL);
                    token.SetText(term);
                    lastToken = term;
                    return token;
                case 8: // verifica char após operador se achar outro vai pro estado 7
                    if (IsOperator(currentChar))
                    {
                        term += currentChar;
                        //estado = 7;
                        token = new Token();
                        token.SetType(Token.TK_RELATIONAL);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                    }
                    else if (!(IsOperator(currentChar)) && (term.Equals("<") || term.Equals(">")))
                    {
                        token = new Token();
                        token.SetType(Token.TK_RELATIONAL);
                        token.SetText(term);
                        lastToken = term;
                        return token;
                    }
                    else if (!(IsOperator(currentChar)))
                    {
                        estado = 13;
                    }
                    else
                    {
                        Debug.LogError("<color=red>Insira Simbolo Valido após o Operador!</color>Simbolo Atual:( " + currentChar + " )");
                        return null;
                    }
                    break;
                case 9: // token operador matematico
                    token = new Token();
                    token.SetType(Token.TK_MATHOP);
                    token.SetText(term);
                    lastToken = term;
                    return token;
                case 10: // token limitador
                    token = new Token();
                    token.SetType(Token.TK_LIMITER);
                    token.SetText(term);
                    lastToken = term;
                    return token;
                case 11: // token palavra reservada
                    Back();
                    token = new Token();
                    token.SetType(Token.TK_RESERVED);
                    token.SetText(term);
                    lastToken = term;
                    return token;
                case 12: // token pontuaction 
                    token = new Token();
                    token.SetType(Token.TK_PONCTUATION);
                    token.SetText(term);
                    lastToken = term;
                    return token;
                case 13: // token operador
                    Back();
                    token = new Token();
                    token.SetType(Token.TK_OPERATOR);
                    token.SetText(term);
                    lastToken = term;
                    return token;
            }

        }

    }

    private bool IsDigit(char c)
    {
        return c >= '0' && c <= '9';
    }

    private bool IsChar(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }

    private bool IsOperator(char c)
    {
        return c == '>' || c == '<' || c == '=' || c == '!';
    }

    private bool IsSpace(char c)
    {
        return c == ' ' || c == '\t' || c == '\n' || c == '\r';
    }

    //-----------------
    private bool IsMath(char c)
    {
        return c == '+' | c == '-' | c == '/' | c == '*';
    }

    private bool IsReservedWord(string s)
    {
        return s == "mova" | s == "se" | s == "entao" | s == "senao" | s == "repita" | s == "direita" | s == "esquerda" | s == "cima" | s == "baixo";
        //| s == "inteiro" | s == "texto";

    }

    private bool IsLimiter(char c)
    {
        return c == '(' | c == ')' | c == '{' | c == '}';
    }

    private bool IsPontuaction(char c)
    {
        return c == ';' | c == ',';
    }

    private bool IsType(string s)
    {
        return s == "inteiro" | s == "texto";
    }

    //-----------------------
    private char NextChar()
    {
            char c = content[pos];
            pos++;
            return c;
            // if (content[pos] > content.Length)
            // {
            //     return false;
            // }
            // else
            // {
            //     currentChar = content[pos];
            //     pos++;
            //     return true;
            // }
            
        /*
        if (pos+1 > content.Length) { return ' '; }
        return content[pos++];
        */
    }


    private bool IsEOF()
    {
        return pos == content.Length;
    }

    private void Back()
    {
        pos--;
    }
    public int GetPos()
    {
        return pos;
    }
    public void SetPos(int p)
    {
        pos = p;
    }

    public void BackToken(int numBack)
    {
        //numBack = numEspaco + tamanhoToken
        while (numBack > 0) { Back(); numBack--; }

    }


    public bool IsEnd()
    {
        //return end;
        return content.Length == pos;
    }


}
