using UnityEngine;
using UnityEngine.SceneManagement;

public class ContagemDeNpc : MonoBehaviour
{
    [Header("Configurações da Cena")]
    public int quantidadeParaTrocarCena = 10; // Quantidade de inimigos a serem destruídos para trocar de cena
    public string nomeCenaDestino; // Nome da cena para a qual será trocada

    private int inimigosDestruidos = 0;

    public void NpcDerrotado()
    {
        inimigosDestruidos++;
        Debug.Log("Inimigos destruídos: " + inimigosDestruidos);

        if (inimigosDestruidos >= quantidadeParaTrocarCena)
        {
            SceneManager.LoadScene(nomeCenaDestino);
        }
    }
}
