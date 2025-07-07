using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class ReelManager : MonoBehaviour
{
    // Fields for jackpot, red, orange,pink, yellow and green icons
    [SerializeField] private Sprite JackpotIcon;
    [SerializeField] private Sprite RedIcon;
    [SerializeField] private Sprite OrangeIcon;
    [SerializeField] private Sprite PinkIcon;
    [SerializeField] private Sprite YellowIcon;
    [SerializeField] private Sprite GreenIcon;
    
    // The blurred icons for the sprites
    [SerializeField] private Sprite BlurredJackpotIcon;
    [SerializeField] private Sprite BlurredRedIcon;
    [SerializeField] private Sprite BlurredOrangeIcon;
    [SerializeField] private Sprite BlurredPinkIcon;
    [SerializeField] private Sprite BlurredYellowIcon;
    [SerializeField] private Sprite BlurredGreenIcon;
    
    // Reels
    [SerializeField] private GameObject ReelLeft;
    [SerializeField] private GameObject ReelMiddle;
    [SerializeField] private GameObject ReelRight;
    
    // Distribution of slot symbols
    [SerializeField] private int ReelLeftJackpot;
    [SerializeField] private int ReelMiddleJackpot;
    [SerializeField] private int ReelRightJackpot;
    [SerializeField] private int NumYellow;
    [SerializeField] private int NumGreen;
    [SerializeField] private int NumRed;
    [SerializeField] private int NumOrange;
    [SerializeField] private int NumPink;

    [SerializeField] private int minReelRollLength;
    [SerializeField] private int maxReelRollLength;

    [SerializeField] private GameObject LineManager;
    [SerializeField] private GameObject MainGameFacade;
    
    private MoneyTracker moneyTracker;
    
    // Actual representation of the reels
    private List<String> _leftReel;
    private List<String> _middleReel;
    private List<String> _rightReel;

    private int _leftReelIndex;
    private int _middleReelIndex;
    private int _rightReelIndex;
    
    private ReelDisplayer _reelDisplayerLeft;
    private ReelDisplayer _reelDisplayerMiddle;
    private ReelDisplayer _reelDisplayerRight;
    
    // Map symbol names to sprites
    private Dictionary<string, Sprite> sprites;
    private Dictionary<string, Sprite> blurredSprites;
    
    private LineRenderChecker lineRenderChecker;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderChecker = LineManager.GetComponent<LineRenderChecker>();   
        moneyTracker = MainGameFacade.GetComponent<MoneyTracker>();
        sprites = new Dictionary<string, Sprite>
        {
            ["Jackpot"] = JackpotIcon,
            ["Green"] = GreenIcon,
            ["Yellow"] = YellowIcon,
            ["Red"] = RedIcon,
            ["Orange"] = OrangeIcon,
            ["Pink"] = PinkIcon
        };
        
        // Dict for blurred sprites
        blurredSprites = new Dictionary<string, Sprite>
        {
            ["Jackpot"] = BlurredJackpotIcon,
            ["Green"] = BlurredGreenIcon,
            ["Yellow"] = BlurredYellowIcon,
            ["Red"] = BlurredRedIcon,
            ["Orange"] = BlurredOrangeIcon,
            ["Pink"] = BlurredPinkIcon
        };
        
        
        // Get reel displayers
        _reelDisplayerLeft = ReelLeft.GetComponent<ReelDisplayer>();
        _reelDisplayerMiddle = ReelMiddle.GetComponent<ReelDisplayer>();
        _reelDisplayerRight = ReelRight.GetComponent<ReelDisplayer>();
        
        // For each reel, generate a randomized list with their given distributions
        _leftReel = GenerateReel(ReelLeftJackpot);
        _middleReel = GenerateReel(ReelMiddleJackpot);
        _rightReel = GenerateReel(ReelRightJackpot);
        
        // Init random starting index for each reel
        _leftReelIndex = UnityEngine.Random.Range(0, _leftReel.Count);
        _middleReelIndex = UnityEngine.Random.Range(0, _middleReel.Count);
        _rightReelIndex = UnityEngine.Random.Range(0, _rightReel.Count);
        
        // Adjust minReel and maxReel roll lengths  (dont include jackpot count for consistency)
        minReelRollLength = Mathf.Max(minReelRollLength, _leftReel.Count - ReelLeftJackpot);
        maxReelRollLength = Mathf.Min(maxReelRollLength, _leftReel.Count - ReelLeftJackpot);
        
        // Set initial slots
        AllReelsRoll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private List<String> GenerateReel(int numJackpotSymbols)
    {
        // Init dictionary
        Dictionary<string, int> symbolCounts = new Dictionary<string, int>
        {
            ["Jackpot"] = numJackpotSymbols,
            ["Green"] = NumGreen,
            ["Yellow"] = NumYellow,
            ["Red"] = NumRed,
            ["Orange"] = NumOrange,
            ["Pink"] = NumPink
        };
        
        // Init reel using dict
        List<String> reel = new List<String>();

        foreach (var pair in symbolCounts)
        {
            for (int i = 0; i < pair.Value; i++)
            {
                reel.Add(pair.Key);
            }
        }
        
        // Randomize reel
        // Randomize using Fisher-Yates shuffle
        System.Random rng = new System.Random();
        int n = reel.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            string temp = reel[k];
            reel[k] = reel[n];
            reel[n] = temp;
        }

        return reel;
    }

    private List<String> GetDisplayedReelsByIndex(List<String> reel, int numSymbols, int index)
    {
        List<String> reelWindow = new List<String>();
        for (int i = index; i < index + numSymbols; i++)
        {
            reelWindow.Add(reel[i % reel.Count]);
        }
        reelWindow.Reverse();
        //Debug.Log($"REELWINDOW window: {string.Join(",", reelWindow)}");
        return reelWindow;
    }

    private void DisplayReel(ReelDisplayer reelDisplayer, int windowStart, List<String> reel, bool blur)
    {   // First get reel window
        List<String> reelWindow = GetDisplayedReelsByIndex(reel, 3, windowStart);
        //Debug.Log($"Reel Window: {string.Join(", ", reelWindow.Count)}");
        // Get associated sprites
        var spriteDict = sprites;
        if (blur)
        {
            spriteDict = blurredSprites;
        }
        List<Sprite> reelSprites = new List<Sprite>();
        foreach (var symbol in reelWindow)
        {
            reelSprites.Add(spriteDict[symbol]);
        }
        //Debug.Log($"Reel Sprites: {string.Join(", ", reelSprites.Count)}");
        // Display reel
        reelDisplayer.DisplayReel(reelSprites);
    }
    
    public async void AllReelsRoll()
    {
        if (_leftReel == null || _middleReel == null || _rightReel == null)
        { return; }

        if (_leftReel.Count == 0 || _middleReel.Count == 0 || _rightReel.Count == 0)
        { return; }

        if (moneyTracker.getCurrentBet() <= 0 || moneyTracker.getCurrentEarnings() < moneyTracker.getCurrentBet())
        { return; }
        
        _leftReelIndex = (_leftReelIndex + UnityEngine.Random.Range(minReelRollLength, maxReelRollLength + 1)) % _leftReel.Count;
        _middleReelIndex = (_middleReelIndex + UnityEngine.Random.Range(minReelRollLength, maxReelRollLength + 1)) % _middleReel.Count;
        _rightReelIndex = (_rightReelIndex + UnityEngine.Random.Range(minReelRollLength, maxReelRollLength + 1)) % _rightReel.Count;;
        // Display reels
        DisplayReel(_reelDisplayerLeft, _leftReelIndex, _leftReel, false);
        DisplayReel(_reelDisplayerMiddle, _middleReelIndex, _middleReel, false);
        DisplayReel(_reelDisplayerRight, _rightReelIndex, _rightReel, false);
        
        // After animations done, alert line render manager to render correct lines
        var finalMult = await lineRenderChecker.DoScoreCheck(PrepareGrid(
            GetDisplayedReelsByIndex(_leftReel, 3, _leftReelIndex),
            GetDisplayedReelsByIndex(_middleReel, 3, _middleReelIndex),
            GetDisplayedReelsByIndex(_rightReel, 3, _rightReelIndex)
        ));

        moneyTracker.UpdateEarnings(finalMult);
    }

    private List<String> PrepareGrid(List<String> leftWindow, List<String> midWindow, List<String> rightWindow)
    {
        //Debug.Log($"Left window: {string.Join(",", leftWindow)}");
        //Debug.Log($"Middle window: {string.Join(",", midWindow)}");
        //Debug.Log($"Right window: {string.Join(",", rightWindow)}");
        // Initialize list with 9 null/empty values
        List<String> grid = Enumerable.Repeat(string.Empty, 9).ToList();
    
        // Fill left column (indices 0, 3, 6)
        for (int i = 0; i < 3; i++)
        {
            grid[i * 3] = leftWindow[i];
        }
    
        // Fill middle column (indices 1, 4, 7)
        for (int i = 0; i < 3; i++)
        {
            grid[(i * 3) + 1] = midWindow[i];
        }
    
        // Fill right column (indices 2, 5, 8)
        for (int i = 0; i < 3; i++)
        {
            grid[(i * 3) + 2] = rightWindow[i];
        }

        return grid;
    }

}
