using System;
using System.IO;
using UnityEngine;

public class JsonDataService : IDataService
{
    public bool SaveData<T>(string relativePath, T data, bool Encrypted)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string json = JsonUtility.ToJson(data);
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, json);
            return true;
        }
        catch(Exception e)
        {
            Debug.LogError($"Não rolou de salvar o jogo, motivo: {e.Message} {e.StackTrace}");
            return false;
        }

    }
    
    public T LoadData<T>(string relativePath, bool Encrypted)
    {
        string path = Application.persistentDataPath + relativePath;

        if(!File.Exists(path))
        {
            Debug.LogError($"Nao rolou de carregar o arquivo no caminho: {path}. Arquivo nao existe.");
            throw new FileNotFoundException($"{path} nao existe");
        }     

        try
        {
            T data = JsonUtility.FromJson<T>(File.ReadAllText(path));
            return data;
        }   
        catch(Exception e)
        {
            Debug.LogError($"Não rolou carregar o jogo salvo, motivo: {e.Message} {e.StackTrace}");
            throw e;
        }
    }

    public bool DeleteData(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log($"Arquivo deletado com sucesso: {path}");
                return true;
            }
            else
            {
                Debug.LogWarning($"Tentativa de deletar um arquivo inexistente: {path}");
                return false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Erro ao deletar arquivo: {e.Message} {e.StackTrace}");
            return false;
        }
    }

}
