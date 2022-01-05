using Emotions;

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
      Emotions = this.DefaultEmotions.ToDictionary(key => key, value => EmotionData.Default()), 
      LEXP = 0.0
    };
}