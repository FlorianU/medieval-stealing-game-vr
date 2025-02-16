using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{

   private Button button;

   // Start is called before the first frame update
   void Start()
   {
      button = GetComponent<Button>();
      button.onClick.AddListener(ContinueGame);
   }

   public void ContinueGame()
   {
      GameManager.Instance.TogglePauseScreen();
   }


   // Update is called once per frame
   void Update()
   {

   }
}
