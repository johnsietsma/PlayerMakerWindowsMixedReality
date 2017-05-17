using UnityEngine;
using UnityEngine.Windows.Speech;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("HoloLens")]
    [Tooltip("Check for a HoloLens Voice Command")]
    public class CheckForHoloLensVoiceCommand : FsmStateAction
    {
        public FsmEventTarget eventTarget;

        public FsmEvent phraseRecognizedEvent;

        [RequiredField]
        [ArrayEditor(VariableType.String)]
        public FsmArray keywords;

        private KeywordRecognizer keywordRecognizer;

        public override void Awake()
        {
            keywordRecognizer = new KeywordRecognizer(keywords.stringValues);
            keywordRecognizer.OnPhraseRecognized += PhraseRecognizedHandler;
        }

        public override void OnEnter()
        {
            keywordRecognizer.Start();
        }

        public override void OnExit()
        {
            keywordRecognizer.Stop();
        }

        public override void Reset()
        {
            keywords = null;
        }

        private void PhraseRecognizedHandler(PhraseRecognizedEventArgs args)
        {
            Debug.Log("Recognized Phrase: " + args.text);
            Fsm.EventData.StringData = args.text;
            Fsm.Event(eventTarget, phraseRecognizedEvent);
        }
    }
}

