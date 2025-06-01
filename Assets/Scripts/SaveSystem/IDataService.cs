public interface IDataService
{
    bool SaveData<T>(string relativePath, T data, bool Encrypted);
    T LoadData<T>(string relativePath, bool Encrypted);
    bool DeleteData(string relativePath);
}
