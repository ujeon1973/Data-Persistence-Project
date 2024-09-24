using TMPro;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public Button startButton;
    public Button quitButton;
    public TMP_InputField yourIdInput;
    public TextMeshProUGUI textMeshProUGUI;
    public TextMeshProUGUI playGame;
    bool isUserCheck = false;

    void Start()
    {
        startButton.onClick.AddListener(StartOnClick);
        quitButton.onClick.AddListener(QuitOnClick);
        // playerScores ����Ʈ���� �ְ� ���� �÷��̾�� ���� ã��
        if (GameManager.Instance.playerScores.Count > 0)
        {
            GameManager.Instance.BestScore();
            textMeshProUGUI.text = $"BestScore : {GameManager.Instance.bestScoreUser} <color=green>{GameManager.Instance.bestScore}</color>";
        }
        else
        {
            textMeshProUGUI.text = $"BestScore : Not Yet";
        }
    }

    void StartOnClick()
    {
        if (yourIdInput.text != "")
        {
            if (!isUserCheck)
            {
                GameManager.Instance.currentUserName = yourIdInput.text;
                if (GameManager.Instance.playerScores.ContainsKey(GameManager.Instance.currentUserName))
                {
                    yourIdInput.text = "Reentry " + GameManager.Instance.currentUserName;
                }
                else
                {
                    GameManager.Instance.playerScores[GameManager.Instance.currentUserName] = 0; // ���ο� ���� ��� �� �ʱ� ���� 100 ����
                    GameManager.Instance.SaveScore();
                    yourIdInput.text = "Welcome " + GameManager.Instance.currentUserName;
                    
                }
                playGame.text = "Play Game";
                isUserCheck = true;
            }
            else
            {
                StartGame();
            }
        }
    }

    void QuitOnClick()
    {
        // ���� ���� ����
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // Unity �÷��̾ �����ϴ� ���� �ڵ�
#endif
    }

    void StartGame()
    {
        GameManager.Instance.playerScores["Saga"] = 0;
        SceneManager.LoadScene("main"); // "main"�� ���� �� �ε��� ���� �̸��Դϴ�.
    }
}
