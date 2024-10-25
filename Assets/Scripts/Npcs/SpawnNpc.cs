using System.Collections.Generic;
using UnityEngine;

public class SpawnNpc : MonoBehaviour
{
    [Header("Configurações de Prefabs")]
    [Tooltip("Lista de prefabs que serão gerados aleatoriamente.")]
    [SerializeField] private List<GameObject> prefabsParaGerar;

    [Header("Configurações de Tempo")]
    [Tooltip("Tempo mínimo entre a geração dos prefabs.")]
    [SerializeField] private float intervaloMinimo = 1.0f;
    [Tooltip("Tempo máximo entre a geração dos prefabs.")]
    [SerializeField] private float intervaloMaximo = 3.0f;

    [Header("Configurações de Posição")]
    [Tooltip("Área mínima de geração.")]
    [SerializeField] private Vector3 areaMinima;
    [Tooltip("Área máxima de geração.")]
    [SerializeField] private Vector3 areaMaxima;

    private void Start()
    {
        IniciarGeracao();
    }

    private void IniciarGeracao()
    {
        // Define um tempo aleatório para a próxima geração
        float tempoParaGerar = Random.Range(intervaloMinimo, intervaloMaximo);
        Invoke(nameof(GerarPrefabAleatorio), tempoParaGerar);
    }

    private void GerarPrefabAleatorio()
    {
        if (prefabsParaGerar.Count == 0)
        {
            Debug.LogWarning("Nenhum prefab foi definido na lista!");
            return;
        }

        // Escolhe um prefab aleatoriamente da lista
        GameObject prefabEscolhido = prefabsParaGerar[Random.Range(0, prefabsParaGerar.Count)];

        // Define uma posição aleatória dentro da área especificada
        Vector3 posicaoAleatoria = new Vector3(
            Random.Range(areaMinima.x, areaMaxima.x),
            Random.Range(areaMinima.y, areaMaxima.y),
            Random.Range(areaMinima.z, areaMaxima.z)
        );

        // Instancia o prefab na posição escolhida
        Instantiate(prefabEscolhido, posicaoAleatoria, Quaternion.identity);

        // Reinicia a geração
        IniciarGeracao();
    }
}
