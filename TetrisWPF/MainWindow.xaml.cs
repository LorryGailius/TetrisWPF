using System;
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

                    Canvas.SetTop(imageControl, (r - 2) * cellSize);
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

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.GetTiles())
            {
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }


        private void DrawGameState(GameState gameState)
        {
            DrawGrid(gameState.Grid);
            DrawBlock(gameState.CurrentBlock);
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
            }

            DrawGameState(gameState);
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            DrawGameState(gameState);
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
