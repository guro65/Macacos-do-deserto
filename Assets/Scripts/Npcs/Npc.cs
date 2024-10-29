using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Npc : MonoBehaviour
{

    [Header("Configurações Principais")]
    [SerializeField] private GameObject player;
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private float paraDeSeguirDistancia = 3f;
    [SerializeField] private float tempoEntreAcoes = 2.5f;
    [SerializeField] private float tempoSeguir = 2f;
    [SerializeField] private bool estaSeguindo;
    [SerializeField] private bool estaAtacando;
    //[SerializeField] private BarraDeVida barraDeVida;

    public int danoInimigo;
    //private int vidaAtual;
    public int vida = 100;
    private Rigidbody rb;
    private Animator animator;
    private bool defendendo = false;

    private ContagemDeNpc controleDeObjetivo; // Referência ao controlador de objetivos

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        estaSeguindo = false;
        estaAtacando = false;
        animator.SetBool("EstaParado", true);
        animator.SetBool("Defesa", false);
        // Obtém a referência ao controlador de objetivos na cena
        controleDeObjetivo = FindObjectOfType<ContagemDeNpc>();
        // vidaAtual = vidaInimigo;
        // barraDeVida.AlteraBarraDeVida(vidaAtual,vidaInimigo);

    }

    public Npc(float tempoSeguir)
    {
        this.tempoSeguir = tempoSeguir;
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
            vida = 0;  // Garante que a vida não fique negativa
            //animator.SetTrigger("Morrer");
            estaSeguindo = false;
            estaAtacando = false;
            rb.linearVelocity = Vector3.zero;

            // Notifica o controlador de objetivos que este NPC foi derrotado
            if (controleDeObjetivo != null && gameObject.CompareTag("Inimigo"))
            {
                controleDeObjetivo.NpcDerrotado();
                Debug.Log("NPC derrotado e notificado ao controle de objetivos.");
            }

            // Destrói o GameObject do NPC
            new WaitForSeconds(2);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("NPC ainda tem vida: " + vida);
        }
    }

    private void SeguirPlayer()
    {
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;

        // Move o inimigo na direção do player
        rb.linearVelocity = moveDirection * velocidade;

        // Verifica se está próximo o suficiente para parar de seguir
        if (Vector3.Distance(player.transform.position, transform.position) <= paraDeSeguirDistancia)
        {
            estaSeguindo = false;
            rb.linearVelocity = Vector3.zero;
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
        //animator.SetBool("EstaParado", true);
        animator.SetBool("Andar", false);
    }

    private IEnumerator ExecutarAcoesAleatorias()
    {
        while (estaAtacando)
        {
            estaSeguindo = false;
            int acao = Random.Range(0, 2);
            animator.SetBool("Andar", false);
            switch (acao)
            {
                case 0:
                    new WaitForSeconds(1);
                    animator.SetTrigger("Ataque");
                    player.GetComponent<Player>().ReceberDano(danoInimigo);
                    break;
                case 1:
                    new WaitForSeconds(1);
                    animator.SetTrigger("Ataque2");
                    player.GetComponent<Player>().ReceberDano(danoInimigo * 2);
                    break;
                case 2:
                    new WaitForSeconds(1);
                    animator.SetTrigger("Defesa");
                    defendendo = true;
                    player.GetComponent<Player>().ReceberDano(danoInimigo / 2);
                    new WaitForSeconds(1);
                    defendendo = false;
                    break;
            }
            yield return new WaitForSeconds(tempoEntreAcoes);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            estaSeguindo = false;
            StartCoroutine(ExecutarAcoesAleatorias());
        }

    }

    private void OnTriggerStay(Collider other)
    {
        StartCoroutine(ExecutarAcoesAleatorias());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaAtacando = false;
            StopCoroutine(ExecutarAcoesAleatorias());
            estaSeguindo = true;
            animator.SetBool("Andar", true);
        }
    }
}