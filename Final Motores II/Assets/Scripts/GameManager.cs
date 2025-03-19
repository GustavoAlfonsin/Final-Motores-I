using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject panelGameOver;
    public GameObject endLevelPanel;
    
    public Image virus1Conseguido;
    public Image virus2Conseguido;
    private int cantVirusConseguidos;

    // barra de vida
    [SerializeField]private Slider slider;
    // indicador de puntos
    private int ptos;
    public TextMeshProUGUI txtPuntos;

    //Texto guia
    public TextMeshProUGUI txtAyuda;

    private bool GameOver;

    private void Start()
    {
        panelGameOver.SetActive(false);
        endLevelPanel.SetActive(false);
        cantVirusConseguidos = 0;
        ptos = 0;
        txtAyuda.text = "Con las teclas A y D podes avanzar \n y con la barra espaciadora podes disparar";
        txtPuntos.text = $"{0000}";
       
    }

    public void UpdateHpUI(int currentHp)
    {
        Debug.Log($"Salud: {currentHp}");
        slider.value = currentHp;
    }

    public void iniciarHP(int maxHP, int currentHp)
    {
        slider.maxValue = maxHP;
        slider.value = currentHp;
    }

    public void gameOver(float deathTime)
    {
        StartCoroutine(showDeath(deathTime));
    }

    private IEnumerator showDeath(float delay)
    {
        yield return new WaitForSeconds(delay);
        panelGameOver.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Volvemos a jugar");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void levelComplete()
    {
        Time.timeScale = 0f;
        endLevelPanel.SetActive(true);
    }

    public void showAdvise(string textAyuda)
    {
        txtAyuda.text = textAyuda;
    }

    public void takeVirus()
    {
        cantVirusConseguidos++;
        if (cantVirusConseguidos == 1)
        {
            virus1Conseguido.gameObject.SetActive(true);
        }else if (cantVirusConseguidos == 2)
        {
            virus1Conseguido.gameObject.SetActive(false);
            virus2Conseguido.gameObject.SetActive(true);
        }
        else
        {
            virus1Conseguido.gameObject.SetActive(false);
            virus2Conseguido.gameObject.SetActive(false);
            cantVirusConseguidos = 0;
        }
    }

    public void winPoints(int points)
    {
        ptos += points;
        txtPuntos.text = ptos.ToString(); 
    }

    public void goBackMenu()
    {
        SceneManager.LoadScene("Menu inicio");
    }
}
