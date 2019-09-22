using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System;
using System.Linq;

public class ultimateCipher : MonoBehaviour {
    
    public TextMesh[] screenTexts;
    public string[] wordList;
    public KMBombInfo Bomb;
    public KMBombModule module;
    public AudioClip[] sounds;
    public KMAudio Audio;
    public TextMesh submitText;
   
    
    private string[] matrixWordList =
      {
                "ACID",
                "BUST",
                "CODE",
                "DAZE",
                "ECHO",
                "FILM",
                "GOLF",
                "HUNT",
                "ITCH",
                "JURY",
                "KING",
                "LIME",
                "MONK",
                "NUMB",
                "ONLY",
                "PREY",
                "QUIT",
                "RAVE",
                "SIZE",
                "TOWN",
                "URGE",
                "VERY",
                "WAXY",
                "XYLO",
                "YARD",
                "ZERO",
                "ABORT",
                "BLEND",
                "CRYPT",
                "DWARF",
                "EQUIP",
                "FANCY",
                "GIZMO",
                "HELIX",
                "IMPLY",
                "JOWLS",
                "KNIFE",
                "LEMON",
                "MAJOR",
                "NIGHT",
                "OVERT",
                "POWER",
                "QUILT",
                "RUSTY",
                "STOMP",
                "TRASH",
                "UNTIL",
                "VIRUS",
                "WHISK",
                "XERIC",
                "YACHT",
                "ZEBRA",
                "ADVICE",
                "BUTLER",
                "CAVITY",
                "DIGEST",
                "ELBOWS",
                "FIXURE",
                "GOBLET",
                "HANDLE",
                "INDUCT",
                "JOKING",
                "KNEADS",
                "LENGTH",
                "MOVIES",
                "NIMBLE",
                "OBTAIN",
                "PERSON",
                "QUIVER",
                "RACHET",
                "SAILOR",
                "TRANCE",
                "UPHELD",
                "VANISH",
                "WALNUT",
                "XYLOSE",
                "YANKED",
                "ZODIAC",
                "ALREADY",
                "BROWSED",
                "CAPITOL",
                "DESTROY",
                "ERASING",
                "FLASHED",
                "GRIMACE",
                "HIDEOUT",
                "INFUSED",
                "JOYRIDE",
                "KETCHUP",
                "LOCKING",
                "MAILBOX",
                "NUMBERS",
                "OBSCURE",
                "PHANTOM",
                "QUIETLY",
                "REFUSAL",
                "SUBJECT",
                "TRAGEDY",
                "UNKEMPT",
                "VENISON",
                "WARSHIP",
                "XANTHIC",
                "YOUNGER",
                "ZEPHYRS",
                "ADVOCATE",
                "BACKFLIP",
                "CHIMNEYS",
                "DISTANCE",
                "EXPLOITS",
                "FOCALIZE",
                "GIFTWRAP",
                "HOVERING",
                "INVENTOR",
                "JEALOUSY",
                "KINSFOLK",
                "LOCKABLE",
                "MERCIFUL",
                "NOTECARD",
                "OVERCAST",
                "PERILOUS",
                "QUESTION",
                "RAINCOAT",
                "STEALING",
                "TREASURY",
                "UPDATING",
                "VERTICAL",
                "WISHBONE",
                "XENOLITH",
                "YEARLONG",
                "ZEALOTRY"
        };

    private string[][] pages;
    private string answer;
    private int page;
    private bool submitScreen;
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    public KMSelectable leftArrow;
    public KMSelectable rightArrow;
    public KMSelectable submit;
    public KMSelectable[] keyboard;
    void Awake()
    {
        moduleId = moduleIdCounter++;
        leftArrow.OnInteract += delegate () { left(leftArrow); return false; };
        rightArrow.OnInteract += delegate () { right(rightArrow); return false; };
        submit.OnInteract += delegate () { submitWord(submit); return false; };
        foreach(KMSelectable keybutton in keyboard)
        {
            KMSelectable pressedButton = keybutton;
            pressedButton.OnInteract += delegate () { letterPress(pressedButton); return false; };
        }
    }
        // Use this for initialization
        void Start ()
    
