using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] GameObject MenuCanvas;
    [SerializeField] GameObject GameCanvas;

    [SerializeField] GameObject[] everthing;
    [SerializeField] GameObject tutorial;

    [Header("Button")]
    [SerializeField] Button startGame;
    [SerializeField] Button exitGame;

    [Header("Managers")]
    [SerializeField] GameObject[] gameObjects;
    [SerializeField] Transform playerTransform;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void StartGame()
    {
        SoundManager.Instance.PlaySound("buttonClick");
        MenuCanvas.SetActive(false);
        GameCanvas.SetActive(true);

        Camera.main.transform.SetParent(playerTransform);

        foreach (var gameObject in gameObjects)
        {
            gameObject.SetActive(true);
        }
    }
    public void ShowTutorial()
    {
        tutorial.SetActive(true);
        foreach (var gameObject in everthing)
        {
            gameObject.SetActive(false);
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
