using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{

    public Color color
    {
        get
        {
            return _color;
        }
        set
        {
            gameObject.GetComponent<SpriteRenderer>().color = value;
            _color = value;
        }
    }
    private Color _color;
    public bool activate;

    public void Destroy()
    {
        activate = false;
        this.gameObject.SetActive(false);
    }
}
