using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida : MonoBehaviour
{
    [SerializeField] private Image barraVidaSprite; // Sprite da barra de vida
    private Player player;

    void Start()
    {
        player = GetComponentInParent<Player>(); // Referência ao Player
    }

    void Update()
    {
        if (player != null)
        {
            AtualizarBarraDeVida(player.VidaAtual, player.VidaMaxima);
            SeguirPlayer();
        }
    }

    // Atualiza a barra de vida com base na vida atual
    public void AtualizarBarraDeVida(int vidaAtual, int vidaMaxima)
    {
        if (barraVidaSprite != null)
        {
            barraVidaSprite.fillAmount = (float)vidaAtual / vidaMaxima;
        }
    }

    // Faz a barra de vida seguir o Player e sempre se orientar para a câmera
    private void SeguirPlayer()
    {
        Camera camera = Camera.main;
        if (camera != null)
        {
            transform.position = player.transform.position + Vector3.up * 2; // Posição acima do Player
            transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        }
    }
}
