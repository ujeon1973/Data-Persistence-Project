using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using System;

[System.Serializable]
public class PlayerScore
{
    public string playerName;
    public int score;
}

[System.Serializable]
public class SaveData
{
    public List<PlayerScore> playerScores;

    public void Save(string filePath)
    {
        try
        {
            string jsonData = JsonUtility.ToJson(this);
            File.WriteAllText(filePath, jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError("JSON 직렬화 또는 파일 저장 오류: " + e.Message);
        }
    }

    public static SaveData Load(string filePath)
    {
        if (File.Exists(filePath))
        {
            try
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonUtility.FromJson<SaveData>(jsonData);
            }
            catch (Exception e)
            {
                Debug.LogError("JSON 역직렬화 오류: " + e.Message);
            }
        }
        return new SaveData { playerScores = new List<PlayerScore>() };
    }

    public Dictionary<string, int> ToDictionary()
    {
        return playerScores.ToDictionary(ps => ps.playerName, ps => ps.score);
    }

    public void FromDictionary(Dictionary<string, int> dictionary)
    {
        playerScores = dictionary.Select(kvp => new PlayerScore { playerName = kvp.Key, score = kvp.Value }).ToList();
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤 인스턴스
    public string currentUserName;
    public Dictionary<string, int> playerScores;
    public string bestScoreUser;
    public int bestScore;

    private string filePath;

    void Awake()
    {
        // 싱글톤 인스턴스가 이미 존재하면 이 오브젝트를 파괴합니다.
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        // 싱글톤 인스턴스를 설정하고, 씬 전환 시에도 파괴되지 않도록 합니다.
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        playerScores = new Dictionary<string, int>();

        filePath = Application.dataPath + "/playerData.json";
        LoadScore(); // 게임 시작 시 데이터 로드
    }

    void Start()
    {
        bestScoreUser = "";
        bestScore = 0;
        if (playerScores == null)
        {
            Debug.Log("널값이다");
        }
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.FromDictionary(playerScores);
        data.Save(filePath);
    }

    public void LoadScore()
    {
        SaveData data = SaveData.Load(filePath);
        playerScores = data.ToDictionary();
    }

    public void BestScore()
    {
        // playerScores 딕셔너리에서 최고 점수 플레이어와 점수 찾기
        if (playerScores.Count > 0)
        {
            var topPlayer = playerScores.OrderByDescending(x => x.Value).First();
            bestScoreUser = topPlayer.Key;
            bestScore = topPlayer.Value;
        }
    }
}
