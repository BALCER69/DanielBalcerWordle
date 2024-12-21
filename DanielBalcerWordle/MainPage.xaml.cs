public partial class MainPage : ContentPage
{
    private string targetWord;
    private int currentRow = 0;
    private Entry[,] entryGrid = new Entry[6, 5];
    private List<string> validWords = new List<string>();
    private List<GuessHistory> guessHistory = new List<GuessHistory>();

    private DateTime startTime;
    private DateTime endTime;

    public MainPage()
    {
        InitializeComponent();
        CreateGrid();
        LoadWordListAsync();
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

    private async void LoadWordListAsync()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string wordsText = await client.GetStringAsync("https://raw.githubusercontent.com/DonH-ITS/jsonfiles/main/words.txt");
                validWords = new List<string>(wordsText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
                validWords = validWords.FindAll(word => word.Length == 5).ConvertAll(word => word.Trim().ToUpper());

                Random random = new Random();
                targetWord = validWords[random.Next(validWords.Count)];
            }
        }
        catch (Exception)
        {
            DisplayAlert("Error", "Failed to load the word list.", "OK");
        }
    }

    private string GetCurrentGuess(int row)
    {
        string guess = string.Empty;
        for (int col = 0; col < 5; col++)
        {
            var entry = entryGrid[row, col];
            guess += entry.Text?.ToUpper() ?? string.Empty;
        }
        return guess;
    }

    private void ProvideFeedback(string guess, int row)
    {
        for (int col = 0; col < 5; col++)
        {
            var entry = entryGrid[row, col];
            char guessedLetter = guess[col];
            char targetLetter = targetWord[col];

            Device.BeginInvokeOnMainThread(() =>
            {
                if (guessedLetter == targetLetter)
                    entry.BackgroundColor = Colors.Green;
                else if (targetWord.Contains(guessedLetter))
                    entry.BackgroundColor = Colors.Yellow;
                else
                    entry.BackgroundColor = Colors.Gray;
            });
        }
    }

    private async void OnSubmitGuess(object sender, EventArgs e)
    {
        if (currentRow >= 6)
        {
            DisplayAlert("Game Over", $"The correct word was: {targetWord}", "OK");
            return;
        }

        string guess = GetCurrentGuess(currentRow);

        if (guess.Length != 5 || !validWords.Contains(guess.ToUpper()))
        {
            DisplayAlert("Invalid Guess", "Please enter a valid 5-letter word.", "OK");
            return;
        }

        if (guess.ToUpper() == targetWord)
        {
            DisplayAlert("Congratulations!", "You've guessed the correct word!", "OK");
            return;
        }

        ProvideFeedback(guess, currentRow);
        currentRow++;

        if (currentRow == 6 && guess.ToUpper() != targetWord)
            DisplayAlert("Game Over", $"The correct word was: {targetWord}", "OK");
    }

    private void OnNewGame(object sender, EventArgs e)
    {
        LoadWordListAsync();
        currentRow = 0;

        foreach (var entry in entryGrid)
        {
            entry.Text = string.Empty;
            entry.BackgroundColor = Colors.LightGray;
        }
    }
}
}
    
    public class GuessHistory
{
    public string Guess { get; set;
    }
    public List<Color> Feedback { get; set;
    }
    public DateTime DateTime { get; set;
    }
    public bool IsAnswer { get; set; 
    }
    public TimeSpan TotalTime { get; set;
    }
}
}