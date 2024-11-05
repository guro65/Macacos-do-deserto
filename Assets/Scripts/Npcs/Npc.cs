using System.Collections;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [Header("Configurações Principais")]
    [SerializeField] private GameObject player;
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private float paraDeSeguirDistancia = 3f;
    [SerializeField] private float tempoEntreAcoes = 2.5f;
    [SerializeField] private bool estaSeguindo;
    [SerializeField] private bool estaAtacando;

    public Machado arma; // Referência à arma do NPC
    public int vida = 100;
    private Rigidbody rb;
    private Animator animator;
    private bool defendendo = false;
    private ContagemDeNpc controleDeObjetivo;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        estaSeguindo = false;
        estaAtacando = false;
        animator.SetBool("EstaParado", true);
        animator.SetBool("Defesa", false);
        controleDeObjetivo = FindObjectOfType<ContagemDeNpc>();

        
        if (arma == null)
        {
            arma = GetComponentInChildren<Machado>(); // Tenta pegar a arma do filho
        }
    }

    private void Update()
    {
        if (estaSeguindo)
        {
            animator.SetBool("EstaParado", false);
            SeguirPlayer();
        }
    }

    public void ReceberDano(int dano)
    {
        int danoRecebido = defendendo ? dano / 2 : dano;
        vida -= danoRecebido;

        if (vida <= 0)
        {
            vida = 0;
            estaSeguindo = false;
            estaAtacando = false;
            rb.velocity = Vector3.zero;

            if (controleDeObjetivo != null && gameObject.CompareTag("Inimigo"))
            {
                controleDeObjetivo.NpcDerrotado();
                Debug.Log("NPC derrotado e notificado ao controle de objetivos.");
            }

            Destroy(gameObject, 2f);
        }
        else
        {
            Debug.Log("NPC ainda tem vida: " + vida);
        }
    }

    private void SeguirPlayer()
    {
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;
        rb.velocity = moveDirection * velocidade;

        if (Vector3.Distance(player.transform.position, transform.position) <= paraDeSeguirDistancia)
        {
            estaSeguindo = false;
            rb.velocity = Vector3.zero;
            animator.SetBool("Andar", true);
            estaAtacando = false;
            return;
        }

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            animator.SetBool("Andar", true);
            estaSeguindo = true;
        }
        else
        {
            estaSeguindo = false;
            animator.SetBool("Andar", false);
        }
    }

    public void EstaSeguindo()
    {
        animator.SetBool("Andar", true);
        estaSeguindo = true;
    }

    public void NaoEstaSeguindo()
    {
        estaSeguindo = false;
        animator.SetBool("Andar", false);
    }

    private IEnumerator ExecutarAtaque()
    {
        while (estaAtacando)
        {
            if (player == null) yield break; // Verifica se o player ainda existe
            animator.SetTrigger("Ataque");
            yield return new WaitForSeconds(1f); // Tempo da animação de ataque

            // Causa dano ao player usando a arma
            if (arma != null)
            {
                player.GetComponent<Player>().ReceberDano(arma.dano); // Chama o método para receber dano
            }

            yield return new WaitForSeconds(tempoEntreAcoes);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaSeguindo = false;
            estaAtacando = true;
            StartCoroutine(ExecutarAtaque());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaAtacando = false;
            StopCoroutine(ExecutarAtaque());
            estaSeguindo = true;
            animator.SetBool("Andar", true);
        }
    }
}
