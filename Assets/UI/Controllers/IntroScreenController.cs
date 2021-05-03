using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Controllers
{
    public class IntroScreenController : MonoBehaviour
    {
        public Animator uiAnimator;
        
        private static readonly int IntroClose = Animator.StringToHash("intro_close");

        public void FixedUpdate()
        {
            if (AnyKeyboardPressed() || AnyGamepadWasPressed())
            {
                uiAnimator.SetTrigger(IntroClose);
            }
        }

        private bool AnyKeyboardPressed() => Keyboard.current.anyKey.wasPressedThisFrame;

        private bool AnyGamepadWasPressed() => Gamepad.current?.allControls.Any(c => c.IsPressed()) ?? false;
    }
}
