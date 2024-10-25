using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int vidaAtual;
    [SerializeField] private int vida = 100;
    [SerializeField] private float ataque;
    [SerializeField] private float velocidade;
    [SerializeField] private int forcaPulo;
    [SerializeField] private int ouro;
    [SerializeField] private bool temChave;
    [SerializeField] private bool pegando;
    [SerializeField] private bool podePegar;
    [SerializeField] private Animator animator;
    [SerializeField] private bool estaVivo = true;
    [SerializeField] private List<GameObject> inventario = new List<GameObject>();
    [SerializeField] private BarraDeVida barraDeVida;
    private Rigidbody rb;
    private bool estaPulando;
    private Vector3 angleRotation;
    private bool defendendo = false;

    void Start()
    {
        temChave = false;
        pegando = false;
        podePegar = false;
        angleRotation = new Vector3(0, 90, 0);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        vidaAtual = vida;
        barraDeVida.AlteraBarraDeVida(vidaAtual,vida);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && podePegar)
        {
            animator.SetTrigger("Pegando");
            pegando = true;
        }

        if(Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Andar", true);
            animator.SetBool("AndarParaTras", false);
            Walk();
        }
        else if(Input.GetKey(KeyCode.S))
        {
            animator.SetBool("AndarParaTras", true);
            animator.SetBool("Andar", false);
            Walk();
        }
        else if(Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Andar", true);
        }
        else
        {
            animator.SetBool("Andar", false);
            animator.SetBool("AndarParaTras", false);
        }

        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            animator.SetBool("Andar", false);
            animator.SetBool("AndarParaTras", false);
        }

        if(Input.GetKeyDown(KeyCode.Space) && !estaPulando)
        {
            animator.SetTrigger("Pular");
            Jump();
        }

        if(Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Ataque");
            Atacar();
        }

        if(Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Ataque2");
        }

        if(Input.GetMouseButtonDown(0) && Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetTrigger("Ataque3");
        }

        if(Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            animator.SetBool("Correndo", true);
            Walk(8);
        }
        else
        {
            animator.SetBool("Correndo", false);
        }

        if(!estaVivo)
        {
            animator.SetTrigger("EstaVivo");
            estaVivo = true;
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            animator.SetTrigger("Defesa");
            defendendo = true;
        }
        else if(Input.GetKeyUp(KeyCode.Q))
        {
            defendendo = false;
        }

      

        TurnAround();
    }

    public void ReceberDano(int dano)
    {
        int danoRecebido = defendendo ? dano / 2 : dano;
        vida -= danoRecebido;
        if (vidaAtual <= 0)
        {
            vidaAtual -= dano;
            estaVivo = false;
            animator.SetTrigger("Morrer");
        }
    }

    private void Atacar()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward, 1.5f);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Inimigo"))
            {
                enemy.GetComponent<Npc>().ReceberDano((int)ataque);
            }
        }
    }

    private void FixedUpdate()
    {

    }

    private void Walk(float velo = 1)
    {
        if((velo == 1))
        {
            velo = velocidade;
        }
        float fowardInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.forward * fowardInput;
        Vector3 moveFoward = rb.position + moveDirection * velo * Time.deltaTime;
        rb.MovePosition(moveFoward);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
        estaPulando = true;
        animator.SetBool("EstaNoChao", false);
    }

    private void TurnAround()
    {
        float sideInput = Input.GetAxis("Horizontal");
        Quaternion deltaRotation = Quaternion.Euler(angleRotation * sideInput * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Inimigo"))
        {
            ReceberDano(collision.gameObject.GetComponent<Npc>().danoInimigo);
        }

        if(collision.gameObject.CompareTag("Chao"))
        {
            estaPulando = false;
            animator.SetBool("EstaNoChao", true);
        }
    }

    private void OnTriggerEnter()
    {
        podePegar = true;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.tag);

        if(other.gameObject.CompareTag("Chave") && pegando)
        {
            inventario.Add(Instantiate(other.gameObject.GetComponent<ChaveAntiga>().CopiaDaChave()));
            int numero = other.gameObject.GetComponent<ChaveAntiga>().PegarNumeroChave();
            Debug.LogFormat($"Chave número: {numero} inserida no inventário");
            Destroy(other.gameObject);
            pegando = false;
            podePegar = false;
        }

        if(other.gameObject.CompareTag("Porta") && pegando && temChave)
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("Abrir");
            temChave = true;
        }

        if(other.gameObject.CompareTag("Bau") && pegando)
        {
            if(VerificaChave(other.gameObject.GetComponent<Bau>().PegarNumeroFechadura()))
            {
                other.gameObject.GetComponent<Animator>().SetTrigger("AbrirBau");
            }
            else
            {
                Debug.Log("Você não tem a chave.");
            }
        }

        if(other.gameObject.CompareTag("Inimigo"))
        {
            other.gameObject.GetComponent<Npc>().EstaSeguindo();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Inimigo"))
        {
            other.gameObject.GetComponent<Npc>().NaoEstaSeguindo();
        }
    }

    private void OnCollisionExit(Collision other)
    {
        pegando = false;
        podePegar = false;
    }

    private bool VerificaChave(int chave)
    {
        foreach(GameObject item in inventario)
        {
            if(item.gameObject.CompareTag("Chave"))
            {
                if(item.gameObject.GetComponent<ChaveAntiga>().PegarNumeroChave() == chave)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void PegarConteudoDoBau(GameObject bau)
    {
        Bau bauTesouro = bau.GetComponent<Bau>();

        ouro += bauTesouro.PegarOuro();

        if(bauTesouro.AcessarConteudoBau() != null)
        {
            foreach(GameObject item in bauTesouro.AcessarConteudoBau())
            {
                inventario.Add(item);
            }
            bauTesouro.RemoverConteudoBau();
        }
    }
}
