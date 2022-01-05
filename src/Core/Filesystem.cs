using System.Text.Json;
using Data;
using YamlDotNet.Serialization;

namespace Core;

class Filesystem 
{
  public static string GameDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/iem_game";
  public static bool PatchDirectory(bool force) {
    try 
    {
      if (force || !Directory.Exists(GameDirectory)) 
      {
        // Create base directory
        Directory.CreateDirectory(GameDirectory);
        // Copy local game data to directory
        File.Copy("game.yml", GetPath("game"), true);
        return false;
      }
    } 
    catch (Exception e) 
    {
      Console.WriteLine($"Error while creating game directory: {e}");
    }

    return true;
  }

  public static string GetPath(string path)
    => $"{GameDirectory}/{path}.yml";

  public static GameData? LoadGameData()
    => new Deserializer().Deserialize<GameData>(File.ReadAllText(GetPath("game")));

  public static PlayerData? LoadPlayerData(GameData? data, bool patch) 
    => patch ? JsonSerializer.Deserialize<PlayerData>(File.ReadAllText(GetPath("save"))) : 
      (data is null ? null : data.DefaultPlayerData());

  public static Game? LoadGame(bool force) 
  {
    try 
    {
      bool patch = PatchDirectory(force);
      var gameData = LoadGameData();
      var playerData = LoadPlayerData(gameData, patch);
      return Game.Create(gameData, playerData);
    } 
    catch (Exception e) 
    {
      Console.WriteLine($"Error while loading game data: {e}");
      return null;
    }
  }

  public static void SavePlayerData(PlayerData data) 
  {
    try 
    {
      File.WriteAllText(GetPath("save"), new Serializer().Serialize(data));
    } 
    catch (Exception e) 
    {
      Console.WriteLine($"Error while saving player data: {e}");
    }
  }
}