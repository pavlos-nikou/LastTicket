using System;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour
{
    public static GameTracker Instance { get; private set; }

    // --- Session ---
    public float SessionStartTime { get; private set; }
    public float TotalPlayTime => Time.realtimeSinceStartup - SessionStartTime;

    // --- Interactions ---
    private Dictionary<string, int> interactionCounts = new();
    private List<string> interactionLog = new();         // ordered history

    // --- Story ---
    private HashSet<string> discoveredClues = new();
    private HashSet<string> completedEvents = new();
    private Dictionary<string, string> playerChoices = new(); // event -> choice made

    // --- Computer ---
    public int PasswordAttempts { get; private set; }
    public bool IsLoggedIn { get; private set; }

    // --- Stats ---
    public int TotalInteractions { get; private set; }
    public string LastInteracted { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // persists across scenes
        SessionStartTime = Time.realtimeSinceStartup;
    }

    // -------------------------------------------------------
    // INTERACTIONS
    // -------------------------------------------------------

    public void TrackInteraction(string objectName)
    {
        TotalInteractions++;
        LastInteracted = objectName;

        if (!interactionCounts.ContainsKey(objectName))
            interactionCounts[objectName] = 0;
        interactionCounts[objectName]++;

        string entry = $"[{FormatTime(TotalPlayTime)}] Interacted: {objectName} (x{interactionCounts[objectName]})";
        interactionLog.Add(entry);

        Debug.Log(entry);
    }

    public int GetInteractionCount(string objectName)
    {
        return interactionCounts.TryGetValue(objectName, out int count) ? count : 0;
    }

    public bool HasInteracted(string objectName)
    {
        return interactionCounts.ContainsKey(objectName);
    }

    // -------------------------------------------------------
    // CLUES & STORY EVENTS
    // -------------------------------------------------------

    public void DiscoverClue(string clueName)
    {
        if (discoveredClues.Add(clueName)) // Add returns false if already present
        {
            string entry = $"[{FormatTime(TotalPlayTime)}] Clue discovered: {clueName}";
            interactionLog.Add(entry);
            Debug.Log(entry);
        }
    }

    public bool HasDiscovered(string clueName) => discoveredClues.Contains(clueName);
    public int TotalCluesFound => discoveredClues.Count;

    public void CompleteEvent(string eventName)
    {
        if (completedEvents.Add(eventName))
        {
            string entry = $"[{FormatTime(TotalPlayTime)}] Event completed: {eventName}";
            interactionLog.Add(entry);
            Debug.Log(entry);
        }
    }

    public bool IsEventComplete(string eventName) => completedEvents.Contains(eventName);

    public void RecordChoice(string eventName, string choice)
    {
        playerChoices[eventName] = choice;
        string entry = $"[{FormatTime(TotalPlayTime)}] Choice made: [{eventName}] -> {choice}";
        interactionLog.Add(entry);
        Debug.Log(entry);
    }

    public string GetChoice(string eventName)
    {
        return playerChoices.TryGetValue(eventName, out string choice) ? choice : null;
    }

    // -------------------------------------------------------
    // COMPUTER SPECIFIC
    // -------------------------------------------------------

    public void TrackPasswordAttempt(bool success)
    {
        PasswordAttempts++;
        if (success)
        {
            IsLoggedIn = true;
            CompleteEvent("ComputerLogin");
        }

        string result = success ? "SUCCESS" : "FAILED";
        interactionLog.Add($"[{FormatTime(TotalPlayTime)}] Password attempt #{PasswordAttempts}: {result}");
    }

    // -------------------------------------------------------
    // SUMMARY
    // -------------------------------------------------------

    public void PrintSummary()
    {
        Debug.Log("===== GAME TRACKER SUMMARY =====");
        Debug.Log($"Play time: {FormatTime(TotalPlayTime)}");
        Debug.Log($"Total interactions: {TotalInteractions}");
        Debug.Log($"Clues found: {TotalCluesFound}");
        Debug.Log($"Password attempts: {PasswordAttempts}");
        Debug.Log("--- Interaction Counts ---");
        foreach (var kvp in interactionCounts)
            Debug.Log($"  {kvp.Key}: {kvp.Value}x");
        Debug.Log("--- Choices Made ---");
        foreach (var kvp in playerChoices)
            Debug.Log($"  {kvp.Key}: {kvp.Value}");
        Debug.Log("--- Full Log ---");
        foreach (var entry in interactionLog)
            Debug.Log(entry);
    }

    // -------------------------------------------------------
    // UTILS
    // -------------------------------------------------------

    private string FormatTime(float seconds)
    {
        TimeSpan t = TimeSpan.FromSeconds(seconds);
        return $"{t.Minutes:D2}:{t.Seconds:D2}";
    }
}