using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{

    public Color color;

    [HideInInspector]
    public bool activate;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    public void ResetColor()
    {
            GetComponent<SpriteRenderer>().color = color;
    }

    public void SetColor(Color c)
    {
            GetComponent<SpriteRenderer>().color = c;
    }

    public void Destroy()
    {
        activate = false;
        this.gameObject.SetActive(false);
    }
}
