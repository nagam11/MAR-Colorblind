using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechRecognition : MonoBehaviour {

    public string[] commands = new string[] {"deuteranopia", "protanopia", "tritanopia", "next"};
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float speed = 1;
    protected PhraseRecognizer recognizer;
    protected string word;

    void Start () {

        if (commands != null)
        {
            recognizer = new KeywordRecognizer(commands, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        print("You said: " + word);
    }

    void Update () {
		switch(word)
        {
            case "deuteranopia":
                GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness -= Colorblind.Blindness.deuteranopia;
                break;
            case "protanopia":
                GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness -= Colorblind.Blindness.protanopia;
                break;
            case "tritanopia":
                GameObject.Find("ARCamera").GetComponent<Colorblind>().blindness -= Colorblind.Blindness.tritanopia;
                break;
            case "next":
                GameObject.Find("ARCamera").GetComponent<Colorblind>().NextMode();
                break;
        }
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
