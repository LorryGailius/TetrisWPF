﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TetrisWPF.Blocks;

namespace TetrisWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Themes/Modern/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/TileRed.png", UriKind.Relative))

        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Themes/Modern/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Themes/Modern/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 100;
        private readonly int delayStep = 25;

        private GameState gameState = new GameState(22, 10);

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.Grid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDistance();

            foreach (Position p in block.GetTiles())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.GetTiles())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }


        private void DrawGameState(GameState gameState)
        {
            DrawGrid(gameState.Grid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.heldBlock);
            ScoreText.Text = $"Score: {gameState.Score}";
        }

        private async Task GameLoop()
        {
            DrawGameState(gameState);

            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayStep));
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                DrawGameState(gameState);
            }

            FinalScoreText.Text = $"Score: {gameState.Score}";
            GameOverScreen.Visibility = Visibility.Visible;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Up:
                case Key.W:
                    gameState.RotateBlockClockwise();
                    break;

                case Key.Left:
                case Key.A:
                    gameState.MoveBlockLeft();
                    break;

                case Key.Right:
                case Key.D:
                    gameState.MoveBlockRight();
                    break;

                case Key.Down:
                case Key.S:
                    gameState.MoveBlockDown();
                    break;

                case Key.LeftShift:
                case Key.RightShift:
                    gameState.HoldBlock();
                    break;

                case Key.Space:
                    gameState.DropBlock();
                    break;
            }

            DrawGameState(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState(22, 10);
            GameOverScreen.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}
