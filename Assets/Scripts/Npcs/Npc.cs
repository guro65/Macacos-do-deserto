using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Npc : MonoBehaviour
{
<<<<<<< HEAD
=======

>>>>>>> 07b6ce3ac697fbe4ebc70305f271697b1d7ad61e
    [Header("Configurações Principais")]
    [SerializeField] private GameObject player;
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private float paraDeSeguirDistancia = 3f;
    [SerializeField] private float tempoEntreAcoes = 2.5f;
    [SerializeField] private float tempoSeguir = 2f;
    [SerializeField] private bool estaSeguindo;
    [SerializeField] private bool estaAtacando;
<<<<<<< HEAD
=======
    //[SerializeField] private BarraDeVida barraDeVida;
>>>>>>> 07b6ce3ac697fbe4ebc70305f271697b1d7ad61e

    public int danoInimigo;
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
        controleDeObjetivo = FindObjectOfType<ContagemDeNpc>();
        estaSeguindo = false;
        estaAtacando = false;
        animator.SetBool("EstaParado", true);
<<<<<<< HEAD
=======
        animator.SetBool("Defesa", false);
        // Obtém a referência ao controlador de objetivos na cena
        controleDeObjetivo = FindObjectOfType<ContagemDeNpc>();
        // vidaAtual = vidaInimigo;
        // barraDeVida.AlteraBarraDeVida(vidaAtual,vidaInimigo);

>>>>>>> 07b6ce3ac697fbe4ebc70305f271697b1d7ad61e
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
            animator.SetTrigger("Morrer");
            estaSeguindo = false;
            estaAtacando = false;
            rb.velocity = Vector3.zero;

            if (controleDeObjetivo != null && gameObject.CompareTag("Inimigo"))
            {
                controleDeObjetivo.NpcDerrotado();
            }

<<<<<<< HEAD
=======
            // Destrói o GameObject do NPC
            new WaitForSeconds(2);
>>>>>>> 07b6ce3ac697fbe4ebc70305f271697b1d7ad61e
            Destroy(gameObject);
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
        //animator.SetBool("EstaParado", true);
        animator.SetBool("Andar", false);
    }

    private IEnumerator ExecutarAcoesAleatorias()
    {
        while (estaAtacando)
        {
<<<<<<< HEAD
            int acao = Random.Range(0, 3);
=======
            estaSeguindo = false;
            int acao = Random.Range(0, 2);
>>>>>>> 07b6ce3ac697fbe4ebc70305f271697b1d7ad61e
            animator.SetBool("Andar", false);
            yield return new WaitForSeconds(1);

            switch (acao)
            {
                case 0:
<<<<<<< HEAD
=======
                    new WaitForSeconds(1);
>>>>>>> 07b6ce3ac697fbe4ebc70305f271697b1d7ad61e
                    animator.SetTrigger("Ataque");
                    player.GetComponent<Player>().ReceberDano(danoInimigo);
                    break;
                case 1:
<<<<<<< HEAD
=======
                    new WaitForSeconds(1);
>>>>>>> 07b6ce3ac697fbe4ebc70305f271697b1d7ad61e
                    animator.SetTrigger("Ataque2");
                    player.GetComponent<Player>().ReceberDano(danoInimigo * 2);
                    break;
                case 2:
<<<<<<< HEAD
=======
                    new WaitForSeconds(1);
>>>>>>> 07b6ce3ac697fbe4ebc70305f271697b1d7ad61e
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
        if (other.CompareTag("Player"))
        {
            estaSeguindo = false;
            animator.SetBool("Andar", false);
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
