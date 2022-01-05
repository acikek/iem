using Core;
using Terminal.Gui;

namespace Gui;

class Display 
{
  public Game CoreGame;
  public Label LEXPCounter;
  public Label LPSCounter;
  public Window Main;
  public Window MessageBoard;
  public Window Emotions;
  public MenuBar Menu;

  public Display(Game coreGame)
  {
    Application.Init();

    this.CoreGame = coreGame;

    this.LEXPCounter = new Label() { X = 1, Y = 2 };
    this.LPSCounter = new Label() { X = 1, Y = 3 };

    this.Main = CreateMain();
    this.MessageBoard = CreateMessageBoard();
    this.Emotions = CreateEmotions();
    this.Menu = CreateMenu();

    this.CoreGame.GetTotalLPS();
  }

  public Window CreateMain() {
    var main = new Window("iEM")
    {
      X = 0,
      Y = 1,
      Width = Dim.Percent(30),
      Height = Dim.Percent(49)
    };

    var welcome = new Label("Welcome to Interactive Emotion Manager!")
    {
      X = Pos.Center()
    };

    main.Add(welcome);
    main.Add(this.LEXPCounter);
    main.Add(this.LPSCounter);

    return main;
  }

  public Window CreateMessageBoard() {
    var board = new Window("Message Board")
    {
      X = 0,
      Y = Pos.Bottom(this.Main),
      Width = Dim.Percent(30),
      Height = Dim.Fill()
    };

    return board;
  }

  public Window CreateEmotions() {
    var emotions = new Window("Emotions") 
    {
      X = Pos.Right(this.Main),
      Y = 1,
      Width = Dim.Percent(30),
      Height = Dim.Fill()
    };

    return emotions;
  }

  public MenuBar CreateMenu()
    => new MenuBar(new MenuBarItem[]
    {
      new MenuBarItem("_File", new MenuItem[]
      {
        new MenuItem("_Save", "Saves your current progress", () =>
        {
          MessageBox.Query("Save", "Progess saved successfully.", "OK");
          Filesystem.SavePlayerData(this.CoreGame.Player);
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

  /*public void AddEmotions() 
  {
    foreach (var item in this.CoreGame.Player.Emotions.Select((pair, i) => new { i, pair })) {
      (string name, EmotionData data) = item.pair;
      this.Emotions.Add(new Label(name) {
        X = 1,
        Y = item.i + 1
      });
    }
  }*/
  public void UpdateLEXP()
  {
    double lps = this.CoreGame.AddLEXP();
    this.LEXPCounter.Text = $"LEXP: {Math.Round(this.CoreGame.Player.LEXP, 2)}";
    this.LPSCounter.Text = $"LEXP/s: {Math.Round(lps, 2)}";
  }

  public void Start() 
  {
    Application.Top.Add(this.Menu, this.Main, this.MessageBoard, this.Emotions);
    Application.Top.Add(this.CoreGame.Data.GetEmotionBaseWindow("Happy", this.Emotions));

    UpdateLEXP();

    Application.MainLoop.AddTimeout(TimeSpan.FromSeconds(1), loop => 
    {
      UpdateLEXP();
      return true;
    });
    
    try { Application.Run(); } catch (Exception) { };
  }
}