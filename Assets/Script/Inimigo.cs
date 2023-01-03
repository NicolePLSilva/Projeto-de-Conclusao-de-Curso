using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{
    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;
    private BoxCollider2D box;
    [SerializeField] float moveSpeed = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        box =  GameObject.Find("Colisor").GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (IsFacingUp())
        {

            myRigidbody2D.velocity = new Vector2(0f, moveSpeed);
            myAnimator.SetInteger("direction", 1);
           

        }
        else
        {
            myRigidbody2D.velocity = new Vector2(0f, -moveSpeed);
            myAnimator.SetInteger("direction", 0);
        }
    }

    bool IsFacingUp()
    {
        return box.transform.localScale.y > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        box.transform.localScale = new Vector2( 1f, -(Mathf.Sign(myRigidbody2D.velocity.y)));
    }
}
