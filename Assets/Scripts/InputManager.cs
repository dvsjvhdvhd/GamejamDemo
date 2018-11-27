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

    public enum InputButton
    {
        A,
        B,
        S,
    }

    public class InputManager : MonoBehaviour
    {
        public PlayerAnimState playerAnimState;

        public Animator playerAnimator;

        public List<InputButton> userInputs;

        private void Start()
        {
            Koreographer.Instance.RegisterForEvents("Bump", UserInput);
        }

        private void UserInput(KoreographyEvent koreoEvent)
        {
            if (Input.GetKey(KeyCode.A))
            {
                userInputs.Add(InputButton.A);
                Debug.LogError("A");
            }
            else if (Input.GetKey(KeyCode.B))
            {
                userInputs.Add(InputButton.B);
                Debug.LogError("B");
            }
            else
            {
                userInputs.Add(InputButton.S);
                Debug.LogError("S");
            }

            if (userInputs.Count == 8)
            {
                userInputs.Clear();
            }
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
