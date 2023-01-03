using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParserInterpreter 
{
    private Scanner scanner;
    private Token token;
    private int posAtual;
    private int posInicial;

    private bool cmdinterno;

   
    private int checkpointPos;

    private int _tipo;
    private string _varName;
    private string _varValue;
    private TabelaSimbolo _tabelaSimbolo = new TabelaSimbolo();
    private Simbolos _simbolo;

    private Gerador _gerador = new Gerador();
    private Exe _exe;
    private List<ComandoAbstrato> _mainThread = new List<ComandoAbstrato>();

    private string _exprLida;
    private string _exprID;
    private string _exprContent;

    private Stack<List<ComandoAbstrato>> _stackCommands = new Stack<List<ComandoAbstrato>>();

    private bool _exprDecisao;
    private List<ComandoAbstrato> _listaIf; //lista else if
    private List<List<ComandoAbstrato>> _listaElif = new List<List<ComandoAbstrato>>();//lista de listas else if
    private List<ComandoAbstrato> l; // lista de comandos do else if
    private List<ComandoAbstrato> _listaElse;//lista else

    private List<ComandoAbstrato> _listaLoop;
    private string _exprLoop;

    private bool inLoop;
    private int numLoops;
    private int numId;
    private string idValue;
    private int numLenght;

    Player player;
    public void IsParser(Scanner scanner)
    {
        this.scanner = scanner;
        player = GameObject.Find("Player").GetComponent<Player>();
        _exe = GameObject.Find("Exe").GetComponent<Exe>();
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
            Debug.Log("Variável" + _varName + " já declarada!");
            return false;
        }
    }

    public bool ChecarSimbolo(string varname)
    {
        if (!_tabelaSimbolo.exists(varname))
        {
            Debug.Log("Variável" + _varName + " ainda não declarada!");
            return false;
        }
        return true;
    }

    public string ChecarRetornarValor(string varname)
    {
        if (_tabelaSimbolo.exists(varname))
        {
            if (!GetValor(varname).Equals(""))
            {
                return GetValor(varname);
            }
            else
            {
                Debug.Log("Valor da váriavel " + _varName + " não atribuido!");
                return GetValor(varname);
            }
           
        }
        else
        {
            return varname;
        }
    }

    public int ChecarVarTipo(string varname)
    {

        if (_tabelaSimbolo.exists(varname))
        {
            Simbolos s = _tabelaSimbolo.get(varname);
            Variaveis v = (Variaveis)s;
            return v.MyType;
        }

        return 2;
    }

    public void AddValorVar(string varname, string valor)
    {
        if (_tabelaSimbolo.exists(varname))
        {
            Simbolos s = _tabelaSimbolo.get(varname);
            Variaveis v = (Variaveis)s;
            v.MyID = valor;
        }

    }

    public string GetValor(string varname)
    {
        if (_tabelaSimbolo.exists(varname))
        {
            Simbolos s = _tabelaSimbolo.get(varname);
            Variaveis v = (Variaveis)s;
            return v.MyID;
        }

        return "";
    }

    public void ExecultarCodigo()
    {  
        _exe.ExecultarComandos();
    }

    public void ExibeComandos()
    {
        foreach (ComandoAbstrato c in _exe.MyComandos)
        {
            Debug.Log("<color=yellow>" + c + "</color>");
        }
    }

    //INICIO  -> CMD METODO INICIO
    public bool INICIO()
    {


        cmdinterno = false;
        _mainThread = new List<ComandoAbstrato>();
        _stackCommands.Push(_mainThread);
        if (!CMD()) { return false; }

        //----------_gerador.SetComandos(_mainThread);

         _exe.MyTabelaVar = _tabelaSimbolo;
         _exe.MyComandos = _stackCommands.Pop();
         ExibeComandos();
        //Debug.Log(_stackCommands.Peek());
        //------------------------------------
        return true;
    }

    //CMD     -> CMDATRIB | CMDREPITA | CMDSE
    public bool CMD()
    {
        
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

        }
        else
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
                            //Debug.Log("CMD NÃO RECONHECIDO");
                            return false;
                        }
                    }
                }

            }
        }

        if (cmdinterno)//se cmd for chamado de outro metodo então retomar
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
        if (!(DECLARAVAR())) { Debug.Log("DECLARACAO - DECLARAVAR()"); return false; }
        return true;
    }

    //DECLARAVAR -> TIPO ID () PV
    public bool DECLARAVAR()
    {

        if (!(TIPO())) { Debug.Log("DECLARAVAR - TIPO()"); return false; }
        if (!(ID())) { Debug.Log("DECLARAVAR - ID()"); return false; }
        //-------------------------
        if (!(AddSimbolo())) { Debug.Log("DECLARAVAR - ADDSIMBOLO()"); return false; }
        //-------------------------
        checkpointPos = scanner.GetPos();
        do
        {
            if (VIRGULA())
            {
                if (!(ID())) { Debug.Log("DECLARAVAR - VIRGULA() - ID()"); return false; }
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

        if (!(PV())) { Debug.Log("DECLARAVAR - PV()"); return false; }
        Debug.Log("DECLARAVAR OK");
        return true;
    }

    public bool TIPO()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
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
    public bool METODO()
    {
        if (!(ID())) { Debug.Log("METODO - ID()"); return false; }
        if (!(AP())) { Debug.Log("METODO - AP()"); return false; }
        if (!(EXPR())) { Debug.Log("METODO - EXPR()"); return false; }
        if (!(FP())) { Debug.Log("METODO - FP()"); return false; }
        if (!(PV())) { Debug.Log("METODO - PV()"); return false; }
        Debug.Log("METODO OK");
        return true;
    }

    //CMDATRIB-> id ATRIB EXPR PV
    public bool CMDATRIB()
    {
        numId = 0;
        idValue = "";
        if (!(ID())) { Debug.Log("CMDATRIB - ID()"); return false; }
        //----------------------------------------
        if (!ChecarSimbolo(token.GetText())) { return false; }
        int varTipo = ChecarVarTipo(token.GetText());
        _exprLoop = token.GetText();
        _exprID = token.GetText();
        //-----------------------------------
        if (!(ATRIB())) { Debug.Log("CMDATRIB - ID()"); return false; }
        //-----------------------------------------------
        _exprLoop = token.GetText();
        _exprContent = "";
        //-----------------------------------------------
        //if (!(EXPR())) { Debug.Log("CMDATRIB - EXPR()"); return false; }
        //------------------------------------------------------
        if (varTipo == 0)
        {
            if (!(NUMBER())) { Debug.Log("CMDATRIB - NUMBER()"); return false; }
            numId = int.Parse(token.GetText());
            _exprContent = token.GetText();
        }
        else if(varTipo == 1)
        {
            if (!(ID())) { Debug.Log("CMDATRIB - NUMBER()"); return false; }
            idValue = token.GetText();
            _exprContent = token.GetText();
        }
        //-------------------------------------------------------
        if (!(PV())) { Debug.Log("CMDATRIB - PV()"); return false; }
        _exprLoop = token.GetText();
        Debug.Log("CMDATRIB OK");
        //----------------------------------------------
        AddValorVar(_exprID, _exprContent);
        ComandoAtribuicao cmd = new ComandoAtribuicao(_exprID, _exprContent);
        _stackCommands.Peek().Add(cmd);
        //-------------------------------------------
        return true;

    }

    //CMDREPITA-> "Repita" AP EXPR FP BLOCO
    public bool CMDREPITA()
    {
        inLoop = true; 
        if (!(REPITA())) { Debug.Log("REPITA - REPITA()"); return false; }
        if (!(AP())) { Debug.Log("REPITA - AP()"); return false; }
        //if (!(EXPRREPITA())) { Debug.Log("REPITA - EXPR()"); return false; }
        // Debug.Log("exprLida=" + token.GetText());
        if(!(NUMBER())) { Debug.Log("REPITA - EXPR()"); return false; }
        numLoops = int.Parse(token.GetText());
        if (!(FP())) { Debug.Log("REPITA - FP()"); return false; }
        if (!(ACH())) { Debug.Log("CMDREPITA - ACH()"); return false; }
        cmdinterno = true;
        _mainThread = new List<ComandoAbstrato>();
        _stackCommands.Push(_mainThread);
        if (!(BLOCO())) { Debug.Log("CMDREPITA - BLOCO()"); return false; }
        cmdinterno = false;
        if (!(FCH())) { Debug.Log("CMDREPITA - FCH()"); return false; }
        Debug.Log("CMDREPITA OK");
        //----adiciona o RepitaCommand na lista de comandos ----      
        _listaLoop = _stackCommands.Pop();
        ComandoRepita cmd = new ComandoRepita(_exprLoop, _listaLoop, numLoops);
        _mainThread = new List<ComandoAbstrato>();
        //_stackCommands.Push(_mainThread);
        _stackCommands.Peek().Add(cmd);

        //------------------------------------------------------

        inLoop = false;
        return true;
    }

    public bool EXPRREPITA()
    {
        _exprLoop = "";


        //i = 0;  i < 9; i++
        
        if (!CMDATRIB()) { Debug.Log("EXPRREPITA - CMDATRIB()"); return false; } 
        if (!ID()) { Debug.Log("EXPRREPITA - ID()2"); return false; }
        _exprLoop += token.GetText();
        if (!OPREL()) { Debug.Log("EXPRREPITA - OPREL()"); return false; }
        _exprLoop += token.GetText();
        checkpointPos = scanner.GetPos();
        if (!ID())
        {
            scanner.SetPos(checkpointPos);
            if (!NUMBER()) { Debug.Log("EXPRREPITA - NUMBER()2"); return false; }
            //return false; 
            numLenght = int.Parse(token.GetText());
        }
        _exprLoop += token.GetText();
        if (!PV()) { Debug.Log("EXPRREPITA - PV()2"); return false; }
        _exprLoop += token.GetText();
        if (!ID()) { Debug.Log("EXPRREPITA - ID()3"); return false; }
        _exprLoop += token.GetText();
        if (!MATHOP()) { Debug.Log("EXPRREPITA - MATHOP()3"); return false; }
        _exprLoop += token.GetText();
        if (!MATHOP()) { Debug.Log("EXPRREPITA - MATHOP()4"); return false; }
        _exprLoop += token.GetText();
        
        return true;
    }

    //CMDSE -> SE AP COND FP ACH (CMD)* FCH (ELSEIF AP COND FP ACH (CMD)* FCH)? (ELSE ACH (CMD)+ FCH)?;
    public bool CMDSE()
    {

        if (!(SE())) { Debug.Log("CMDSE - SE()"); return false; }
        if (!(AP())) { Debug.Log("CMDSE - AP()"); return false; }
        if (!(COND())) { Debug.Log("CMDSE - COND()"); return false; }
        //--------------------------------------
        bool condIf = _exprDecisao;
        //--------------------------------------
        if (!(FP())) { Debug.Log("CMDSE - FP()"); return false; }
        if (!(ACH())) { Debug.Log("CMDSE - ACH()"); return false; }
        checkpointPos = scanner.GetPos();
        cmdinterno = true;
        //--------------------------------------------------------
        _mainThread = new List<ComandoAbstrato>();
        _stackCommands.Push(_mainThread);
        //----------------------------------------------------------
        if (!(BLOCO())) { Debug.Log("CMDSE - BLOCO()"); return false; }
        cmdinterno = false;
        if (!(FCH())) { Debug.Log("CMDSE - FCH()"); return false; }
        //----------------------------------------------------
        _listaIf = _stackCommands.Pop();
        //----------------------------------------------------
        checkpointPos = scanner.GetPos();

        List<bool> cond = new List<bool>();
        l = new List<ComandoAbstrato>();
        do
        {
            if (!scanner.IsEnd() && SENAO())
            {
                if (!(AP())) { Debug.Log("CMDSE -SENAO - SE()"); return false; }
                if (!(COND())) { Debug.Log("CMDSE -SENAO - AP()"); return false; }
                //------------------------------
                cond.Add(_exprDecisao);
                //------------------------------
                if (!(FP())) { Debug.Log("CMDSE -SENAO - FP()"); return false; }
                if (!(ACH())) { Debug.Log("CMDSE -SENAO - ACH()"); return false; }
                cmdinterno = true;
                //--------------------------------------------
                _mainThread = new List<ComandoAbstrato>();
                _stackCommands.Push(_mainThread);
                //-------------------------------------------
                if (!(BLOCO())) { Debug.Log("CMDSE - BLOCO()"); return false; }

                cmdinterno = false;
                if (!(FCH())) { Debug.Log("CMDSE -SENAO - FCH()"); return false; }
                //-------------------------------------------

                l = _stackCommands.Pop();
                //_listaElif = new List<List<AbstractCommand>>();
                _listaElif.Add(l);
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
            _mainThread = new List<ComandoAbstrato>();
            _stackCommands.Push(_mainThread);
            //----------------------------------------------------
            cmdinterno = true;
            if (!(BLOCO())) { Debug.Log("CMDSE - BLOCO()"); return false; }
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
        //-----------------------------------------------------------------------------
        ComandoSe cmd = new ComandoSe(condIf, cond, _listaIf, _listaElif, _listaElse);
        _mainThread = new List<ComandoAbstrato>();
        //_stackCommands.Push(_mainThread);
        _stackCommands.Peek().Add(cmd);
        //-----------------------------------------------------------------------------
        Debug.Log("CMDSE OK");
        return true;
    }

    public bool CMDMOVA()
    {
        string direcao = "";
        int passos = 1;
        if (!(MOVA())) { Debug.Log("MOVA - MOVA()"); return false; }
        if (!(AP())) { Debug.Log("MOVA - AP()"); return false; }
        if (!(DIRECAO())) { Debug.Log("MOVA - DIRECAO()"); return false; }
        direcao = token.GetText();
        if (!(FP())) { Debug.Log("MOVA - FP()"); return false; }
        if (!(PV())) { Debug.Log("MOVA - PV()"); return false; }
        //--------------------------------------------------------------------
        if (inLoop)
        {
            passos = numLoops;
        }
        ComandoMova cmd = new ComandoMova(direcao, passos);
        //_stackCommands.Push(_mainThread);
        _stackCommands.Peek().Add(cmd);
        Debug.Log("CMDMOVA OK");
        
        return true;
    }

    public bool ARGS()
    {
        if (!(NUMBER())) { Debug.Log("ARGS - NUMBER()"); return false; }
        if (!(VIRGULA())) { Debug.Log("ARGS - VIRGULA()"); return false; }
        if (!(DIRECAO())) { Debug.Log("ARGS - DIRECAO()"); return false; }
        return true;
    }

    public bool VIRGULA()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
        if (token.GetTypeOfToken() == Token.TK_PONCTUATION && token.GetText().Equals(","))
        {
            return true;
        }
        return false;
    }

    public bool DIRECAO()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
        if (token.GetTypeOfToken() == Token.TK_RESERVED || token.GetText().Equals("direita") || token.GetText().Equals("esquerda")
            || token.GetText().Equals("cima") || token.GetText().Equals("baixo"))
        {
            return true;
        }
        return false;
    }

    public bool MOVA()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
        if (token.GetTypeOfToken() == Token.TK_RESERVED && token.GetText().Equals("mova"))
        {
            return true;
        }
        return false;
    }


    public bool SE()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
        if (token.GetTypeOfToken() == Token.TK_RESERVED && token.GetText().Equals("se"))
        {
            return true;
        }
        return false;
    }

    public bool SENAO()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
        if (token.GetTypeOfToken() == Token.TK_RESERVED && token.GetText().Equals("senao"))
        {
            return true;
        }
        return false;
    }

    public bool ENTAO()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
        if (token.GetTypeOfToken() == Token.TK_RESERVED && token.GetText().Equals("entao"))
        {
            return true;
        }
        return false;
    }

    public bool REPITA()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
        if (token.GetTypeOfToken() == Token.TK_RESERVED && token.GetText().Equals("repita"))
        {
            return true;
        }
        return false;
    }



    //EXPR -> TERMO | TERMO MATHOP TERMO
    public bool EXPR()
    {

        if (!(TERMO())) { return false; }
        _exprLida = token.GetText();
        checkpointPos = scanner.GetPos();
        do
        {
            if (MATHOP())
            {
                _exprLida += token.GetText();
                _exprContent += token.GetText();
                if (!(TERMO())) { Debug.Log("EXPR - TERMO() "); return false; }

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
    public bool BLOCO()
    {
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
    public bool COND()
    {
        _exprDecisao = false;
        string oprel = "";
        if (!(TERMO())) { /*iserir erro*/ return false; }
        //---------------------------------------------------------
        var t1 = ChecarRetornarValor(token.GetText());
        var t2 = "";
        //---------------------------------------------------------
        checkpointPos = scanner.GetPos();

        if (OPREL())
        {
            oprel = token.GetText();
            if (!(TERMO())) { Debug.Log("DECLARAVAR - VIRGULA() - ID()"); return false; }
            t2 = ChecarRetornarValor(token.GetText());

            checkpointPos = scanner.GetPos();
        }
        else
        {
            scanner.SetPos(checkpointPos);

        }
        int i1 = 0;
        string s1 = "";
        int i2 = 0;
        string s2 = "";

        if (int.TryParse(t1, out int t) && int.TryParse(t2, out int j))
        {
            i1 = t;
            i2 = j;
            switch (oprel)
            {
                case "<":
                    if (i1 < i2)
                    {
                        _exprDecisao = true;
                    }
                    break;
                case ">":
                    if (i1 > i2)
                    {
                        _exprDecisao = true;
                    }
                    break;
                case "<=":
                    if (i1 <= i2)
                    {
                        _exprDecisao = true;
                    }
                    break;
                case ">=":
                    if (i1 >= i2)
                    {
                        _exprDecisao = true;
                    }
                    break;
                case "==":
                    if (i1 == i2)
                    {
                        _exprDecisao = true;
                    }
                    break;
                case "!=":
                    if (i1 != i2)
                    {
                        _exprDecisao = true;
                    }
                    break;
            }
        }
        else
        {
            s1 = t1;
            s2 = t2;
            switch (oprel)
            {        
                case "==":
                    if (s1.Equals(s2))
                    {
                        _exprDecisao = true;
                    }
                    break;
                case "!=":
                    if (!i1.Equals(i2))
                    {
                        _exprDecisao = true;
                    }
                    break;
            }
        }
        
        return true;
    }

    //TERMO-> id | numero
    public bool TERMO()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
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
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
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
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
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
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
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
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
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
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
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
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
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
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
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
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
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
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
        if (token.GetTypeOfToken() != Token.TK_ASSIGN)
        {
            return false;
        }
        return true;
    }

    //ID -> [a-z] ([a-z] | [A-Z] | [0-9])+;
    public bool ID()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
        if (token.GetTypeOfToken() != Token.TK_IDENTIFIER)
        {
            return false;
        }
        return true;
    }

    public bool NUMBER()
    {
        token = scanner.NextToken();
        if (token == null)
        {
            Debug.Log("Erro encontrado");
            return false;
        }
        if (token.GetTypeOfToken() != Token.TK_NUMBER)
        {
            return false;
        }
        return true;
    }
}
