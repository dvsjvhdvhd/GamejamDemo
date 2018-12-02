using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.UI;

namespace GameLogic
{
    public enum PlayerAnimState
    {
        idle,
        Attack01,
        Attack02,
        Attack03,
    }

    [System.Serializable]
    public struct Combo
    {
        public string value;
        public PlayerAnimState state;
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
        public AudioSource audioBeat;
        public List<Combo> skillCombo;
        public float pitchControl;

        [HideInInspector]
        public List<string> userInputs;
        [Range(50f, 500f)]
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
            audioCom.pitch = pitchControl;
            Koreographer.Instance.RegisterForEvents(eventID, MusicEvent);
            eventIndex = 0;
            controller = Koreographer.Instance.GetKoreographyAtIndex(0);
            rhythmTrack = controller.GetTrackByID(eventID);
            rawEvents = rhythmTrack.GetAllEvents();
        }

        private void Update()
        {
            UpdateInternalValues();

            if (userInputs.Count == 7)
            {
                var inputCombo = string.Join("", userInputs.ToArray());
                Debug.LogError(inputCombo);
                foreach (var combo in skillCombo)
                {
                    if (inputCombo == combo.value)
                    {
                        ChangeState(combo.state);
                        break;
                    }
                }
                userInputs.Clear();
            }

            if (isHit)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    userInputs.Add("A");
                    audioBeat.Play();
                    eventIndex++;
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    userInputs.Add("B");
                    audioBeat.Play();
                    eventIndex++;
                }
            }

            if (isMissed)
            {
                userInputs.Add("=");
                eventIndex++;
            }
        }

        private void MusicEvent(KoreographyEvent koreoEvent)
        {
            Debug.Log("event");
        }

        public void ChangeState(PlayerAnimState state)
        {
            switch (state)
            {
                case PlayerAnimState.Attack01:
                    playerAnimator.SetBool("Attack01", true);
                    break;
                case PlayerAnimState.Attack02:
                    playerAnimator.SetBool("Attack02", true);
                    break;
                case PlayerAnimState.Attack03:
                    playerAnimator.SetBool("Attack03", true);
                    break;
            }
        }
    }
}
