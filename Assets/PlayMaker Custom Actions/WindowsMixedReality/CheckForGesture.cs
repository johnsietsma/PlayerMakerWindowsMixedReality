using UnityEngine;
using UnityEngine.VR.WSA.Input;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("HoloLens")]
    [Tooltip("Check for a Windows Gesture")]
    public class CheckForGesture : FsmStateAction
    {
        public FsmEventTarget eventTarget;

        public FsmEvent gestureErrorEvent;
        public FsmEvent holdCanceledEvent;
        public FsmEvent holdCompletedEvent;
        public FsmEvent holdStartedEvent;
        public FsmEvent manipulationCanceledEvent;
        public FsmEvent manipulationCompletedEvent;
        public FsmEvent manipulationStartedEvent;
        public FsmEvent manipulationUpdatedEvent;
        public FsmEvent navigationCanceledEvent;
        public FsmEvent navigationCompletedEvent;
        public FsmEvent navigationStartedEvent;
        public FsmEvent navigationUpdatedEvent;
        public FsmEvent recognitionEndedEvent;
        public FsmEvent recognitionStartedEvent;
        public FsmEvent tappedEvent;

        private GestureRecognizer gestureRecognizer;

        public override void Awake()
        {
            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.GestureErrorEvent += GestureErrorHandler;
            gestureRecognizer.HoldCanceledEvent += HoldCanceledEventHandler;
            gestureRecognizer.HoldCompletedEvent += HoldCompletedEventHandler;
            gestureRecognizer.HoldCanceledEvent += HoldCanceledEventHandler;
            gestureRecognizer.ManipulationCanceledEvent += ManipulationCanceledEventHandler;
            gestureRecognizer.ManipulationCompletedEvent += ManipulationCompletedEventHandler;
            gestureRecognizer.ManipulationStartedEvent += ManipulationStartedEventHandler;
            gestureRecognizer.ManipulationUpdatedEvent += ManipulationUpdatedEventHandler;
            gestureRecognizer.NavigationCanceledEvent += NavigationCanceledEventHandler;
            gestureRecognizer.NavigationCompletedEvent += NavigationCompletedEventHandler;
            gestureRecognizer.NavigationStartedEvent += NavigationStartedEventHandler;
            gestureRecognizer.NavigationUpdatedEvent += NavigationUpdatedEventHandler;
            gestureRecognizer.RecognitionStartedEvent += RecognitionStartedEventHandler;
            gestureRecognizer.RecognitionEndedEvent += RecognitionEndedEventHandler;
            gestureRecognizer.TappedEvent += TappedEventHandler;
        }

        public override void OnEnter()
        {
            gestureRecognizer.StartCapturingGestures();
        }

        public override void OnExit()
        {
            gestureRecognizer.StopCapturingGestures();
        }

        private void GestureErrorHandler(string error, int hresult)
        {
            SendFsmEvent(gestureErrorEvent);
        }

        private void HoldCanceledEventHandler(InteractionSourceKind source, Ray headRay)
        {
            SendFsmEvent(holdCanceledEvent);
        }
        
        private void HoldCompletedEventHandler(InteractionSourceKind source, Ray headRay)
        {
            SendFsmEvent(holdCompletedEvent);
        }

        private void HoldStartedEventHandler(InteractionSourceKind source, Ray headRay)
        {
            SendFsmEvent(holdStartedEvent);
        }

        private void ManipulationCanceledEventHandler(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            SendFsmEvent(manipulationCanceledEvent);
        }

        private void ManipulationCompletedEventHandler(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            SendFsmEvent(manipulationCompletedEvent);
        }

        private void ManipulationStartedEventHandler(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            SendFsmEvent(manipulationStartedEvent);
        }

        private void ManipulationUpdatedEventHandler(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
        {
            SendFsmEvent(manipulationUpdatedEvent);
        }

        private void NavigationCanceledEventHandler(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            SendFsmEvent(navigationCompletedEvent);
        }

        private void NavigationCompletedEventHandler(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            SendFsmEvent(navigationCompletedEvent);
        }

        private void NavigationStartedEventHandler(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            SendFsmEvent(navigationStartedEvent);
        }

        private void NavigationUpdatedEventHandler(InteractionSourceKind source, Vector3 normalizedOffset, Ray headRay)
        {
            SendFsmEvent(navigationUpdatedEvent);
        }
        
        private void RecognitionEndedEventHandler(InteractionSourceKind source, Ray headRay)
        {
            SendFsmEvent(recognitionEndedEvent);
        }

        private void RecognitionStartedEventHandler(InteractionSourceKind source, Ray headRay)
        {
            SendFsmEvent(recognitionStartedEvent);
        }

        private void TappedEventHandler(InteractionSourceKind source, int tapCount, Ray headRay)
        {
            SendFsmEvent(tappedEvent);
        }

        private void SendFsmEvent( FsmEvent fsmEvent )
        {
            if (fsmEvent != null)
            {
                Debug.LogFormat("Sending gesture event: {0}", fsmEvent.Name);
                Fsm.Event(eventTarget, fsmEvent);
            }
        }
    }
}

