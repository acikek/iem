using Core;
using Gui;

var game = Filesystem.LoadGame();

if (game is null)
  Environment.Exit(1);

new Display(game).Start();