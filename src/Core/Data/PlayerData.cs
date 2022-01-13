namespace Data;

class PlayerData
{
  public Dictionary<string, EmotionData> Emotions;
  public double LEXP;

  public void AddEmotion(string name)
    => this.Emotions[name] = new EmotionData();

  public void BuyEmotion(string name, double cost)
  {
    this.LEXP -= cost;
    AddEmotion(name);
  }
  
  public void RemoveEmotion(string name)
    => this.Emotions.Remove(name);

  public bool HasEmotion(string name)
    => this.Emotions.ContainsKey(name);

  public void LevelEmotion(string name, int level)
    => this.Emotions[name].Level += level;
}