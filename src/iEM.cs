/*using Terminal.Gui;

Application.Init();

var menu = new MenuBar(new MenuBarItem[] {
  new MenuBarItem("_Hello", new MenuItem[] {
    new MenuItem("_Bye", "", () => {
      Application.RequestStop();
      Application.Shutdown();
    })
  })
});

var win = new Window("Bruh") {
  X = 0,
  Y = 1,
  Width = Dim.Fill(),
  Height = Dim.Fill()
};

Application.Top.Add(menu, win);

try { Application.Run(); } catch (Exception) { }*/

var game = Filesystem.LoadGame();

if (game is null)
  Environment.Exit(1);

Filesystem.SavePlayerData(game.Player);