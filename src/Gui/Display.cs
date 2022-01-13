using Core;
using Terminal.Gui;

namespace Gui;

class Display 
{
  public static ColorScheme Colors = new ColorScheme {
    Normal = Application.Driver.MakeAttribute(Color.White, Color.Blue),
    Focus = Application.Driver.MakeAttribute(Color.Black, Color.White),
    HotNormal = Application.Driver.MakeAttribute(Color.BrightYellow, Color.Blue),
    HotFocus = Application.Driver.MakeAttribute(Color.BrightYellow, Color.White),
  };

  public Game Core;
  public Label LEXPCounter;
  public Label LPSCounter;
  public Window Main;
  public Window MessageBoard;
  public Window Emotions;
  public Window SelectedEmotion;
  public MenuBar Menu;

  public Display(Game coreGame)
  {
    Application.Init();

    this.Core = coreGame;
    // Declare externally (update in main loop)
    this.LEXPCounter = new Label() { X = 1, Y = 2 };
    this.LPSCounter = new Label() { X = 1, Y = 3 };
    // Create all windows
    this.Main = CreateMain();
    this.MessageBoard = CreateMessageBoard();
    this.Emotions = CreateEmotions();
    this.SelectedEmotion = CreateSelectedEmotion();
    this.Menu = CreateMenu();
    // Refresh LPS cache
    this.Core.GetTotalLPS();
  }

  public Window CreateMain() 
  {
    var main = new Window("iEM")
    {
      X = 0,
      Y = 1,
      Width = Dim.Percent(30),
      Height = Dim.Percent(49),
      ColorScheme = Colors
    };

    var welcome = new Label("Welcome to Interactive Emotion Manager!")
    {
      X = Pos.Center()
    };

    main.Add(
      welcome,
      this.LEXPCounter,
      this.LPSCounter
    );

    return main;
  }

  public Window CreateMessageBoard()
  {
    var board = new Window("Message Board")
    {
      X = 0,
      Y = Pos.Bottom(this.Main),
      Width = Dim.Percent(30),
      Height = Dim.Fill(),
      ColorScheme = Colors
    };

    return board;
  }

  public Window CreateEmotions() 
  {
    var emotions = new Window("Emotions") 
    {
      X = Pos.Right(this.Main),
      Y = 1,
      Width = Dim.Percent(30),
      Height = Dim.Fill(),
      ColorScheme = Colors
    };

    var list = new ListView(this.Core.Data.Emotions.Keys.ToArray())
    {
      X = 1,
      Y = 1,
      Width = Dim.Fill(),
      Height = Dim.Fill()
    };

    list.OpenSelectedItem += OpenEmotionWindow;

    emotions.Add(list);

    return emotions;
  }

  public Window CreateSelectedEmotion() 
  {
    var selected = new Window("Selected Emotion")
    {
      X = Pos.Right(this.Emotions),
      Y = 1,
      Width = Dim.Fill(),
      Height = Dim.Fill(),
      ColorScheme = Colors
    };

    selected.Add(new Label("Select an emotion to bring up buy\noptions and extra information!") 
    {
      X = Pos.Center(),
      Y = Pos.Center()
    });

    return selected;
  }

  public MenuBar CreateMenu()
    => new MenuBar(new MenuBarItem[]
    {
      new MenuBarItem("_File", new MenuItem[]
      {
        new MenuItem("_Save", "Saves your current progress", () =>
        {
          MessageBox.Query("Save", "Progess saved successfully.", "OK");
          Filesystem.SavePlayerData(this.Core.Player);
        }),
        new MenuItem("_Quit", "", () => RequestQuit())
      }),
      new MenuBarItem("_Theme", new MenuItem[]
      {

      })
    });

  public void RequestQuit() 
  {
    int n = MessageBox.Query("Quit", "Are you sure you want to quit?\nAll unsaved progress will be lost.", "No", "Yes");
    if (n == 1) {
      Application.RequestStop();
      Application.Shutdown();
    }
  }

  /// <summary>Generates a "locked" window for an emotion where the player can buy it.</summary>
  /// <param name="name">The name of the emotion</param>
  /// <param name="other">The left-hand view to align this window with.</param>
  public Window GetEmotionLockedWindow(string name, View other) 
  {
    var win = this.Core.Data.GetEmotionBaseWindow(name, other);
    var emotion = this.Core.Data.Emotions[name];

    var cost = new Label($"Cost: {Game.GetCost(emotion.Cost)}")
    {
      X = 1,
      Y = 1
    };

    var buy = this.Core.GetBuyButton(Pos.Right(cost) + 1, 1);

    buy.Clicked += () =>
    {
      if (this.Core.BuyQuery(win, buy, "Emotion", $"Do you want to buy {name} for {Game.GetCost(emotion.Cost)}?", emotion.Cost))
      {
        this.Core.Player.BuyEmotion(name, emotion.Cost);
        ReplaceEmotionWindow(name);
      }
    };

    win.Add(cost, buy);
    return win;
  }

  public Window GetEmotionWindow(string name, View other)
  {
    return this.Core.Data.GetEmotionBaseWindow(name, other);
  }

  public void ReplaceEmotionWindow(string name)
  {
    Application.Top.Remove(this.SelectedEmotion);
    this.SelectedEmotion = this.Core.Player.HasEmotion(name) ?
      GetEmotionWindow(name, this.Emotions) :
      GetEmotionLockedWindow(name, this.Emotions);
    Application.Top.Add(this.SelectedEmotion);
  }

  public void OpenEmotionWindow(ListViewItemEventArgs args) 
  {
    ReplaceEmotionWindow(args.Value.ToString() ?? "Emotion");
  }

  public void UpdateLEXP()
  {
    double lps = this.Core.AddLEXP();
    this.LEXPCounter.Text = $"LEXP: {Math.Round(this.Core.Player.LEXP, 2)}";
    this.LPSCounter.Text = $"LEXP/s: {Math.Round(lps, 2)}";
  }

  public void Start() 
  {
    Application.Top.Add(
      this.Menu, 
      this.Main, 
      this.MessageBoard, 
      this.Emotions,
      this.SelectedEmotion
    );

    UpdateLEXP();

    Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), loop => 
    {
      UpdateLEXP();
      return true;
    });
    
    try { Application.Run(); } catch (Exception) { };
  }
}