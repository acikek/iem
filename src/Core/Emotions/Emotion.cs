using Data;

namespace Emotions;

class Emotion
{
  public const double LPSPerLevel = 0.1;
  public string Description;
  public string Category;
  public double Base;
  public int Cost;
  public Dictionary<string, Upgrade> Upgrades;
  
  public double GetLPS(int level, double boost)
    => (this.Base + level * Emotion.LPSPerLevel) * boost;

  public Upgrade[] GetUnlockedUpgrades(EmotionData data)
    => this.Upgrades
      .Where(u => u.Key == "Default" || data.Upgrades.Contains(u.Key))
      .Select(u => u.Value)
      .ToArray();
}