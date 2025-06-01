using System;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveSystem : MonoBehaviour
{
    public Transform PlayerTransform;
    public int Health;
    public int Score;

    public void SaveGame(Transform playerPos, int health, int score)
    {
        GameData gameData = new GameData();
        gameData.Position = new float[] { playerPos.position.x, playerPos.position.y, playerPos.position.z }; 
        gameData.Health = health;
        gameData.Score = score;

        string json = JsonUtility.ToJson(gameData);
        string path = Application.persistentDataPath + "/gameData.json";
        File.WriteAllText(path, json);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/gameData.json";

        if (File.Exists(path)){
            string json = File.ReadAllText(path);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
        }
    }
}
