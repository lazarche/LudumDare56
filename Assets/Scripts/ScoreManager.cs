using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
    static ScoreManager instance;
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
    public static ScoreManager Instance { get { return instance; } }
    #endregion

    public int score = 0;
    [SerializeField] TextMeshProUGUI scoreText;

    public void AddScore(int add)
    {
        score += add;

        scoreText.text = score + "";
        scoreText.transform.DOComplete();
        scoreText.transform.DOPunchScale(Vector3.one * 0.3f, 0.3f, 0, 1);
    }
}
