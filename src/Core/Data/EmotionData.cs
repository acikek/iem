using Emotions;

namespace Data;

class EmotionData
{
  public int Level;

  public string[] Upgrades;

  public EmotionData(int level, string[] upgrades) 
  {
    this.Level = level;
    this.Upgrades = upgrades;
  }

  public double GetBoost(string name, Upgrade[] upgrades) 
    => upgrades
      .Select(u => u.EvalBoost(this.Level))
      .Where(d => d.ContainsKey(name))
      .Select(d => d[name])
      .DefaultIfEmpty(1.0)
      .Aggregate((x, y) => x * y);

  public static EmotionData Default()
    => new EmotionData(1, Array.Empty<string>());
}