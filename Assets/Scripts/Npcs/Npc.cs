using System.Collections;
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

    private ContagemDeNpc controleDeObjetivo; // Referência ao controlador de objetivos

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        estaSeguindo = false;
        estaAtacando = false;
        animator.SetBool("EstaParado", true);

        // Obtém a referência ao controlador de objetivos na cena
        controleDeObjetivo = FindObjectOfType<ContagemDeNpc>();
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
            animator.SetTrigger("Morrer");
            estaSeguindo = false;
            estaAtacando = false;
            rb.velocity = Vector3.zero;

            // Notifica o controlador de objetivos que este NPC foi derrotado
            if (controleDeObjetivo != null && gameObject.CompareTag("Inimigo"))
            {
                controleDeObjetivo.NpcDerrotado();
                Debug.Log("NPC derrotado e notificado ao controle de objetivos.");
            }

            // Destrói o GameObject do NPC
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
        rb.velocity = moveDirection * velocidade;

        // Verifica se está próximo o suficiente para parar de seguir
        if (Vector3.Distance(player.transform.position, transform.position) <= paraDeSeguirDistancia)
        {
            estaSeguindo = false;
            rb.velocity = Vector3.zero;
            animator.SetBool("Andar", false);
            return;
        }

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            animator.SetBool("Andar", true);
        }
        else
        {
            animator.SetBool("Andar", false);
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
        animator.SetBool("Andar", false);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaAtacando = true;
            StartCoroutine(ExecutarAcoesAleatorias());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            estaAtacando = false;
            animator.SetBool("Andar", true);
        }
    }
}
