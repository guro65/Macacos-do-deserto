using UnityEngine;
using UnityEngine.SceneManagement;

public class ContagemDeNpc : MonoBehaviour
{
<<<<<<< HEAD
    [Header("Configurações da Cena")]
    public int quantidadeParaTrocarCena = 10; // Quantidade de inimigos a serem destruídos para trocar de cena
    public string nomeCenaDestino; // Nome da cena para a qual será trocada
=======
    [Header("Configurações de Objetivo")]
    [Tooltip("Quantidade de NPCs 'Inimigos' que devem ser derrotados. Essa quantidade será ignorada se houver mais NPCs na cena.")]
    [SerializeField] private int npcsNecessarios = 6;
    [Tooltip("Nome da cena para a qual será mudada ao atingir o objetivo.")]
    [SerializeField] private string nomeCenaProxima;
>>>>>>> 07b6ce3ac697fbe4ebc70305f271697b1d7ad61e

    private int inimigosDestruidos = 0;

<<<<<<< HEAD
=======
    private void Start()
    {
        // Encontra todos os NPCs na cena com a tag "Inimigo"
        Npc[] npcs = FindObjectsOfType<Npc>();
        foreach (var npc in npcs)
        {
            npcsNaCena.Add(npc);
            npcsDerrotados = 0;
        }

        // Define a quantidade necessária com base na quantidade total de NPCs na cena
        npcsNecessarios = npcsNaCena.Count;
    }

    // Método para ser chamado quando um NPC é derrotado
>>>>>>> 07b6ce3ac697fbe4ebc70305f271697b1d7ad61e
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
