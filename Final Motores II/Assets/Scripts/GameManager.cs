using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager master;

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
    // indicador de puertas abiertas
    private bool GameOver;

    private void Awake()
    {
        if (master == null)
        {
            master = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        panelGameOver.SetActive(false);
        endLevelPanel.SetActive(false);
        cantVirusConseguidos = 0;
        ptos = 0;
        txtPuntos.text = $"{0000}";
        //slider = GetComponent<Slider>();
        //slider.maxValue = 100;
    }

    public void UpdateHpUI(int currentHp)
    {
        Debug.Log($"Salud: {currentHp}");
        slider.value = currentHp;
    }

    public void iniciarHP(int maxHP)
    {
        slider.maxValue = maxHP;
        slider.value = maxHP;
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
        }
    }

    public void winPoints(int points)
    {
        ptos += points;
        txtPuntos.text = ptos.ToString(); 
    }
}
