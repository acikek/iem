using System.Text.Json;
using Data;

class Filesystem {
  public static string GameDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/iem_game";
  public static bool PatchDirectory() {
    try {
      if (!Directory.Exists(GameDirectory)) {
        // Create base directory
        Directory.CreateDirectory(GameDirectory);
        // Copy local game data to directory
        File.Copy("game.json", $"{GameDirectory}/game.json");
        return false;
      }
    } catch (Exception e) {
      Console.WriteLine($"Error while creating game directory: {e.Message}");
    }

    return true;
  }

  public static string Read(string path)
    => File.ReadAllText($"{GameDirectory}/{path}.json");

  public static GameData? LoadGameData()
    => JsonSerializer.Deserialize<GameData>(Read("game"));

  public static PlayerData? LoadPlayerData(GameData? data, bool patch) {
    if (patch)
      return JsonSerializer.Deserialize<PlayerData>(Read("save"));
    else
      return PlayerData.Default(data);
  }

  public static Game? LoadGame() {
    try {
      bool patch = PatchDirectory();
      var gameData = LoadGameData();
      var playerData = LoadPlayerData(gameData, patch);
      return Game.Create(gameData, playerData);
    } catch (Exception e) {
      Console.WriteLine($"Error while loading game data: {e.Message}");
      return null;
    }
  }

  public static void SavePlayerData(PlayerData data) {
    try {
      string content = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
      File.WriteAllText($"{GameDirectory}/save.json", content);
    } catch (Exception e) {
      Console.WriteLine($"Error while saving player data: {e.Message}");
    }
  }
}