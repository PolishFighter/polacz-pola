using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUI : MonoBehaviour
{
   public Text timer;
   public Text score;
   public Text levelName;
   public GameObject nextLevelPanel;
   public GameObject gameOverPanel;
   public GameObject gameplayPanel;
   public GameObject pausePanel;

   public void DisableAllPanels()
   {
        nextLevelPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        pausePanel.SetActive(false);
   }

}
