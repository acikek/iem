using Emotions;

namespace Data;

/// <summary>The entire game including all of its data.</summary>
class Game {
  public GameData Data
  { get; set; }

  /// <summary>The game's player data.</summary>
  public PlayerData Player
  { get; set; }

  /// <summary>A cache of the LPS generated by each emotion. Updated when an individual emotion's LPS is calculated.</summary>
  /// <see cref="GetEmotionLPS"/>
  public Dictionary<string, double> LPSCache
  { get; private set; }

  public Game(GameData data, PlayerData player) {  
    this.Data = data;
    this.Player = player;
    this.LPSCache = new Dictionary<string, double>();
  }

  public static Game? Create(GameData? data, PlayerData? player)
    => data is null || player is null ? null : new Game(data, player);

  /// <summary>Calculates the boost for an emotion based on the unlocked upgrades of all the player's emotions.</summary>
  /// <param name="name">The name-ID of the emotion.</param>
  public double GetEmotionBoost(string name)
    => this.Player.Emotions
      .Select(pair => {
        // The upgrades of the emotion that the player has unlocked
        Upgrade[] unlockedUpgrades = this.Data.Emotions[pair.Key].GetUnlockedUpgrades(pair.Value);
        // Calculate and return boost
        return pair.Value.GetBoost(name, unlockedUpgrades);
      })
      .Aggregate((x, y) => x * y);

  /// <summary>Calculates an emotion's LPS based on its boost, data, and level.
  ///   Updates the LPS cache with the generated value.</summary>
  /// <param name="emotion">The emotion object (to use its data).</param>
  /// <param name="name">The name-ID of the emotion.</param>
  /// <param name="data">The player's data for the emotion.</param>
  /// <see cref="LPSCache"/>
  public double GetEmotionLPS(Emotion emotion, string name, EmotionData data) {
    double lps = emotion.GetLPS(data.Level, GetEmotionBoost(name));
    this.LPSCache[name] = lps;
    return lps;
  }

  /// <summary>Calculates the total LPS by doing so for each individual unlocked emotion.</summary>
  public double GetTotalLPS()
    => this.Player.Emotions
      .Select(pair => GetEmotionLPS(this.Data.Emotions[pair.Key], pair.Key, pair.Value))
      .Aggregate((x, y) => x + y);

  /// <summary>Calculates the total LPS using the cached values.</summary>
  /// <see cref="LPSCache"/>
  public double GetTotalCacheLPS()
    => this.LPSCache.Values.Aggregate((x, y) => x + y);
}