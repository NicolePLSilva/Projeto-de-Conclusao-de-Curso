using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float force = 5f;
    Rigidbody2D myRigidbody;

    Vector2 movement;

    bool toStop = false;


    [SerializeField] Ponte ponte;

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Transform movePoint;
    Collider2D moveCollider;
    //[System.NonSerialized]
    public string direction { get; set; }
    public bool move;
    public int numSteps;
    //public int MyDirection 
    [SerializeField]private Sprite[] spritesArray;
    [SerializeField]private SpriteRenderer sr;

    Animator myAnimator;
   
    void Start()
    {   
        myRigidbody = GetComponent<Rigidbody2D>();
       
        ponte = GameObject.Find("Ponte").GetComponent<Ponte>();
        moveCollider = GameObject.Find("PlayerMovePoint").GetComponent<Collider2D>();

        movePoint.parent = null;

        myAnimator = GetComponent<Animator>();
        sr = this.transform.Find("PlayerSprite").GetComponent<SpriteRenderer>();

        direction = "";
        move = false;
        numSteps = 1;
    }

    private void FixedUpdate()
    {

        Vector3 mp = movePoint.position;
        // if (moveCollider.IsTouchingLayers(LayerMask.GetMask("Walls")))
        // { movePoint.position = mp; }
        RaycastHit2D upInfo = Physics2D.Raycast(movePoint.position, Vector2.up, 1f, LayerMask.GetMask("Walls"));
        RaycastHit2D downInfo = Physics2D.Raycast(movePoint.position, Vector2.down, 1f, LayerMask.GetMask("Walls"));
       
        RaycastHit2D leftInfo = Physics2D.Raycast(movePoint.position, Vector2.left, 1f, LayerMask.GetMask("Walls"));
        Debug.DrawRay(movePoint.position, Vector2.up, Color.red);

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (move && !direction.Equals(""))
        {
            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                MoveDirecao();
                //myAnimator.SetBool("IsWalking", true);
                move = false;
            }
        }
    }

    public void MoveDirecao()
    {
        switch (direction)
        {
            case "direita":
                MoveDireita();
                break;
            case "esquerda":
                MoveEsquerda();
                break;
            case "cima":
                MoveCima();
                break;
            case "baixo":
                MoveBaixo();
                break;
            case "":
                
                Parar();
                break;
        }
    }

    public void MoveDireita()
    {
        RaycastHit2D rightInfo = Physics2D.Raycast(movePoint.position, Vector2.right, 1f, LayerMask.GetMask("Walls"));
        if (rightInfo)
        {
            Debug.Log("Obstaculo no caminho");
            return;
        }
        MudarSprite(spritesArray[2]);
        
        movePoint.position += new Vector3(1f * numSteps, 0f, 0f);
        
        direction = "";
        //myAnimator.SetBool("IsWalking", false);
    }

    public void MoveEsquerda()
    {
        RaycastHit2D leftInfo = Physics2D.Raycast(movePoint.position, Vector2.left, 1f, LayerMask.GetMask("Walls"));
        if (leftInfo)
        {
            Debug.Log("Obstaculo no caminho");
            return;
        }
        MudarSprite(spritesArray[2]);
        sr.flipX = true ;
        movePoint.position += new Vector3(-1f * numSteps, 0f, 0f);
      
        direction = "";
    }

    public void MoveCima()
    {
        RaycastHit2D upInfo = Physics2D.Raycast(movePoint.position, Vector2.up, 1f, LayerMask.GetMask("Walls"));
        if (upInfo)
        {
            Debug.Log("Obstaculo no caminho");
            return;
        }
        MudarSprite(spritesArray[1]);
        movePoint.position += new Vector3( 0f, 1f * numSteps, 0f);
        direction = "";
    }

    public void MoveBaixo()
    {
        RaycastHit2D downInfo = Physics2D.Raycast(movePoint.position, Vector2.down, 1f, LayerMask.GetMask("Walls"));
        if (downInfo)
        {
            Debug.Log("Obstaculo no caminho");
            return;
        }
        MudarSprite(spritesArray[0]);
        movePoint.position += new Vector3(0f, -1f * numSteps, 0f);
        direction = "";
    }

    public void Parar()
    {
        //myRigidbody.velocity = new Vector2(0f, -(force));
        movePoint.position += new Vector3(0f, 0f, 0f);
        ponte.MoverPlayer("");
        direction = "";
    }


    public void MudarSprite(Sprite newSprite)
    {
        sr.sprite = newSprite;
    }

    IEnumerator EsperaCoroutine()
    {
        //move = false;
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (move && !direction.Equals(""))
        {
            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                MoveDirecao();
                
            }
        }
        yield return new WaitForSeconds(2);
        //direction = "";
        //move = true;
        //yield return null;
    }
}
