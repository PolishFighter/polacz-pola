using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUI : MonoBehaviour
{
   public Text timer;
   public Text score;
   public Text scoreGameOver;
   public Text highScoreGameOver;
   public GameObject newHighScore;

   public Text levelName;
   public GameObject gameOverPanel;
   public GameObject gameplayPanel;

   public void DisableAllPanels()
   {
        gameOverPanel.SetActive(false);
        gameplayPanel.SetActive(false);
   }

}
