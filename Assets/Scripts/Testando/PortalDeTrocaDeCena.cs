using UnityEngine;
using UnityEngine.SceneManagement;

public class TrocaCenaComObjeto : MonoBehaviour
{
    // Nome da cena para a qual o jogo ir� mudar
    public string nomeCenaDestino;

    // Transform que define a posi��o de spawn do jogador na nova cena
    public Transform pontoDeSpawn;

    // M�todo que � chamado quando o jogador toca em um objeto (por exemplo, atrav�s de colis�o)
    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto tocado � o jogador
        if (other.CompareTag("Player"))
        {
            // Salva o objeto tocado
            GameObject objetoToque = other.gameObject;

            // Remove o objeto da cena atual antes de carregar a nova cena
            DontDestroyOnLoad(objetoToque);

            // Muda para a nova cena
            SceneManager.sceneLoaded += (scene, mode) => OnSceneLoaded(scene, mode, objetoToque);
            SceneManager.LoadScene(nomeCenaDestino);
        }
    }

    // Este m�todo � chamado quando a nova cena for carregada
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode, GameObject objetoToque)
    {
        // Mova o objeto para a nova cena
        if (objetoToque != null)
        {
            // Preserva o objeto entre as cenas
            DontDestroyOnLoad(objetoToque);

            // Teleporta o objeto para a posi��o desejada na nova cena
            if (pontoDeSpawn != null)
            {
                objetoToque.transform.position = pontoDeSpawn.position;  // Coloca o player no ponto de spawn
                objetoToque.transform.rotation = pontoDeSpawn.rotation;  // Ajusta a rota��o para corresponder ao ponto de spawn
            }
            else
            {
                // Se n�o houver ponto de spawn, usa uma posi��o padr�o (por exemplo, (0, 1, 0))
                objetoToque.transform.position = new Vector3(0f, 1f, 0f);
            }
        }

        // Remove o evento do carregamento da cena ap�s a troca
        SceneManager.sceneLoaded -= (scene, mode) => OnSceneLoaded(scene, mode, objetoToque);
    }
}
