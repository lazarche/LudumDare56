using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    public static GameManager Instance { get { return instance; } }
    #endregion

    [SerializeField] InputManager inputManager;
    [SerializeField] SpawningManager spawningManager;
    [SerializeField] GameObject player;

    [Header("End Panel")]
    [SerializeField] GameObject endPanel;
    [SerializeField] Image endPanelImg;

    [SerializeField] TextMeshProUGUI[] enPanelTexts;
    [SerializeField] Image[] endPanelImages;

    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] GameObject restarButton;
    public void GameOver()
    {
        spawningManager.enabled = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
            enemies[i].GetComponent<Enemy>().End();

        StartCoroutine(EndPanelSequence());
    }

    
    IEnumerator EndPanelSequence()
    {
        levelText.text = "Level: " + LevelManager.Instance.level;
        scoreText.text = "Score: " + ScoreManager.Instance.score;
        waveText.text = "Waves survived: " + (SpawningManager.Instance.GetCurrentWave()-1);

        Cursor.lockState = CursorLockMode.None;
        inputManager.End();
        endPanel.SetActive(true);

        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerMotor>().enabled = false;
        player.transform.DORotate(new Vector3(0, 0, -90), 1);
        player.transform.DOScaleY(player.transform.position.y - 0.75f, 0.9f);

        Color color = Color.black;
        color.a = 0.995f;
        endPanelImg.DOColor(color, 2);
        yield return new WaitForSecondsRealtime(1);
        UIManager.Instance.HideEverything();

        foreach (TextMeshProUGUI text in enPanelTexts)
        {
            Color c = text.color;
            c.a = 1;

            text.DOColor(c, 1.5f);
        }

        foreach(Image image in endPanelImages)
        {
            Color c = image.color;
            c.a = 1;

            image.DOColor(c, 1.5f);
        }

        yield return new WaitForSecondsRealtime(1);
        restarButton.SetActive(true);
        yield return null;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            RestartGame();
        }
    }
}
