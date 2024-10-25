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
    public int danoInimigo;
    public int vida = 100;
    private Rigidbody rb;
    private Animator animator;
    private bool defendendo = false;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        estaSeguindo = false;
        estaAtacando = false;
        animator.SetBool("EstaParado", true);
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
        }
    }

    private void SeguirPlayer()
    {
        float distancia = Vector3.Distance(player.transform.position, transform.position);
        if (distancia > paraDeSeguirDistancia)
        {
            Vector3 direcao = (player.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direcao * velocidade * Time.deltaTime);
            animator.SetBool("Andar", true);
        }
        else
        {
            animator.SetBool("Andar", false);
            StartCoroutine(ExecutarAcoesAleatorias());
        }
    }

    public void EstaSeguindo()
    {
        estaSeguindo = true;
    }

    public void NaoEstaSeguindo()
    {
        estaSeguindo = false;
        animator.SetBool("EstaParado", true);
    }

    private IEnumerator ExecutarAcoesAleatorias()
    {
        while (estaAtacando)
        {
            int acao = Random.Range(0, 3);
            animator.SetBool("Andar", false);
            switch (acao)
            {
                case 0:
                    yield return new WaitForSeconds(1);
                    animator.SetTrigger("Ataque");
                    player.GetComponent<Player>().ReceberDano(danoInimigo);
                    break;
                case 1:
                    yield return new WaitForSeconds(1);
                    animator.SetTrigger("Ataque2");
                    player.GetComponent<Player>().ReceberDano(danoInimigo * 2);
                    break;
                case 2:
                    yield return new WaitForSeconds(1);
                    animator.SetTrigger("Defesa");
                    defendendo = true;
                    yield return new WaitForSeconds(tempoEntreAcoes);
                    defendendo = false;
                    break;
            }
            yield return new WaitForSeconds(tempoEntreAcoes);
        }
    }
}
