using Emotions;
using Terminal.Gui;

namespace Data;

/// <summary>The data for a game instance.</summary>
class GameData
{
  /// <summary>The loaded and available Emotions in the game.</summary>
  public Dictionary<string, Emotion> Emotions;

  /// <summary>The emotions that a player has unlocked by default.</summary>
  public string[] DefaultEmotions;

  public PlayerData DefaultPlayerData()
    => new PlayerData {
      Emotions = this.DefaultEmotions.ToDictionary(key => key, value => new EmotionData()), 
      LEXP = 0.0
    };

  public Window GetEmotionBaseWindow(string name, View other)
    => new Window(name)
    {
      X = Pos.Right(other),
      Y = 1,
      Width = Dim.Fill(),
      Height = Dim.Fill()
    };

  public Window GetEmotionLockedWindow(string name, View other) {
    return GetEmotionBaseWindow(name, other);
  }
}