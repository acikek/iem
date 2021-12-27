using Emotions;

namespace Data;

/// <summary>The data for a game instance.</summary>
class GameData {
  /// <summary>The loaded and available Emotions in the game.</summary>
  public Dictionary<string, Emotion> Emotions
  { get; set; }

  /// <summary>The emotions that a player has unlocked by default.</summary>
  public string[] DefaultEmotions
  { get; set; }

  public GameData(Dictionary<string, Emotion> emotions, string[] defaultEmotions) {
    this.Emotions = emotions;
    this.DefaultEmotions = defaultEmotions;
  }
}