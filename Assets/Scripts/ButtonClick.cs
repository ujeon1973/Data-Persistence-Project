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
        // playerScores 리스트에서 최고 점수 플레이어와 점수 찾기
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
                    GameManager.Instance.playerScores[GameManager.Instance.currentUserName] = 0; // 새로운 유저 등록 및 초기 점수 100 설정
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
        // 게임 완전 종료
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // Unity 플레이어를 종료하는 원본 코드
#endif
    }

    void StartGame()
    {
        GameManager.Instance.playerScores["Saga"] = 0;
        SceneManager.LoadScene("main"); // "main"은 종료 후 로드할 씬의 이름입니다.
    }
}
