using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraDeVida1 : MonoBehaviour
{
    private Transform myCamera;
    [SerializeField] private Image barraDeVida;
    // Start is called before the first frame update
    private void Awake() 
    {
        myCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + myCamera.forward);
    }

    public void AlteraBarraDeVida(int vidaAtual, int vida)
    {
        barraDeVida.fillAmount = (float)vidaAtual / vida;
    }
}
