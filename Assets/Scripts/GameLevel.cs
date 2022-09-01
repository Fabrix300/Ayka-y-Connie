public class GameLevel
{
    public string levelName;
    public string levelTitle;
    public int carrotsLeft;
    public int totalCarrots;
    public bool unlocked;
    public bool hasBeenPlayed;

    public GameLevel(
        string _levelName,
        string _levelTitle,
        int _carrotsLeft,
        int _totalCarrots,
        bool _unlocked,
        bool _hasBeenPlayed
        )
    {
        levelName = _levelName;
        levelTitle = _levelTitle;
        carrotsLeft = _carrotsLeft;
        totalCarrots = _totalCarrots;
        totalCarrots = _totalCarrots;
        unlocked = _unlocked;
        hasBeenPlayed = _hasBeenPlayed;
    }
}
