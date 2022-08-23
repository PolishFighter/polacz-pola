using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public GameObject particleDestroy;
    public Color color;
    public Color selectedColor;
    public Color limitColor;

    public SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer.color = color;
    }

    public void SetDefaultColor()
    {
            spriteRenderer.color = color;
    }

    public void SetSelectedColor()
    {
        spriteRenderer.color = selectedColor;
    }

    public void SetLimitColor()
    {
        spriteRenderer.color = limitColor;
    }

    public void SetColor(Color c)
    {
            GetComponent<SpriteRenderer>().color = c;
    }

    public void Destroy()
    {
        Destroy(Instantiate(particleDestroy, transform.position, Quaternion.identity, transform.parent), 2f);
        Destroy(this.gameObject);
    }
}
