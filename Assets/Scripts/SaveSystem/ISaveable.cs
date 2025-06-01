public interface ISaveable
{
    void SaveGame();
    T LoadGame<T>();
    T LoadGameWithPath<T>(string path);
    void DeleteProgress();
}
