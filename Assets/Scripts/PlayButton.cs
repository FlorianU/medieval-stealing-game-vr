using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{

   private Button button;

   // Start is called before the first frame update
   void Start()
   {
      button = GetComponent<Button>();
      button.onClick.AddListener(StartGame);
   }

   public void StartGame()
   {
      GameManager.Instance.StartGame();
   }


   // Update is called once per frame
   void Update()
   {

   }
}
