
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SelecionarPersonagens : MonoBehaviour
{
    public string nome;
    public int dano;
    public int defesa;
    public bool selecionado;
    public GameObject prefabPersonagem;
    [SerializeField] private TextMeshProUGUI statusDefesa;
    [SerializeField] private TextMeshProUGUI statusDano;
    [SerializeField] private TextMeshProUGUI statusNome;
    public string cenaDestino;
    public Transform pontoDeSpawn;
    private static GameObject personagemSelecionado;
    private static Transform pontoDeSpawnSelecionado;

    private void Start()
    {
        statusDefesa = GameObject.FindWithTag("StatusDefesa").GetComponent<TextMeshProUGUI>();
        statusDano = GameObject.FindWithTag("StatusDano").GetComponent<TextMeshProUGUI>();
        statusNome = GameObject.FindWithTag("StatusNome").GetComponent<TextMeshProUGUI>();
    }

    private void OnMouseEnter()
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        statusDefesa.text = "Defesa: " + defesa;
        statusDano.text = "Dano: " + dano;
        statusNome.text = "Nome: " + nome;
    }

    private void OnMouseExit()
    {
        if (!selecionado)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            statusDefesa.text = "";
            statusDano.text = "";
            statusNome.text = "";
        }
    }

    private void OnMouseDown()
    {
        transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        selecionado = true;
        personagemSelecionado = prefabPersonagem;
        pontoDeSpawnSelecionado = pontoDeSpawn;
        DontDestroyOnLoad(personagemSelecionado);
    }

    public void TrocarCena()
    {
        if (selecionado)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(cenaDestino);
        }
        else
        {
            Debug.Log("Nenhum personagem selecionado!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (personagemSelecionado != null)
        {
            if (pontoDeSpawnSelecionado != null)
            {
                Instantiate(personagemSelecionado, pontoDeSpawnSelecionado.position, pontoDeSpawnSelecionado.rotation);
            }
            else
            {
                Instantiate(personagemSelecionado, Vector3.zero, Quaternion.identity);
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
