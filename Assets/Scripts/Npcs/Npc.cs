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
    [SerializeField]private bool estaSeguindo;
    [SerializeField]private bool estaAtacando;   // Tempo entre as ações de ataque/defesa
    public int danoInimigo;
    public int vida = 100;
    private Rigidbody rb;
    private Animator animator;    
   

    private void Start()
    {
        player = GameObject.FindWithTag("Player"); // Encontra o player por nome
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
            NaoEstaSeguindo();
            estaAtacando = true;   // Começa as ações de ataque/defesa
            //StartCoroutine(ExecutarAcoesAleatorias());  // Inicia o ciclo de ações aleatórias
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && estaAtacando)
        {
            StartCoroutine(ExecutarAcoesAleatorias());  // Inicia o ciclo de ações aleatórias
            
        }
    }

    private void OnCollisionExit(Collision other)
    {
        StopCoroutine(ExecutarAcoesAleatorias());
        //new WaitForSeconds(0.3f);
        estaAtacando = false;
        animator.SetBool("Andar", true);
        SeguirPlayer();
    }

    // Corrotina que executa ações aleatórias (ataques e defesa)
    private IEnumerator ExecutarAcoesAleatorias()
    {
        while (estaAtacando)
        {
            int acao = Random.Range(0, 3); // Gera um número aleatório entre 0 e 3 (3 ações no total)
            animator.SetBool("Andar", false);
            switch (acao)
            {
                case 0:
                    new WaitForSeconds(1);
                    animator.SetTrigger("Ataque");
                    Debug.Log("Executando Ataque 1");
                    danoInimigo = 10;
                    break;
                case 1:
                     new WaitForSeconds(1);
                    animator.SetTrigger("Ataque2");
                    Debug.Log("Executando Ataque 2");
                    danoInimigo = 20;
                    break;
                case 2:
                    new WaitForSeconds(1);
                    animator.SetTrigger("Ataque3");
                    Debug.Log("Executando Ataque 3");
                    danoInimigo = 40;
                    break;
                case 3:
                    new WaitForSeconds(1);
                    animator.SetTrigger("Defesa");
                    Debug.Log("Executando Defesa");
                    break;
            }
            // Espera o tempo entre as ações antes de executar a próxima
            yield return new WaitForSeconds(tempoEntreAcoes);
            
        }
    }

    public void EstaSeguindo()
    {
        estaSeguindo = true;
    }

    public void NaoEstaSeguindo()
    {
        estaSeguindo = false;
    }


}
