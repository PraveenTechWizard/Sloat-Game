using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineManager : MonoBehaviour
{
    [Header("Reels & Lever")]
    [SerializeField] private ReelController[] reels; // Ordered left to right (Reel_1, Reel_2, Reel_3)
    [SerializeField] private LeverController leverController; // Controls the lever pull & spring-back

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button[] betButtons;

    [Header("Game Settings")]
    [SerializeField] private int playerGold = 500;
    [SerializeField] private float baseSpinTime = 1.0f; // Base duration before staggered stops
    [SerializeField] private float reelStopDelay = 0.45f; // Extra spin time per reel from left to right
    [SerializeField] private float leverReturnDelay = 0.25f;
    
    [Header("Bet Button")]
    [SerializeField] private GameObject betButtonPanel;

    // Symbol IDs: 0 = Seven, 1 = Cherry, 2 = Bell, 3 = BAR
    private readonly Dictionary<int, int> _payoutMultipliers = new Dictionary<int, int>()
    {
        { 1, 3 },   // 3 Cherries = 3x bet
        { 2, 5 },   // 3 Bells    = 5x bet
        { 3, 10 },  // 3 BARs     = 10x bet
        { 0, 50 }   // 3 Sevens   = 50x bet (Jackpot)
    };

    private bool _isRoundActive;

    private void Start()
    {
        UpdateGoldUI();
        SetStatusText("PLACE YOUR BET");
    }

    // 
    public void PlaceBet(int betAmount)
    {
        if (_isRoundActive) return;

        if (betAmount <= 0)
        {
            Debug.LogWarning("Bet amount must be greater than zero.", this);
            return;
        }

        if (playerGold < betAmount)
        {
            SetStatusText("NOT ENOUGH GOLD!");
            return;
        }

        playerGold -= betAmount;
        UpdateGoldUI();
        StartCoroutine(ExecuteSpinRound(betAmount));
    }

    
    private IEnumerator ExecuteSpinRound(int currentBet)
    {
        // 
        betButtonPanel.SetActive(false);
        _isRoundActive = true;
        SetBetButtonsInteractable(false);
        SetStatusText("GOOD LUCK...");

        // 
        if (reels == null || reels.Length < 3)
        {
            Debug.LogError("Please assign all three reels in the Inspector.");

            _isRoundActive = false;
            SetBetButtonsInteractable(true);
            yield break;
        }

        foreach (ReelController reel in reels)
        {
            if (reel == null)
            {
                Debug.LogError("One or more ReelController references are missing.");

                _isRoundActive = false;
                SetBetButtonsInteractable(true);
                yield break;
            }
        }

        // 
        if (leverController != null)
        {
            leverController.PullLever();
        }

        //
        int[] results = new int[reels.Length];

        for (int i = 0; i < reels.Length; i++)
        {
            results[i] = Random.Range(0, 4);

            float stopTime = baseSpinTime + (i * reelStopDelay);

            reels[i].StartSpin(results[i], stopTime);
        }

        //  
        yield return new WaitForSeconds(leverReturnDelay);

        //  
        yield return new WaitUntil(AreAllReelsStopped);

        //  
        int[] finalResults = new int[reels.Length];

        for (int i = 0; i < reels.Length; i++)
        {
            finalResults[i] = reels[i].CurrentSymbolIndex;
        }

        // 
        /*Debug.Log(
            $"Reel 1: {finalResults[0]}, " +
            $"Reel 2: {finalResults[1]}, " +
            $"Reel 3: {finalResults[2]}"
        );*/

        // 
        EvaluateWin(finalResults, currentBet);

        // 
        _isRoundActive = false;
        SetBetButtonsInteractable(true);
    }


    private bool AreAllReelsStopped()
    {
        if (reels == null) return true;

        foreach (ReelController reel in reels)
        {
            if (reel != null && reel.IsSpinning)
            {
                return false;
            }
        }
        
        betButtonPanel.SetActive(true);
        
        return true;
        
       
    }

    private void EvaluateWin(int[] results, int betAmount)
    {
        if (results == null || results.Length < 3) return;

        // 
        if (results[0] == results[1] && results[1] == results[2])
        {
            int winningSymbol = results[0];
            int multiplier = _payoutMultipliers.TryGetValue(winningSymbol, out int mult) ? mult : 1;
            int winnings = betAmount * multiplier;

            playerGold += winnings;
            UpdateGoldUI();

            if (winningSymbol == 0)
            {
                SetStatusText($"JACKPOT!! +{winnings}G!");
            }
            else
            {
                SetStatusText($"WIN! +{winnings}G!");
            }
            return;
        }

        // 
        int cherryCount = 0;
        foreach (int symbolId in results)
        {
            if (symbolId == 1)
            {
                cherryCount++;
            }
        }

        if (cherryCount == 2)
        {
            playerGold += betAmount;
            UpdateGoldUI();
            SetStatusText($"2 CHERRIES! +{betAmount}G");
        }
        else
        {
            SetStatusText("TRY AGAIN");
        }
    }

    private void UpdateGoldUI()
    {
        if (goldText != null)
        {
            goldText.text = $"{playerGold}G";
        }
    }

    private void SetStatusText(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private void SetBetButtonsInteractable(bool isInteractable)
    {
        if (betButtons == null) return;

        foreach (Button button in betButtons)
        {
            if (button != null)
            {
                button.interactable = isInteractable;
            }
        }
    }
    
    // Exit the Game
    public void ExitGame()
    {
        Application.Quit();
    }
}