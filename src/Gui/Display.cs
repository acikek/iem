using Core;
using Data;
using Terminal.Gui;

namespace Gui;

class Display 
{
  public Game CoreGame;
  public Window Emotions;
  public MenuBar Menu;

  public Display(Game coreGame) 
  {
    Application.Init();

    this.CoreGame = coreGame;
    this.Emotions = CreateEmotions();
    this.Menu = CreateMenu();
  }

  public Window CreateEmotions()
    => new Window("Emotions") {
      X = 0,
      Y = 1,
      Width = Dim.Percent(50),
      Height = Dim.Fill() - 1
    };

  public MenuBar CreateMenu()
    => new MenuBar(new MenuBarItem[] {
      new MenuBarItem("_File", new MenuItem[] {
        new MenuItem("_Save", "Saves your current progress", () => {
          MessageBox.Query("Save", "Progess saved successfully.", "OK");
          Filesystem.SavePlayerData(this.CoreGame.Player);
        }),
        new MenuItem("_Quit", "", () => RequestQuit())
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

  public void AddEmotions() 
  {
    int i = 0;
    foreach ((string name, EmotionData data) in this.CoreGame.Player.Emotions) {
      this.Emotions.Add(new Label(name) {
        X = 1,
        Y = i + 1
      });
      i++;
    }
  }

  public void Start() 
  {
    Application.Top.Add(this.Menu, this.Emotions);
    AddEmotions();
    try { Application.Run(); } catch (Exception) { };
  }
}