using Emotions;
using Gui;
using Terminal.Gui;

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
      Emotions = this.DefaultEmotions.ToDictionary(key => key, value => new EmotionData()), 
      LEXP = 0.0
    };

  /// <summary>Generates a default window for emotion windows to build off of.</summary>
  /// <param name="name">The name of the emotion.</param>
  /// <param name="other">The left-hand view to align this window with.</param>
  public (Window, View) GetEmotionBaseWindow(string name, View other)
  {
    var win = new Window(name)
    {
      X = Pos.Right(other),
      Y = 1,
      Width = Dim.Fill(),
      Height = Dim.Fill(),
      ColorScheme = Display.Colors
    };

    var desc = new Label(this.Emotions[name].Description)
    {
      X = Pos.Center(),
      Y = 0
    };

    win.Add(desc);

    return (win, desc);
  }
}