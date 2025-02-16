using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{

    private Button button;
    private GameObject startScreenCanvas;

    // Start is called before the first frame update
    void Start()
    {
        startScreenCanvas = GameObject.Find("StartScreenCanvas");
        button = GetComponent<Button>();
        button.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        Debug.Log(gameObject.name + " was clicked");

        int Index = SceneManager.sceneCount;
        var operation = SceneManager.LoadSceneAsync("World1", LoadSceneMode.Additive);
        //operation.completed += (s) =>
        //{
        //    // text here
        //};

        startScreenCanvas.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
