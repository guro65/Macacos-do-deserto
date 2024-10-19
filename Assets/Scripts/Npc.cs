using System;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [Header("principal")]
    [SerializeField]private GameObject player;
    [SerializeField]private float velocidade =  5f;
    [SerializeField]private float paraDeSeguir =  3f;
    [SerializeField]private float tempo = 1.5f;
    private bool estaSeguindo;
    private Rigidbody rb;
    private Animator animator;
    
   
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Guro");
        rb = GetComponent<Rigidbody>();
        estaSeguindo = false;
        animator = GetComponent<Animator>();
        transform.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        estaSeguindo = true;

        if(player != null && estaSeguindo)
        {
            Vector3 moveDirection = (player.transform.position - transform.position).normalized;//calcula para onde deve ir

            rb.velocity = moveDirection * velocidade; //move o player

            if (Vector3.Distance(player.transform.position, transform.position) <= paraDeSeguir)//para o jogador
            {
                estaSeguindo = false;        
            }

             if (moveDirection != Vector3.one)
            {
                new WaitForSeconds(0.1f);
                float angle = Mathf.Max(moveDirection.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            
        }

        if(velocidade > 1 )
        {
            animator.SetBool("Andar" , true);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject)
            {

            }
        }
    }
}