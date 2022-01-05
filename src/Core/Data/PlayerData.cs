namespace Data;

class PlayerData
{
  public Dictionary<string, EmotionData> Emotions;
  public double LEXP;

  public void AddEmotion(string name)
    => this.Emotions[name] = EmotionData.Default();
  
  public void RemoveEmotion(string name)
    => this.Emotions.Remove(name);

  public void LevelEmotion(string name, int level)
    => this.Emotions[name].Level += level;
}