using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContagemDeNpc : MonoBehaviour
{
    [Header("Configurações de Objetivo")]
    [Tooltip("Quantidade de NPCs 'Inimigos' que devem ser derrotados. Essa quantidade será ignorada se houver mais NPCs na cena.")]
    [SerializeField] private int npcsNecessarios = 6;
    [Tooltip("Nome da cena para a qual será mudada ao atingir o objetivo.")]
    [SerializeField] private string nomeCenaProxima;

    private int npcsDerrotados = 0;
    private List<Npc> npcsNaCena = new List<Npc>();

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
    public void NpcDerrotado()
    {
        npcsDerrotados++;
        Debug.Log("NPC Derrotado! Total de derrotados: " + npcsDerrotados + "/" + npcsNecessarios);

        // Verifica se todos os NPCs foram derrotados
        if (npcsDerrotados >= npcsNecessarios)
        {
            MudarCena();
        }
    }

    // Método para mudar a cena
    private void MudarCena()
    {
        if (!string.IsNullOrEmpty(nomeCenaProxima))
        {
            Debug.Log("Todos os NPCs necessários foram derrotados. Mudando para a cena: " + nomeCenaProxima);
            SceneManager.LoadScene(nomeCenaProxima);
        }
        else
        {
            Debug.LogWarning("O nome da próxima cena não foi definido!");
        }
    }
}
