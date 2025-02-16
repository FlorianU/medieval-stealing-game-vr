using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{

   private Button button;

   // Start is called before the first frame update
   void Start()
   {
      button = GetComponent<Button>();
      button.onClick.AddListener(RestartGame);
   }

   public void RestartGame()
   {
      Debug.Log(gameObject.name + " was clicked");

      SceneManager.LoadScene("World1", LoadSceneMode.Single);
      Time.timeScale = 1;
   }


   // Update is called once per frame
   void Update()
   {

   }
}