    {
        submitText.text = "1";
        //Generating random word
        answer = wordList[UnityEngine.Random.Range(0, wordList.Length)].ToUpper();
        Debug.LogFormat("[Yellow Cipher #{0}] Generated Word: {1}", moduleId, answer);
       
        pages = new string[2][];
        pages[0] = new string[3];
        pages[1] = new string[3];
        pages[0][0] = "";
        pages[0][1] = "";
        pages[0][2] = "";
        string encrypt = yellowcipher(answer);
        pages[0][0] = encrypt.ToUpper();
        page = 0;
        getScreens();
    }
    string yellowcipher(string word)
    {
        Debug.LogFormat("[Yellow Cipher #{0}] Begin Hill Encryption", moduleId);
        string encrypt = HillEnc(word);
        Debug.LogFormat("[Yellow Cipher #{0}] Begin Trifid Encryption", moduleId);
        encrypt = TrifidEnc(encrypt);
        string[] split = encrypt.Split(' ');
        encrypt = split[0].ToUpper();
        string kw1 = MorbitEnc(split[1].ToUpper());
        int num = kw1.Length / 3;
        pages[0][1] = kw1.Substring(0, num);
        pages[0][2] = kw1.Substring(num, num);
        pages[1][0] = kw1.Substring(num + num);
        return encrypt;
    }
    string MorbitEnc(string word)
    {
        
        string[] morse =
        {
             ".-",
             "-...",
             "-.-.",
             "-..",
             ".",
             "..-.",
             "--.",
             "....",
             "..",
             ".---",
             "-.-",
             ".-..",
             "--",
             "-.",
             "---",
             ".--.",
             "--.-",
             ".-.",
             "...",
             "-",
             "..-",
             "...-",
             ".--",
             "-..-",
             "-.--",
             "--.."
        };
        
        
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string encrypt = "";

        for (int aa = 0; aa < word.Length; aa++)
            encrypt = encrypt + "" + morse[alpha.IndexOf(word[aa])] + "X";
        if (encrypt.Length % 2 == 1)
            encrypt = encrypt.Substring(0, encrypt.Length - 1);

        char[] nums = new char[8];
        int items = 0;
        string key = "12345678";
        string kw = matrixWordList[UnityEngine.Random.Range(0, 26) + 104];
        for(int cc = 0; cc < alpha.Length; cc++)
        {
            for(int dd = 0; dd < kw.Length; dd++)
            {
                if (kw[dd].ToString().IndexOf(alpha[cc]) >= 0)
                {
                    nums[dd] = key[items];
                    items++;
                }
            }
        }
        key = nums[0] + "" + nums[1] + "" + nums[2] + "" + nums[3] + "" + nums[4] + "" + nums[5] + "" + nums[6] + "" + nums[7];
        string encrypt2 = "";
        for (int ee = 0; ee < encrypt.Length; ee++)
        {
            string morsepair = encrypt[ee] + "" + encrypt[ee + 1];
            ee++;
            switch(morsepair)
            {
                case "..":
                    encrypt2 = encrypt2 + "" + key[0];
                    break;
                case ".-":
                    encrypt2 = encrypt2 + "" + key[1];
                    break;
                case ".X":
                    encrypt2 = encrypt2 + "" + key[2];
                    break;
                case "-.":
                    encrypt2 = encrypt2 + "" + key[3];
                    break;
                case "--":
                    encrypt2 = encrypt2 + "" + key[4];
                    break;
                case "-X":
                    encrypt2 = encrypt2 + "" + key[5];
                    break;
                case "X.":
                    encrypt2 = encrypt2 + "" + key[6];
                    break;
                case "X-":
                    encrypt2 = encrypt2 + "" + key[7];
                    break;
            }
        }
        Debug.LogFormat("[Yellow Cipher #{0}] Morbit Key: {1}", moduleId, key);
        Debug.LogFormat("[Yellow Cipher #{0}] Morbit Encrypted Word: {1}", moduleId, encrypt2);
        pages[1][1] = kw.ToUpper();

        return encrypt2;
    }
    string TrifidEnc(string word)
    {
        string kw = matrixWordList[UnityEngine.Random.Range(0, matrixWordList.Length)];
        int snnum = Bomb.GetSerialNumberNumbers().First();
        string key = getKey(kw, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", Bomb.GetBatteryCount() % 2 == 1);
        string encrypt = "";
        string[] grid =
        {
            "11111111122222222233333333",
            "11122233311122233311122233",
            "12312312312312312312312312"
        };
        string[] numbers =
            {
                "", "", ""
            };
        for(int aa = 0; aa < 6; aa++)
        {
            int cursor = key.IndexOf(word[aa]);
            numbers[0] = numbers[0] + "" + grid[0][cursor];
            numbers[1] = numbers[1] + "" + grid[1][cursor];
            numbers[2] = numbers[2] + "" + grid[2][cursor];
        }
        bool flag = true;
        for(int bb = 0; bb < 3; bb++)
        {
            string n1 = numbers[bb][0] + "" + numbers[bb][1] + "" + numbers[bb][2];
            string n2 = numbers[bb][3] + "" + numbers[bb][4] + "" + numbers[bb][5];
            if(n1.Equals("333") || n2.Equals("333"))
            {
                flag = false;
                break;
            } 
        }
        if(flag)
        {
            Debug.LogFormat("[Yellow Cipher #{0}] Trifid Key: {1}", moduleId, key);
            Debug.LogFormat("[Yellow Cipher #{0}] Trifid Numbers:\n{1}\n{2}\n{3}", moduleId, numbers[0], numbers[1], numbers[2]);
            for (int bb = 0; bb < 3; bb++)
            {
                string[] nums = new string[2];
                nums[0] = numbers[bb][0] + "" + numbers[bb][1] + "" + numbers[bb][2];
                nums[1] = numbers[bb][3] + "" + numbers[bb][4] + "" + numbers[bb][5];
                string pos = "931";
                for(int cc = 0; cc < 2; cc++)
                {
                    int num = 0;
                    for (int dd = 0; dd < 3; dd++)
                    {
                        num += ((pos[dd] - '0') * (nums[cc][dd] - '0' - 1));
                    }
                    encrypt = encrypt + "" + key[num];
                }
            }
            Debug.LogFormat("[Yellow Cipher #{0}] Trifid Encrypted Word: {1}", moduleId, encrypt);
            return encrypt + " " + kw;
        }
        else
        {
            return TrifidEnc(word);
        }
        
    }
    string HillEnc(string word)
    {
        string encrypt = "";
        int[] numbers = new int[4];
        numbers[1] = UnityEngine.Random.Range(0, 26);
        numbers[2] = UnityEngine.Random.Range(0, 26);
        if((numbers[1] * numbers[2]) % 2 == 1)
            numbers[0] = UnityEngine.Random.Range(1, 13) * 2;
        else
            numbers[0] = (UnityEngine.Random.Range(0, 13) * 2) + 1;
        
        int[] nums = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25};
        for (int aa = 0; aa < 26; aa++)
        {
            int num = (aa * numbers[0]) - (numbers[1] * numbers[2]);
            if ((num % 2 == 0) || (num % 13 == 0))
                nums = nums.Where(val => val != aa).ToArray();
        }
    
        numbers[3] = nums[UnityEngine.Random.Range(0, nums.Length)];
        string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        for (int bb = 0; bb < 6; bb++)
        {
            String logoutput = "";
            int n1 = (alpha.IndexOf(word[bb]) + 1) % 26;
            bb++;
            int n2 = (alpha.IndexOf(word[bb]) + 1) % 26;
            logoutput = logoutput + "" + n1 + " " + n2;
            int l1 = ((numbers[0] * n1) + (numbers[1] * n2)) % 26;
            int l2 = ((numbers[2] * n1) + (numbers[3] * n2)) % 26;
            logoutput = logoutput + " -> " + l1 + " " + l2;
            if (l1 == 0)
                l1 = 26;
            if (l2 == 0)
                l2 = 26;
            encrypt = encrypt + "" + alpha[l1 - 1] + "" + alpha[l2 - 1];
            Debug.LogFormat("[Yellow Cipher #{0}] {1}", moduleId, logoutput);
        }
        Debug.LogFormat("[Yellow Cipher #{0}] Hill Encrypted Word: {1}", moduleId, encrypt);
        pages[1][2] = numbers[0] + "-" + numbers[1] + "-" + numbers[2] + "-" + numbers[3];
        
        return encrypt;
    }
   
