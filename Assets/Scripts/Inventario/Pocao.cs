using UnityEngine;

public class Pocao : MonoBehaviour
{
    [SerializeField] private int vidaParaAumentar = 20;  // Quantidade que a vida m�xima e vida atual ir�o aumentar

    private bool jogadorNoRange = false;  // Verifica se o player est� no alcance da po��o

    private void Update()
    {
        if (jogadorNoRange && Input.GetKeyDown(KeyCode.E))
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            if (player != null)
            {
                player.AumentarVida(vidaParaAumentar);  // Chama a fun��o para aumentar a vida e a vida m�xima
                Destroy(gameObject);  // Destr�i a po��o ap�s ser usada
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNoRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNoRange = false;
        }
    }
}
