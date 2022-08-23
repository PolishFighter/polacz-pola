using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public Text score;

    void Start()
    {
        score.text = GameManager.score.ToString();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}
