using System.Collections;
using UnityEngine;

public class Npc : MonoBehaviour
{
    [Header("Configurações Principais")]
    [SerializeField] private GameObject player;
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private float paraDeSeguirDistancia = 3f;
    [SerializeField] private float tempoEntreAcoes = 1.5f;  // Tempo entre as ações de ataque/defesa
    private bool estaSeguindo;
    private bool estaAtacando;
    private Rigidbody rb;
    private Animator animator;

    private void Start()
    {
        player = GameObject.FindWithTag("Player"); // Encontra o player por nome
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        estaSeguindo = true;
        estaAtacando = false;
    }

    private void Update()
    {
        if (player != null && estaSeguindo)
        {
            SeguirPlayer();
        }

        if (velocidade > 1)
        {
            animator.SetBool("Andar", true); // Animação de andar
        }
    }

    // Faz o inimigo seguir o player
    private void SeguirPlayer()
    {
        Vector3 moveDirection = (player.transform.position - transform.position).normalized;

        // Move o inimigo na direção do player
        rb.velocity = moveDirection * velocidade;

        // Verifica se está próximo o suficiente para parar de seguir
        if (Vector3.Distance(player.transform.position, transform.position) <= paraDeSeguirDistancia)
        {
            estaSeguindo = false;
            rb.velocity = Vector3.zero; // Para o movimento
        }

        // Ajusta a rotação do inimigo para ficar de frente para o player
        if (moveDirection != Vector3.one)
        {
            float angle = Mathf.Atan2(moveDirection.z, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
    }

    // Quando o inimigo colide com o player
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !estaAtacando)
        {
            estaSeguindo = false;  // Para de seguir o player
            estaAtacando = true;   // Começa as ações de ataque/defesa
            StartCoroutine(ExecutarAcoesAleatorias());  // Inicia o ciclo de ações aleatórias
        }
    }

    // Corrotina que executa ações aleatórias (ataques e defesa)
    private IEnumerator ExecutarAcoesAleatorias()
    {
        while (estaAtacando)
        {
            int acao = Random.Range(0, 4); // Gera um número aleatório entre 0 e 3 (4 ações no total)

            switch (acao)
            {
                case 0:
                    animator.SetTrigger("Ataque1");
                    Debug.Log("Executando Ataque 1");
                    break;
                case 1:
                    animator.SetTrigger("Ataque2");
                    Debug.Log("Executando Ataque 2");
                    break;
                case 2:
                    animator.SetTrigger("Ataque3");
                    Debug.Log("Executando Ataque 3");
                    break;
                case 3:
                    animator.SetTrigger("Defesa");
                    Debug.Log("Executando Defesa");
                    break;
            }

            // Espera o tempo entre as ações antes de executar a próxima
            yield return new WaitForSeconds(tempoEntreAcoes);
        }
    }

    // Quando o inimigo sai da colisão com o player
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            estaAtacando = false;  // Para as ações de ataque/defesa
            estaSeguindo = true;   // Volta a seguir o player
            animator.SetBool("Andar", true); // Retorna à animação de andar
        }
    }
}
