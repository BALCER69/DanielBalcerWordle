using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace DanielBalcerWordle

{
    public partial class MainPage : ContentPage
    {
        private string targetWord;
        private int currentRow = 0;
        private Entry[,] entryGrid = new Entry[6, 5];
        private List<string> validWords = new List<string>();

        public MainPage()
        {
            InitializeComponent();
            CreateGrid();
            LoadWordList();
        }
        private void CreateGrid()
        {
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();

            for (int row = 0; row < 6; row++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            for (int col = 0; col < 5; col++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
            }

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    var entry = new Entry
                    {
                        MaxLength = 1,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Keyboard = Keyboard.Text,
                        BackgroundColor = Colors.LightGray,
                        TextColor = Colors.Black,
                        HeightRequest = 60,
                        WidthRequest = 60,
                        FontSize = 24
                    };
                    entry.TextChanged += OnLetterTextChanged;
                    entryGrid[row, col] = entry;

                    var frame = new Frame
                    {
                        Content = entry,
                        BackgroundColor = Colors.Transparent,
                        BorderColor = Colors.Black,
                        CornerRadius = 5,
                        Padding = 0,
                        Margin = new Thickness(5)
                    };

                    Grid.SetRow(frame, row);
                    Grid.SetColumn(frame, col);
                    GameGrid.Children.Add(frame);
                }
            }
        }