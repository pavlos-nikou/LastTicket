using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TerminalGame : MonoBehaviour
{
    int wordLength = 5;
    int wordCount = 8;
    int maxAttempts = 4;

    string password;
    int attemptsLeft;
    bool gameOver;
    string logText = "";
    string hoverWord = "";

    char[] grid;
    List<int> wordStarts = new List<int>();
    List<string> words = new List<string>();
    List<int> bracketOpen = new List<int>();
    List<int> bracketClose = new List<int>();
    HashSet<string> removedDuds = new HashSet<string>();

    GUIStyle termStyle, dimStyle, brightStyle, hoverStyle, btnStyle, logStyle, titleStyle;
    bool stylesReady = false;
    int lastW, lastH;

    [SerializeField] GameObject terminalRoot;

    static readonly string[] Words5 = {
        "ABORT","ADMIN","AGENT","ALERT","ALIGN","ALLOY","ALTER","ANNEX",
        "ARMOR","ARRAY","AUDIT","BADGE","BATCH","BLADE","BLAST","BLAZE",
        "BLEND","BLOCK","BLOOD","BLOOM","BLOWN","BLUNT","BOARD","BOOST",
        "BRAIN","BRAVE","BREAK","BRICK","BRIEF","BRING","BRINK","BROAD",
        "BRUSH","BUILD","BYTES","CACHE","CARGO","CATCH","CAUSE","CHAIN",
        "CHAOS","CHECK","CHIEF","CHORD","CLASS","CLEAN","CLEAR","CLIMB",
        "CLOCK","CLONE","CLOSE","CLOUD","COAST","CODES","COMET","CORAL",
        "COULD","COUNT","COVER","CRAFT","CRASH","CRAWL","CREED","CRIME"
    };
    static readonly string[] Words3 = {
        "ACE","AGE","AID","AIM","AIR","ARC","ARM","ART","ASH","AWE",
        "AXE","BAG","BAN","BAR","BAT","BED","BIT","BOX","BUG","BUS",
        "CAB","CAN","CAP","CAR","CAT","COG","COP","COT","CRY","CUB",
        "CUP","CUT","DIM","DIP","DOC","DOG","DOT","DRY","DUB","DUN"
    };
    static readonly string[] Words7 = {
        "ABANDON","ABOLISH","ABSENCE","ABSOLVE","ABSTAIN","ACCUSED",
        "ACHIEVE","ACQUIRE","ACTUATE","ADDRESS","AILMENT","AIRLOCK",
        "ALARMED","ALTERED","AMBIENT","AMPLIFY","ANALYZE","ARCHIVE",
        "ARSENAL","ASSAULT","ASSUMED","ATOMIZE","ATTEMPT","AUGMENT",
        "BARRAGE","BARRIER","BATTERY","BAYONET","COMMAND","COMPLEX",
        "COMPUTE","CONCEAL","CONFIRM","CONNECT","CONTROL","CORRUPT"
    };

    const string NOISE = "!@#$%^&*-+=|<>?/\\~`{}[];:',. ";
    const int GRID_LEN = 680;

    float SW => Screen.width;
    float SH => Screen.height;
    float Sx(float v) => v * (SW / 1920f);
    float Sy(float v) => v * (SH / 1080f);
    int Si(float v) => Mathf.Max(1, Mathf.RoundToInt(v * (SH / 1080f)));

    void Start() => NewGame();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ExitTerminal();
    }

    void ExitTerminal()
    {
        if (terminalRoot != null)
            terminalRoot.SetActive(false);
    }

    public void NewGame()
    {
        gameOver = false;
        attemptsLeft = maxAttempts;
        logText = "";
        hoverWord = "";
        removedDuds.Clear();
        BuildGrid();
        Log("ENTER PASSWORD NOW");
        Log("");
        Log(AttemptBar());
        Log("");
    }

    void BuildGrid()
    {
        System.Random rng = new System.Random();
        string[] pool = wordLength == 3 ? Words3 : wordLength == 7 ? Words7 : Words5;

        words = pool.OrderBy(x => rng.Next()).Take(Mathf.Min(wordCount, pool.Length)).ToList();
        password = words[rng.Next(words.Count)];

        grid = new char[GRID_LEN];
        for (int i = 0; i < GRID_LEN; i++) grid[i] = NOISE[rng.Next(NOISE.Length)];

        wordStarts.Clear();
        HashSet<int> used = new HashSet<int>();
        foreach (string w in words)
        {
            for (int t = 0; t < 500; t++)
            {
                int pos = rng.Next(0, GRID_LEN - w.Length - 2);
                bool ok = true;
                for (int k = pos - 1; k < pos + w.Length + 1; k++)
                    if (k >= 0 && k < GRID_LEN && used.Contains(k)) { ok = false; break; }
                if (!ok) continue;
                for (int k = pos; k < pos + w.Length; k++) { grid[k] = w[k - pos]; used.Add(k); }
                wordStarts.Add(pos);
                break;
            }
        }

        bracketOpen.Clear(); bracketClose.Clear();
        char[] openers = new char[] { '(', '[', '{', '<' };
        char[] closers = new char[] { ')', ']', '}', '>' };
        for (int b = 0; b < 5; b++)
        {
            for (int t = 0; t < 200; t++)
            {
                int op = rng.Next(0, GRID_LEN - 8);
                int cl = op + rng.Next(2, 7);
                if (cl >= GRID_LEN || used.Contains(op) || used.Contains(cl)) continue;
                int bt = rng.Next(4);
                grid[op] = openers[bt]; grid[cl] = closers[bt];
                used.Add(op); used.Add(cl);
                bracketOpen.Add(op); bracketClose.Add(cl);
                break;
            }
        }
    }

    void OnGUI()
    {
        if (!stylesReady || Screen.width != lastW || Screen.height != lastH)
        {
            BuildStyles();
            lastW = Screen.width;
            lastH = Screen.height;
        }

        float sw = SW, sh = SH;

        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(0, 0, sw, sh), Texture2D.whiteTexture);
        GUI.color = Color.white;

        float padX = Sx(30f);
        float topH = Sy(110f);
        float gridY = topH + Sy(8f);
        float gridH = sh - gridY - Sy(20f);
        float gridW = sw * 0.62f;
        float divX = gridW;
        float logX = divX + Sx(16f);
        float logW = sw - logX - padX;

        GUI.Label(new Rect(padX, Sy(44f), sw * 0.7f, Sy(28f)), "ENTER PASSWORD NOW", brightStyle);
        GUI.Label(new Rect(padX, Sy(76f), sw * 0.7f, Sy(28f)), AttemptBar(), attemptsLeft <= 1 ? hoverStyle : termStyle);

        // ESC hint (top left)
        GUIStyle escStyle = new GUIStyle(dimStyle);
        escStyle.fontSize = Si(13f);
        GUI.Label(new Rect(padX, Sy(12f), Sx(300f), Sy(24f)), "[ ESC ] EXIT TERMINAL", escStyle);

        // Buttons (top-right)
        float bh = Sy(32f);
        float bby = Sy(12f);
        float bx0 = sw - Sx(460f);
        if (GUI.Button(new Rect(bx0, bby, Sx(90f), bh), "EASY", btnStyle)) { wordLength = 3; NewGame(); }
        if (GUI.Button(new Rect(bx0 + Sx(96f), bby, Sx(105f), bh), "NORMAL", btnStyle)) { wordLength = 5; NewGame(); }
        if (GUI.Button(new Rect(bx0 + Sx(207f), bby, Sx(90f), bh), "HARD", btnStyle)) { wordLength = 7; NewGame(); }
        if (GUI.Button(new Rect(bx0 + Sx(303f), bby, Sx(130f), bh), "NEW GAME", btnStyle)) { NewGame(); }

        // Separator
        GUI.color = new Color(0f, 0.55f, 0.1f, 0.55f);
        GUI.DrawTexture(new Rect(padX, topH, sw - padX * 2f, 1f), Texture2D.whiteTexture);
        GUI.color = Color.white;

        // Game Over
        if (gameOver)
        {
            bool won = logText.Contains("ACCEPTED");
            Color c = won ? new Color(0f, 1f, 0.3f) : new Color(1f, 0.15f, 0.05f);
            string msg = won ? "PASSWORD: giorkos04 (esc to exit)" : "TERMINAL LOCKED";
            GUIStyle big = new GUIStyle(titleStyle);
            big.fontSize = Si(64f);
            big.normal.textColor = c;

            GUIContent content = new GUIContent(msg);
            Vector2 size = big.CalcSize(content);
            float msgX = (sw - size.x) * 0.5f;
            float msgY = (sh * 0.42f) - (size.y * 0.5f);
            GUI.Label(new Rect(msgX, msgY, size.x, size.y), msg, big);

            if (!won)
            {
                float btnW = Sx(300f);
                float btnH = Sy(52f);
                float btnX = (sw - btnW) * 0.5f;
                float btnY = msgY + size.y + Sy(24f);
                if (GUI.Button(new Rect(btnX, btnY, btnW, btnH), "[ TRY AGAIN ]", btnStyle))
                    NewGame();
            }
            return;
        }

        // Grid (left)
        DrawGrid(new Rect(padX, gridY, gridW - padX, gridH));

        // Vertical divider
        GUI.color = new Color(0f, 0.55f, 0.1f, 0.45f);
        GUI.DrawTexture(new Rect(divX, gridY, 1f, gridH), Texture2D.whiteTexture);
        GUI.color = Color.white;

        // Log (right)
        GUI.Label(new Rect(logX, gridY, logW, gridH), logText, logStyle);
    }

    void DrawGrid(Rect area)
    {
        float rowH = Sy(24f);
        float addrW = Sx(80f);
        float charW = Sx(12.2f);

        float halfW = area.width / 2f;
        float charsSpace = halfW - addrW;
        int charsPerRow = Mathf.Max(8, Mathf.FloorToInt(charsSpace / charW));

        int totalRows = Mathf.CeilToInt((float)GRID_LEN / charsPerRow);
        int halfRows = Mathf.CeilToInt(totalRows / 2f);

        Event e = Event.current;
        hoverWord = "";

        for (int row = 0; row < totalRows; row++)
        {
            int col = row < halfRows ? 0 : 1;
            int displayRow = col == 0 ? row : row - halfRows;
            float colX = area.x + col * halfW;
            float ry = area.y + displayRow * rowH;

            if (ry > area.y + area.height) continue;

            int addr = 0xF4A0 + row * 12;
            GUI.Label(new Rect(colX, ry, addrW, rowH), "0x" + addr.ToString("X4"), dimStyle);
            float rx = colX + addrW;

            int startChar = row * charsPerRow;
            int endChar = Mathf.Min(startChar + charsPerRow, GRID_LEN);
            int i = startChar;

            while (i < endChar)
            {
                int wi = GetWordAt(i);
                if (wi >= 0 && !removedDuds.Contains(words[wi]))
                {
                    string w = words[wi];
                    float ww = w.Length * charW;
                    Rect wr = new Rect(rx, ry, ww, rowH);
                    bool hov = wr.Contains(e.mousePosition);
                    if (hov) hoverWord = w;
                    if (hov && e.type == EventType.MouseDown && e.button == 0) SubmitWord(w);
                    GUI.Label(wr, w, hov ? hoverStyle : brightStyle);
                    rx += ww; i += w.Length;
                    continue;
                }

                int bi = bracketOpen.IndexOf(i);
                if (bi >= 0 && bracketOpen[bi] != -1)
                {
                    int cl = bracketClose[bi];
                    int span = cl - i + 1;
                    if (span > 0 && i + span <= endChar)
                    {
                        string bt = new string(grid, i, span);
                        float bw = span * charW;
                        Rect br = new Rect(rx, ry, bw, rowH);
                        bool hov = br.Contains(e.mousePosition);
                        GUIStyle bs = new GUIStyle(dimStyle);
                        bs.normal.textColor = hov ? Color.white : new Color(0.5f, 1f, 0.2f);
                        if (hov && e.type == EventType.MouseDown && e.button == 0) ActivateBracket(bi);
                        GUI.Label(br, bt, bs);
                        rx += bw; i += span;
                        continue;
                    }
                }

                int ri = GetRemovedDudAt(i);
                if (ri >= 0)
                {
                    string dots = new string('.', words[ri].Length);
                    GUI.Label(new Rect(rx, ry, dots.Length * charW, rowH), dots, dimStyle);
                    rx += dots.Length * charW; i += words[ri].Length;
                    continue;
                }

                GUI.Label(new Rect(rx, ry, charW, rowH), grid[i].ToString(), dimStyle);
                rx += charW; i++;
            }
        }
    }

    int GetWordAt(int pos)
    {
        for (int i = 0; i < wordStarts.Count; i++)
            if (wordStarts[i] == pos) return i;
        return -1;
    }

    int GetRemovedDudAt(int pos)
    {
        for (int i = 0; i < wordStarts.Count; i++)
            if (wordStarts[i] == pos && removedDuds.Contains(words[i])) return i;
        return -1;
    }

    void SubmitWord(string word)
    {
        if (gameOver) return;
        Log(">>" + word);

        if (word == password)
        {
            Log("Exact match!"); Log("PASSWORD ACCEPTED."); gameOver = true;
            return;
        }

        int like = Likeness(word, password);
        attemptsLeft--;
        Log("Entry denied.");
        Log("Likeness=" + like + "/" + wordLength);

        string posInfo = BuildPositionInfo(word, password);
        Log("Correct chars: " + posInfo);

        Log(AttemptBar());
        Log("");
        if (attemptsLeft <= 0) { Log("TERMINAL LOCKED."); Log("Password was: " + password); gameOver = true; }
    }

    static string BuildPositionInfo(string guess, string actual)
    {
        int len = Mathf.Min(guess.Length, actual.Length);
        List<string> parts = new List<string>(len);
        for (int i = 0; i < len; i++)
            parts.Add(guess[i] == actual[i] ? guess[i].ToString() : "_");
        return string.Join(" ", parts);
    }

    void ActivateBracket(int bi)
    {
        if (gameOver) return;
        List<string> duds = words.Where(w => w != password && !removedDuds.Contains(w)).ToList();
        if (duds.Count > 0 && UnityEngine.Random.value > 0.5f)
        {
            string dud = duds[UnityEngine.Random.Range(0, duds.Count)];
            removedDuds.Add(dud);
            Log(">>Dud removed.");
        }
        else
        {
            attemptsLeft = maxAttempts;
            Log(">>Tries replenished.");
            Log(AttemptBar());
        }
        bracketOpen[bi] = -1; bracketClose[bi] = -1;
    }

    static int Likeness(string a, string b)
    {
        int n = 0;
        for (int i = 0; i < a.Length && i < b.Length; i++) if (a[i] == b[i]) n++;
        return n;
    }

    string AttemptBar()
    {
        string p = "";
        for (int i = 0; i < attemptsLeft; i++) p += "■ ";
        return "ATTEMPT(S) LEFT: " + p.TrimEnd();
    }

    void Log(string line)
    {
        logText += line + "\n";
        string[] lines = logText.Split('\n');
        if (lines.Length > 50) logText = string.Join("\n", lines.Skip(lines.Length - 50));
    }

    void BuildStyles()
    {
        Color green = new Color(0f, 0.80f, 0.10f);
        Color bright = new Color(0f, 1.00f, 0.30f);
        Color dim = new Color(0f, 0.35f, 0.05f);
        Color hover = new Color(0.8f, 1.00f, 0.10f);

        termStyle = MakeStyle(green, Si(18f), false);
        dimStyle = MakeStyle(dim, Si(18f), false);
        brightStyle = MakeStyle(bright, Si(18f), false);
        hoverStyle = MakeStyle(hover, Si(18f), true);
        logStyle = MakeStyle(green, Si(17f), false);
        titleStyle = MakeStyle(bright, Si(20f), true);

        btnStyle = new GUIStyle(GUI.skin.button);
        btnStyle.fontSize = Si(15f);
        btnStyle.normal.textColor = bright;
        btnStyle.hover.textColor = Color.white;
        btnStyle.normal.background = MakeTex(2, 2, new Color(0f, 0.10f, 0f));
        btnStyle.hover.background = MakeTex(2, 2, new Color(0f, 0.28f, 0f));
        btnStyle.border = new RectOffset(2, 2, 2, 2);
        stylesReady = true;
    }

    static GUIStyle MakeStyle(Color c, int size, bool bold)
    {
        GUIStyle s = new GUIStyle(GUI.skin.label);
        s.fontSize = size;
        s.fontStyle = bold ? FontStyle.Bold : FontStyle.Normal;
        s.wordWrap = false;
        s.normal.textColor = c;
        return s;
    }

    static Texture2D MakeTex(int w, int h, Color col)
    {
        Texture2D t = new Texture2D(w, h);
        Color[] px = new Color[w * h];
        for (int i = 0; i < px.Length; i++) px[i] = col;
        t.SetPixels(px); t.Apply();
        return t;
    }
}