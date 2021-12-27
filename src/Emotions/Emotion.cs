using Data;

namespace Emotions;

class Emotion {
  public static readonly double LPSPerLevel = 0.1;
  public string Description
  { get; set; }

  public string Category
  { get; set; }
  
  public double Base 
  { get; set; }

  public Dictionary<string, Upgrade> Upgrades
  { get; set; }

  public Emotion(string description, string category, double Base, Dictionary<string, Upgrade> upgrades) {
    this.Description = description;
    this.Category = category;
    this.Base = Base;
    this.Upgrades = upgrades;
  }

  public double GetLPS(int level, double boost)
    => (this.Base + level * Emotion.LPSPerLevel) * boost;

  public Upgrade[] GetUnlockedUpgrades(EmotionData data)
    => this.Upgrades
      .Where(u => u.Key.Equals("Default") || data.Upgrades.Contains(u.Key))
      .Select(u => u.Value)
      .ToArray();
}