    string getKey(String k, String alpha, bool start)
    {
        for (int aa = 0; aa < k.Length; aa++)
        {
            for (int bb = aa + 1; bb < k.Length; bb++)
            {
                if (k[aa] == k[bb])
                {
                    k = k.Substring(0, bb) + "" + k.Substring(bb + 1);
                    bb--;
                }
            }
            alpha = alpha.Replace(k[aa].ToString(), "");
        }
        if (start)
            return (k + "" + alpha);
        else
            return (alpha + "" + k);
    }
	int correction(int p, int max)
    {
        while (p < 0)
            p += max;
        while (p >= max)
            p -= max;
        return p;
    }
    void left(KMSelectable arrow)
    {
        if(!moduleSolved)
        {
            Audio.PlaySoundAtTransform(sounds[0].name, transform);
            submitScreen = false;
            arrow.AddInteractionPunch();
            page--;
            page = correction(page, pages.Length);
            getScreens();
        }
    }
    void right(KMSelectable arrow)
    {
        if(!moduleSolved)
        {
            Audio.PlaySoundAtTransform(sounds[0].name, transform);
            submitScreen = false;
            arrow.AddInteractionPunch();
            page++;
            page = correction(page, pages.Length);
            getScreens();
        }
    }
    private void getScreens()
    {
        submitText.text = (page + 1) + "";
        screenTexts[0].text = pages[page][0];
        screenTexts[1].text = pages[page][1];
        screenTexts[2].text = pages[page][2];
        if(page == 1)
            screenTexts[2].fontSize = 35;
        else
            screenTexts[2].fontSize = 40;
        screenTexts[0].fontSize = 40;
        screenTexts[1].fontSize = 40;
        
        
    }
    void submitWord(KMSelectable submitButton)
    {
        if(!moduleSolved)
        {
            submitButton.AddInteractionPunch();
            if(screenTexts[2].text.Equals(answer))
            {
                Audio.PlaySoundAtTransform(sounds[2].name, transform);
                module.HandlePass();
                moduleSolved = true;
                screenTexts[2].text = "";
            }
            else
            {
                Audio.PlaySoundAtTransform(sounds[3].name, transform);
                module.HandleStrike();
                page = 0;
                getScreens();
                submitScreen = false;
            }
        }
    }
    void letterPress(KMSelectable pressed)
    {
        if(!moduleSolved)
        {
            pressed.AddInteractionPunch();
            Audio.PlaySoundAtTransform(sounds[1].name, transform);
            if (submitScreen)
            {
                if(screenTexts[2].text.Length < 6)
                {
                    screenTexts[2].text = screenTexts[2].text + "" + pressed.GetComponentInChildren<TextMesh>().text;
                }
            }
            else
            {
                submitText.text = "SUB";
                screenTexts[0].text = "";
                screenTexts[1].text = "";
                screenTexts[2].text = pressed.GetComponentInChildren<TextMesh>().text;
                submitScreen = true;
            }
        }
    }
#pragma warning disable 414
    private string TwitchHelpMessage = "Move to other screens using !{0} right|left|r|l|. Submit the decrypted word with !{0} submit qwertyuiopasdfghjklzxcvbnm";
#pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {

        if (command.EqualsIgnoreCase("right") || command.EqualsIgnoreCase("r"))
        {
            yield return null;
            rightArrow.OnInteract();
            yield return new WaitForSeconds(0.1f);

        }
        if (command.EqualsIgnoreCase("left") || command.EqualsIgnoreCase("l"))
        {
            yield return null;
            leftArrow.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        string[] split = command.ToUpperInvariant().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        if (split.Length != 2 || !split[0].Equals("SUBMIT") || split[1].Length != 6) yield break;
        int[] buttons = split[1].Select(getPositionFromChar).ToArray();
        if (buttons.Any(x => x < 0)) yield break;

        yield return null;

        yield return new WaitForSeconds(0.1f);
        foreach (char let in split[1])
        {
            keyboard[getPositionFromChar(let)].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.1f);
        submit.OnInteract();
        yield return new WaitForSeconds(0.1f);
    }

    private int getPositionFromChar(char c)
    {
        return "QWERTYUIOPASDFGHJKLZXCVBNM".IndexOf(c);
    }
}
