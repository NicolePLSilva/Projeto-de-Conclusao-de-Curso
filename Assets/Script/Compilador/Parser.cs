using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser 
{
    private Scanner scanner;
    private Token token;
    private int posAtual;
    private int posInicial;

    private bool cmdinterno;

    private Semantica semantica ;

    private int checkpointPos;

    private int _tipo;
    private string _varName;
    private string _varValue;
    private TabelaSimbolo _tabelaSimbolo = new TabelaSimbolo();
    private Simbolos _simbolo;

    private Gerador _gerador = new Gerador();
    private List<AbstractCommand> _mainThread = new List<AbstractCommand>();

    private string _exprLida;
    private string _exprID;
    private string _exprContent;

    private Stack<List<AbstractCommand>> _stackCommands = new Stack<List<AbstractCommand>>();

    private string _exprDecisao;
    private List<AbstractCommand> _listaIf; //lista else if
    private List<List<AbstractCommand>> _listaElif = new List<List<AbstractCommand>>();//lista de listas else if
    private List<AbstractCommand> l; // lista de comandos do else if
    private List<AbstractCommand> _listaElse;//lista else

    private List<AbstractCommand> _listaLoop;
    private string _exprLoop;
    public void IsParser(Scanner scanner)
    {
        this.scanner = scanner;
        //semantica = GameObject.Find("Semantica").GetComponent<Semantica>();

    }

    public bool AddSimbolo()
    {
        _varName = token.GetText();
        _varValue = null;
        _simbolo = new Variaveis(_varName, _tipo, _varValue);
        
        if (!_tabelaSimbolo.exists(_varName))
        {
            _tabelaSimbolo.add(_simbolo);
            Debug.Log("SIMBOLO ADICIONADO!" + _simbolo);
            return true;
        }
        else
        {
            Debug.LogError("Variável" + _varName + " já declarada!");
            return false;
        }  
    }

    public bool ChecarSimbolo(string varname)
    {
        if (!_tabelaSimbolo.exists(varname))
        {
            Debug.LogError("Variável" + _varName + " ainda não declarada!");
            return false;
        }
        return true;
    }

    public void GerarCodigo()
    {
        _gerador.GerarObjeto();
    }

    public void ExibeComandos()
    {
        foreach (AbstractCommand c in _gerador.MyComandos)
        {
            Debug.Log("<color=yellow>"+c+"</color>");
        }
    }
    
    //INICIO  -> CMD METODO INICIO
    public bool INICIO(){


        cmdinterno = false;
        
        if (!CMD()) { return false; }

        //----------_gerador.SetComandos(_mainThread);
        _gerador.MyTabelaVar = _tabelaSimbolo;
        _gerador.MyComandos = _stackCommands.Pop();
        ExibeComandos();
        //Debug.Log(_stackCommands.Peek());
        //------------------------------------
        return true;
    }

    //CMD     -> CMDATRIB | CMDREPITA | CMDSE
    public bool CMD() {
        
        posInicial = scanner.GetPos();
        if (!(scanner.IsEnd()) && ID()) // da um next token, verifica se é id
        {
            posAtual = scanner.GetPos();
            if (AP())// verifica se o token seguinte ao id é um limiter
            {
                posAtual = scanner.GetPos();
                scanner.BackToken(posAtual - posInicial);
                if (!(METODO())) { return false; }
            }
            else
            {
                scanner.SetPos(posAtual);
                if (ATRIB())
                {
                    posAtual = scanner.GetPos();
                    scanner.BackToken(posAtual - posInicial);
                    if (!(CMDATRIB())) { return false; }
                }
            }
   
        }else
        {
            scanner.SetPos(posInicial);
            if (!(scanner.IsEnd()) && REPITA())// REPITA
            {
                posAtual = scanner.GetPos();
                scanner.BackToken(posAtual - posInicial);
                if (!(CMDREPITA())) { return false; }
               
            }//se não for repita, preciso voltar pos inicial e checar o se()
            else
            {
                scanner.SetPos(posInicial);
                if (!(scanner.IsEnd()) && SE())// SE
                {
                    posAtual = scanner.GetPos();
                    scanner.BackToken(posAtual - posInicial);
                    if (!(CMDSE())) { return false; }

                }//se não for se, preciso voltar pos inicial e checar o mova()
                else
                {
                    scanner.SetPos(posInicial);
                    if (!(scanner.IsEnd()) && MOVA()) 
                    {
                        posAtual = scanner.GetPos();
                        scanner.BackToken(posAtual - posInicial);
                        if (!(CMDMOVA())) { return false; }

                    }//se não for mova, preciso voltar pos inicial e checar o tipo()
                    else
                    {

                        scanner.SetPos(posInicial);
                        if (!(scanner.IsEnd()) && TIPO())
                        {
                            posAtual = scanner.GetPos();
                            scanner.BackToken(posAtual - posInicial);
                            if (!(DECLARACAO())) { return false; }

                        }
                        else
                        {

                            scanner.SetPos(posInicial);
                            //Debug.LogError("CMD NÃO RECONHECIDO");
                            return false;
                        }
                    }
                }
                
            }
        }
        
        if(cmdinterno)//se cmd for chamado de outro metodo então retomar
        {
            return true;
        }
        else
        {
            if (!(scanner.IsEnd())) 
            { 
                CMD();
            }
            
        }
        //senao fazer uma recursão CMD();
        //CMD();
        return true;
     
    }

    public bool DECLARACAO()
    {
        if (!(DECLARAVAR())) { Debug.LogError("DECLARACAO - DECLARAVAR()"); return false; }
        return true;
    }

    //DECLARAVAR -> TIPO ID () PV
    public bool DECLARAVAR()
    {
       
        if (!(TIPO())) { Debug.LogError("DECLARAVAR - TIPO()"); return false; }
        if (!(ID())) { Debug.LogError("DECLARAVAR - ID()"); return false; }
        //-------------------------
        if (!(AddSimbolo())) { Debug.LogError("DECLARAVAR - ADDSIMBOLO()"); return false; }
       //-------------------------
        checkpointPos = scanner.GetPos();
        do 
        {
            if (VIRGULA())
            {
                if (!(ID())) { Debug.LogError("DECLARAVAR - VIRGULA() - ID()"); return false; }
                //-------------------------
                AddSimbolo();
                //-------------------------   
                checkpointPos = scanner.GetPos();
            }
            else
            {
                scanner.SetPos(checkpointPos);
                break;
            }
        } while (true);
        
        if (!(PV())) { Debug.LogError("DECLARAVAR - PV()"); return false; }
        Debug.Log("DECLARAVAR OK");
        return true;
    }

    public bool TIPO()
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_TYPE)
        {
            return false;
        }
        if (token.GetText().Equals("inteiro"))
        {
            _tipo = Variaveis.INTEIRO;
        }
        else if (token.GetText().Equals("texto"))
        {
            _tipo = Variaveis.TEXTO;
        }
        return true;
    }

    //METODO  -> id AP EXPR FP
    public bool METODO() {
        if (!(ID())) { Debug.LogError("METODO - ID()"); return false; }
        if (!(AP())) { Debug.LogError("METODO - AP()"); return false; }
        if (!(EXPR())) { Debug.LogError("METODO - EXPR()"); return false; }
        if (!(FP())) { Debug.LogError("METODO - FP()"); return false; }
        if (!(PV())) { Debug.LogError("METODO - PV()"); return false; }
        Debug.Log("METODO OK");
        return true;
    }

    //CMDATRIB-> id ATRIB EXPR PV
    public bool CMDATRIB() {
        if (!(ID())) { Debug.LogError("CMDATRIB - ID()"); return false; }
        //----------------------------------------
        if (!ChecarSimbolo(token.GetText())) { return false; }
        _exprID = token.GetText();
        //-----------------------------------
        if (!(ATRIB())) { Debug.LogError("CMDATRIB - ID()"); return false; }
        //-----------------------------------------------
        _exprContent = "";
        //-----------------------------------------------
        if (!(EXPR())) { Debug.LogError("CMDATRIB - EXPR()"); return false; }
        if (!(PV())) { Debug.LogError("CMDATRIB - PV()"); return false; }
        Debug.Log("CMDATRIB OK");
        AtribuicaoCommand cmd = new AtribuicaoCommand(_exprID, _exprContent);
        _stackCommands.Push(_mainThread);
        _stackCommands.Peek().Add(cmd);
        return true;
       
    }

    //CMDREPITA-> "Repita" AP EXPR FP BLOCO
    public bool CMDREPITA() {
        if (!(REPITA())) { Debug.LogError("REPITA - REPITA()"); return false; }
        if (!(AP())) { Debug.LogError("REPITA - AP()"); return false; }
        if (!(EXPRREPITA())) { Debug.LogError("REPITA - EXPR()"); return false; }
        Debug.Log("exprLida="+token.GetText());
        if (!(FP())) { Debug.LogError("REPITA - FP()"); return false; }
        if (!(ACH())) { Debug.LogError("CMDREPITA - ACH()"); return false; }
        cmdinterno = true;
        _mainThread = new List<AbstractCommand>();
        _stackCommands.Push(_mainThread);
        if (!(BLOCO())) { Debug.LogError("CMDREPITA - BLOCO()"); return false; }
        cmdinterno = false;
        if (!(FCH())) { Debug.LogError("CMDREPITA - FCH()"); return false; }
        Debug.Log("CMDREPITA OK");
        //----adiciona o RepitaCommand na lista de comandos ----
        _listaLoop = _stackCommands.Pop();
        RepitaCommand cmd = new RepitaCommand(_exprLoop, _listaLoop);
        _mainThread = new List<AbstractCommand>();
        _stackCommands.Push(_mainThread);
        _stackCommands.Peek().Add(cmd);
        //------------------------------------------------------
        
        
        return true;
    }

    public bool EXPRREPITA()
    {
        _exprLoop = "";
        
        //i = 0;  i < 9; i++
        if (!ID()) { Debug.LogError("EXPRREPITA - ATRIB()"); return false; }
        _exprLoop = token.GetText();
        if (!ATRIB()) { Debug.LogError("EXPRREPITA - ATRIB()"); return false; }
        _exprLoop += token.GetText();
        if (!NUMBER()) { Debug.LogError("EXPRREPITA - NUMBER()"); return false; }
        _exprLoop += token.GetText();
        if (!PV()) { Debug.LogError("EXPRREPITA - PV()"); return false; }
        _exprLoop += token.GetText();
        if (!ID()) { Debug.LogError("EXPRREPITA - ID()2"); return false; }
        _exprLoop += token.GetText();
        if (!OPREL()) { Debug.LogError("EXPRREPITA - OPREL()"); return false; }
        _exprLoop += token.GetText();
        checkpointPos = scanner.GetPos();
        if (!ID() ) {
            scanner.SetPos(checkpointPos);
            if (!NUMBER()) { Debug.LogError("EXPRREPITA - NUMBER()2"); return false; }
            //return false; 
        }
        _exprLoop += token.GetText();
        if (!PV()) { Debug.LogError("EXPRREPITA - PV()2"); return false; }
        _exprLoop += token.GetText();
        if (!ID()) { Debug.LogError("EXPRREPITA - ID()3"); return false; }
        _exprLoop += token.GetText();
        if (!MATHOP()) { Debug.LogError("EXPRREPITA - MATHOP()3"); return false; }
        _exprLoop += token.GetText();
        if (!MATHOP()) { Debug.LogError("EXPRREPITA - MATHOP()4"); return false; }
        _exprLoop += token.GetText();
        return true;
    }

    //CMDSE -> SE AP COND FP ACH (CMD)* FCH (ELSEIF AP COND FP ACH (CMD)* FCH)? (ELSE ACH (CMD)+ FCH)?;
    public bool CMDSE() {
       
        
        if (!(SE())) { Debug.LogError("CMDSE - SE()"); return false; }
        if (!(AP())) { Debug.LogError("CMDSE - AP()"); return false; }
        if (!(COND())) { Debug.LogError("CMDSE - COND()"); return false; }
        //--------------------------------------
        string condIf = _exprDecisao;
        //--------------------------------------
        if (!(FP())) { Debug.LogError("CMDSE - FP()"); return false; }
        if (!(ACH())) { Debug.LogError("CMDSE - ACH()"); return false; }
        checkpointPos = scanner.GetPos();
        cmdinterno = true;
        _mainThread = new List<AbstractCommand>();
        _stackCommands.Push(_mainThread);
       
        if (!(BLOCO())) { Debug.LogError("CMDSE - BLOCO()"); return false; }
        cmdinterno = false;
        if (!(FCH())) { Debug.LogError("CMDSE - FCH()"); return false; }
        _listaIf = _stackCommands.Pop();
        checkpointPos = scanner.GetPos();

        List<string> cond = new List<string>();
        l = new List<AbstractCommand>();
        do
        {
            if (!scanner.IsEnd() && SENAO())
            {
                if (!(AP())) { Debug.LogError("CMDSE -SENAO - SE()"); return false; }
                if (!(COND())) { Debug.LogError("CMDSE -SENAO - AP()"); return false; }
                //------------------------------
                cond.Add(_exprDecisao);
                //------------------------------
                if (!(FP())) { Debug.LogError("CMDSE -SENAO - FP()"); return false; }
                if (!(ACH())) { Debug.LogError("CMDSE -SENAO - ACH()"); return false; }
                cmdinterno = true;
                //--------------------------------------------
                _mainThread = new List<AbstractCommand>();
                _stackCommands.Push(_mainThread);
                //-------------------------------------------
                if (!(BLOCO())) { Debug.LogError("CMDSE - BLOCO()"); return false; }

                cmdinterno = false;
                if (!(FCH())) { Debug.LogError("CMDSE -SENAO - FCH()"); return false; }
                //-------------------------------------------
                 
                 l = _stackCommands.Pop();
                //_listaElif = new List<List<AbstractCommand>>();
                _listaElif.Add( l);
                //-------------------------------------------
                checkpointPos = scanner.GetPos();
                Debug.Log("SENAO OK");
            }
            else
            {
                
                scanner.SetPos(checkpointPos);
                break;
            }
        } while (true);

        if (!scanner.IsEnd() && ENTAO())
        {
            if (!(ACH())) { return false; }
            //---------------------------------------------------
            _mainThread = new List<AbstractCommand>();
            _stackCommands.Push(_mainThread);
            //----------------------------------------------------
            cmdinterno = true;
            if (!(BLOCO())) { Debug.LogError("CMDSE - BLOCO()"); return false; }
            cmdinterno = false;
            if (!(FCH())) { return false; }
            //-------------------------------
            _listaElse = _stackCommands.Pop();
            //---------------------------------
            Debug.Log("ENTAO OK");
        }
        else
        {
            scanner.SetPos(checkpointPos);
        }
        //-----------------------------------------------------------------
        //_listaElse = _stackCommands.Pop();
        SeCommand cmd = new SeCommand(condIf, cond, _listaIf, _listaElif, _listaElse);
        _mainThread = new List<AbstractCommand>();
        _stackCommands.Push(_mainThread);
        _stackCommands.Peek().Add(cmd);
        //-----------------------------------------------------------------
        Debug.Log("CMDSE OK");
        return true;
    }

    public bool CMDMOVA()
    {
        if (!(MOVA())) { Debug.LogError("MOVA - MOVA()"); return false; }
        if (!(AP())) { Debug.LogError("MOVA - AP()"); return false; }
        //if (!(ARGS())) { Debug.LogError("MOVA - EXPR()"); return false; }
        //if (!(NUMBER())) { Debug.LogError("MOVA - NUMBER()"); return false; }
        //string distancia = token.GetText();
        //if (!(VIRGULA())) { Debug.LogError("MOVA - VIRGULA()"); return false; }
        //string vir = token.GetText();
        if (!(DIRECAO())) { Debug.LogError("MOVA - DIRECAO()"); return false; }
        string direcao = token.GetText();
        if (!(FP())) { Debug.LogError("MOVA - FP()"); return false; }
        if (!(PV())) { Debug.LogError("MOVA - PV()"); return false; }
        //--------------------------------------------------------------------
        MovaCommand cmd = new MovaCommand(direcao);
        _stackCommands.Push(_mainThread);
        _stackCommands.Peek().Add(cmd);
        Debug.Log("CMDMOVA OK");
        return true;
    }

    public bool ARGS()
    {
        if (!(NUMBER())) { Debug.LogError("ARGS - NUMBER()"); return false; }
        if (!(VIRGULA())) { Debug.LogError("ARGS - VIRGULA()"); return false; }
        if (!(DIRECAO())) { Debug.LogError("ARGS - DIRECAO()"); return false; }
        return true;
    }

    public bool VIRGULA()
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() == Token.TK_PONCTUATION && token.GetText().Equals(","))
        {
            return true;
        }
        return false;
    }

    public bool DIRECAO()
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() == Token.TK_RESERVED || token.GetText().Equals("direita") || token.GetText().Equals("esquerda") 
            || token.GetText().Equals("cima" ) || token.GetText().Equals("baixo"))
        {
            return true;
        }
        return false;
    }

    public bool MOVA()
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() == Token.TK_RESERVED && token.GetText().Equals("mova"))
        {
            return true;
        }
        return false;
    }


    public bool SE()
    {
        token = scanner.NextToken();
        if(token.GetTypeOfToken() == Token.TK_RESERVED && token.GetText().Equals("se"))
        {
            return true;
        }
        return false;
    }

    public bool SENAO()
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() == Token.TK_RESERVED && token.GetText().Equals("senao"))
        {
            return true;
        }
        return false;
    }

    public bool ENTAO()
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() == Token.TK_RESERVED && token.GetText().Equals("entao"))
        {
            return true;
        }
        return false;
    }

    public bool REPITA()
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() == Token.TK_RESERVED && token.GetText().Equals("repita"))
        {
            return true;
        }
        return false;
    }

 

    //EXPR -> TERMO | TERMO MATHOP TERMO
    public bool EXPR() {
        
        if (!(TERMO())) { return false; }
        _exprLida = token.GetText();
        checkpointPos = scanner.GetPos();
        do
        {
            if (MATHOP())
            {
                _exprLida += token.GetText();
                _exprContent += token.GetText();
                if (!(TERMO())) { Debug.LogError("EXPR - TERMO() "); return false; }

                _exprLida += token.GetText();
                checkpointPos = scanner.GetPos();
            }
            else
            {
                scanner.SetPos(checkpointPos);
                break;
            }
        } while (true);        
        return true;
    }

    //BLOCO-> ACH CMD  FCH
    public bool BLOCO() {
        do
        {
            if (CMD())
            {
                checkpointPos = scanner.GetPos();
            }
            else
            {
                scanner.SetPos(checkpointPos);
                break;
            }
        } while (true);
        return true;
    }

    //COND-> TERMO OPREL TERMO
    public bool COND() {
        _exprDecisao = "";
        if (!(TERMO())) { /*iserir erro*/ return false; }
        _exprDecisao = token.GetText();
        checkpointPos = scanner.GetPos();
        
            if (OPREL())
            {
                _exprDecisao += token.GetText();
                if (!(TERMO())) { Debug.LogError("DECLARAVAR - VIRGULA() - ID()"); return false; }
                _exprDecisao += token.GetText();
                 
                checkpointPos = scanner.GetPos();
            }
            else
            {
                scanner.SetPos(checkpointPos);
               
            }
        
        return true;
    }

    //TERMO-> id | numero
    public bool TERMO() 
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_IDENTIFIER && token.GetTypeOfToken() != Token.TK_NUMBER)
        {
            return false;
        }
        _exprContent += token.GetText();
        return true;
    }

    //OP->  "!" | ">" | "<"
    public bool OP() 
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_OPERATOR)
        {
            return false;
        }
        return true;
    }

    //MATHOP-> "+" | "-" | "/" | "*"
    public bool MATHOP() 
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_MATHOP)
        {
            return false;
        }
        return true;
    }

    //OPREL-> "==" | ">=" | "<=" | "!=" | "<" | ">"
    public bool OPREL() 
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_RELATIONAL)
        {
            return false;
        }
        return true;
    }

    //AP->  "("  
    public bool AP() 
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_LIMITER || !(token.GetText().Equals("(")))
        {
            return false;
        }
        return true;
    }

    //FP-> ")" 
    public bool FP() 
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_LIMITER || !(token.GetText().Equals(")")))
        {
            return false;
        }
        return true;
    }

    //ACH-> "{"
    public bool ACH() 
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_LIMITER || !(token.GetText().Equals("{")))
        {
            return false;
        }
        return true;
    }

    //FCH-> "}"
    public bool FCH() 
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_LIMITER || !(token.GetText().Equals("}")))
        {
            return false;
        }
        return true;
    }

    //PV -> ";" 
    public bool PV() 
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_PONCTUATION)
        {
            return false;
        }
        return true;
    }


    //ATRIB-> '='
    public bool ATRIB() 
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_ASSIGN )
        {
            return false;
        }
        return true;
    }

    //ID -> [a-z] ([a-z] | [A-Z] | [0-9])+;
    public bool ID()
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_IDENTIFIER) 
        { 
            return false; 
        }
        return true;
    }

    public bool NUMBER()
    {
        token = scanner.NextToken();
        if (token.GetTypeOfToken() != Token.TK_NUMBER)
        {
            return false;
        }
        return true;
    }

}
