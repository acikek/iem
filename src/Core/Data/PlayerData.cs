namespace Data;

class PlayerData {
  public Dictionary<string, EmotionData> Emotions
  { get; set; }

  public PlayerData(Dictionary<string, EmotionData> emotions) {
    this.Emotions = emotions;
  }

  public static PlayerData? Default(GameData? data)
    => data is null ? null : new PlayerData(data.DefaultEmotions.ToDictionary(key => key, value => EmotionData.Default()));

  public void AddEmotion(string name)
    => this.Emotions[name] = new EmotionData(1, new string[] { });
  
  public void RemoveEmotion(string name)
    => this.Emotions.Remove(name);
}