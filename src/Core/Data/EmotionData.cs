using Emotions;

namespace Data;

class EmotionData
{
  public int Level;

  public string[] Upgrades;

  public EmotionData() 
  {
    this.Level = 1;
    this.Upgrades = Array.Empty<string>();
  }

  public double GetBoost(string name, Upgrade[] upgrades) 
    => upgrades
      .Select(u => u.EvalBoost(this.Level))
      .Where(d => d.ContainsKey(name))
      .Select(d => d[name])
      .DefaultIfEmpty(1.0)
      .Aggregate((x, y) => x * y);
}