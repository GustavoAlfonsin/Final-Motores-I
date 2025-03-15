using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager master;

    public GameObject panelGameOver;
    // barra de vida
    // indicador de puntos
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
        UpdateHpUI(100);
    }

    public void UpdateHpUI(int currentHp)
    {
        Debug.Log($"Salud: {currentHp}");
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
}
