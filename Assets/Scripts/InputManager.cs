using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

namespace GameLogic
{
    public enum PlayerAnimState
    {
        idle,
        Attack01,
        Attack02,
        Attack03,
    }

    public class InputManager : MonoBehaviour
    {
        private Koreography controller;
        private KoreographyTrack rhythmTrack;
        private List<KoreographyEvent> rawEvents;

        private int eventIndex;
        private int rangeInSamples;

        private bool isHit
        {
            get
            {
                return (Mathf.Abs(currentEvent.StartSample - DelayedSampleTime) <= rangeInSamples);
            }
        }

        private bool isMissed
        {
            get
            {
                return DelayedSampleTime - currentEvent.StartSample >= rangeInSamples;
            }
        }

        private KoreographyEvent currentEvent
        {
            get
            {
                return rawEvents[eventIndex];
            }
        }

        [EventID]
        public string eventID;
        public PlayerAnimState playerAnimState;
        public Animator playerAnimator;
        public AudioSource audioCom;

        [HideInInspector]
        public List<KeyCode> userInputs;
        [Range(50f, 150f)]
        public int rangeInMS;

        public int SampleRate
        {
            get
            {
                return controller.SampleRate;
            }
        }

        public int DelayedSampleTime
        {
            get
            {
                return controller.GetLatestSampleTime() - (int)(audioCom.pitch * SampleRate);
            }
        }

        private void UpdateInternalValues()
        {
            rangeInSamples = (int)(0.001f * rangeInMS * SampleRate);
        }

        private void Start()
        {
            Koreographer.Instance.RegisterForEvents(eventID, UserInput);
            eventIndex = 0;
            controller = Koreographer.Instance.GetKoreographyAtIndex(0);
            rhythmTrack = controller.GetTrackByID(eventID);
            rawEvents = rhythmTrack.GetAllEvents();
            Debug.LogError(rawEvents.Count);
        }

        private void Update()
        {
            UpdateInternalValues();

            if (userInputs.Count == 8)
            {
                foreach (var key in userInputs)
                {
                    Debug.Log(key);
                }
                Debug.Log("================================");
                userInputs.Clear();
            }

            if (isHit)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    userInputs.Add(KeyCode.A);
                    Debug.LogError("A");
                    eventIndex++;
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    userInputs.Add(KeyCode.B);
                    Debug.LogError("B");
                    eventIndex++;
                }          
            }

            if (isMissed)
            {
                userInputs.Add(KeyCode.Space);
                Debug.LogError("=");
                eventIndex++;
            }
        }

        private void UserInput(KoreographyEvent koreoEvent)
        {

        }

        public void ChangeState(PlayerAnimState state)
        {
            switch (state)
            {
                case PlayerAnimState.idle:
                    playerAnimator.SetBool("IsPlay", true);
                    playerAnimator.SetBool("IsIdle", false);
                    break;
                default:
                    break;
            }
        }
    }
}
