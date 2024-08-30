using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScore;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;

    private bool m_GameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            // GameManager �ν��Ͻ��� �����ϴ��� Ȯ��
            if (GameManager.Instance != null)
            {
                Debug.Log(GameManager.Instance);
                Debug.Log("1 : " + GameManager.Instance.currentUserName);
                Debug.Log("2 : " + GameManager.Instance.bestScoreUser);
                Debug.Log("3 : " + GameManager.Instance.bestScore);
                BestScore.text = $"BestScore : {GameManager.Instance.bestScoreUser} <color=green>{GameManager.Instance.bestScore}</color>";
            }
            else
            {
                // GameManager �ν��Ͻ��� ������, �ְ� ������ ǥ������ ����
                BestScore.text = "BestScore : Not Available";
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            Debug.Log("GameOver �����Դϴ�.");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("�����̽� Ű�� ���Ƚ��ϴ�.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Escape Ű�� ���Ƚ��ϴ�.");
                SceneManager.LoadScene("StartMenu");
            }
            else
            {
                Debug.Log("�ٸ� Ű�� ���Ȱų� �ƹ� Ű�� ������ �ʾҽ��ϴ�.");
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        // ���� ���� ����
        if (GameManager.Instance.currentUserName != null)
        {
            if (m_Points > GameManager.Instance.playerScores[GameManager.Instance.currentUserName])
            {
                GameManager.Instance.playerScores[GameManager.Instance.currentUserName] = m_Points;
                GameManager.Instance.SaveScore();
            }
        }
    }
}
