using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParedEspecialController : MonoBehaviour
{
    [SerializeField] private LeverController leverControl;
    [SerializeField] private WallColor colorPared;
    private SpriteRenderer _sprite;
    public bool encendido;



    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        encendido = true;
        gameObject.SetActive(encendido);
        if (colorPared == WallColor.Blue)
        {
            _sprite.color = Color.blue;
        }else if (colorPared == WallColor.Red)
        {
            _sprite.color = Color.red;
        }
        else
        {
            _sprite.color = Color.green;
        }
    }
    
    public void chaceStateWall(bool openSign)
    {
        encendido = openSign;
        gameObject.SetActive(encendido);
    }
}

public enum WallColor
{
    Red,
    Green,
    Blue
}
