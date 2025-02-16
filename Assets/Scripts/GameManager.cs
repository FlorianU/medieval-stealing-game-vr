using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }

   public float maxScore = 516.80f;
   public float currentScore = 0;
   public bool isPaused;
   public bool canInteract;
   public bool isTimerActive;
   private float currentTime;

   public GameObject pauseScreen;
   public GameObject gameOverScreen;
   public GameObject instructionsScreen;
   public TextMeshProUGUI scoreText;
   public TextMeshProUGUI timeText;
   public TextMeshProUGUI maxScoreText;
   public TextMeshProUGUI highScoreText;
   public TextMeshProUGUI endScoreText;
   public TextMeshProUGUI highScoreTimeText;

   private void Awake()
   {
      if (Instance != null && Instance != this) Destroy(this);
      else Instance = this;

      maxScoreText.text = maxScore.ToString("0.00") + " $";
   }

   // Start is called before the first frame update
   void Start()
   {
      TogglePause();
      instructionsScreen.SetActive(true);
   }

   // Update is called once per frame
   void Update()
   {
      if (isTimerActive)
      {
         currentTime = currentTime + Time.deltaTime;
      }

      if (Input.GetButtonDown("Cancel"))
      {
         TogglePauseScreen();
      }
   }

   public void StartGame()
   {
      canInteract = true;
      TogglePause();
      instructionsScreen.SetActive(false);
   }

   public void TogglePauseScreen()
   {
      TogglePause();
      pauseScreen.SetActive(isPaused);
   }


   public void TogglePause()
   {
      isPaused = !isPaused;
      isTimerActive = !isPaused;
      Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
      Cursor.visible = isPaused;
      Time.timeScale = isPaused ? 0 : 1;
   }

   public void IncreaseScore(float score)
   {
      currentScore += score;
      scoreText.text = currentScore.ToString("0.00") + " $";
      endScoreText.text = currentScore.ToString("0.00") + " $";
   }

   public void EndGame()
   {
      TogglePause();
      var highscore = PlayerPrefs.GetFloat("highscore");
      var highscoreTime = PlayerPrefs.GetFloat("highscoreTime");

      // Set highscore time for speedrunning
      if (currentScore == maxScore)
      {
         var newTime = currentTime;
         if (highscoreTime == 0 || highscoreTime > newTime)
         {
            highscoreTime = newTime;
            PlayerPrefs.SetFloat("highscoreTime", highscoreTime);
         }
      }

      if (highscoreTime > 0)
      {
         var formattedHighscoreTime = TimeSpan.FromSeconds(highscoreTime);
         highScoreTimeText.text = "Speedrun Record: " + formattedHighscoreTime.Minutes.ToString("00") + ":" + formattedHighscoreTime.Seconds.ToString("00");
      } else
      {
         highScoreTimeText.text = "Speedrun Record: none";
      }

      // set normal highscore
      if (currentScore > highscore)
      {
         highscore = currentScore;
         PlayerPrefs.SetFloat("highscore", highscore);
         PlayerPrefs.Save();
      }

      var formattedTime = TimeSpan.FromSeconds(highscoreTime);
      timeText.text = TimeSpan.FromSeconds(currentTime).Minutes.ToString("00") + ":" + TimeSpan.FromSeconds(currentTime).Seconds.ToString("00");
      highScoreText.text = "Highscore: " + highscore.ToString("0.00") + " $";
      gameOverScreen.SetActive(true);
   }

   public void DisableInteraction()
   {
      canInteract = false;
   }
}
