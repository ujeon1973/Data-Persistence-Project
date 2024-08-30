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
            Debug.LogError("JSON ����ȭ �Ǵ� ���� ���� ����: " + e.Message);
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
                Debug.LogError("JSON ������ȭ ����: " + e.Message);
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
    public static GameManager Instance; // �̱��� �ν��Ͻ�
    public string currentUserName;
    public Dictionary<string, int> playerScores;
    public string bestScoreUser;
    public int bestScore;

    private string filePath;

    void Awake()
    {
        // �̱��� �ν��Ͻ��� �̹� �����ϸ� �� ������Ʈ�� �ı��մϴ�.
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        // �̱��� �ν��Ͻ��� �����ϰ�, �� ��ȯ �ÿ��� �ı����� �ʵ��� �մϴ�.
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        playerScores = new Dictionary<string, int>();

        filePath = Application.dataPath + "/playerData.json";
        LoadScore(); // ���� ���� �� ������ �ε�
    }

    void Start()
    {
        bestScoreUser = "";
        bestScore = 0;
        if (playerScores == null)
        {
            Debug.Log("�ΰ��̴�");
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
        // playerScores ��ųʸ����� �ְ� ���� �÷��̾�� ���� ã��
        if (playerScores.Count > 0)
        {
            var topPlayer = playerScores.OrderByDescending(x => x.Value).First();
            bestScoreUser = topPlayer.Key;
            bestScore = topPlayer.Value;
        }
    }
}
