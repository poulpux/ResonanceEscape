using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public partial class InputSystem_ 
{
    #region Mother
    public abstract class InputMother
    {
        public string _inputName;
        public virtual void F_Act(InputAction.CallbackContext value) {}

        public virtual void F_Sleep(InputAction.CallbackContext value) {}
    }
    #endregion
    #region Childs
    public class ButtonInputSystem : InputMother
    {
        public UnityEvent _event = new UnityEvent();
        public bool _button, _pressed;


        public ButtonInputSystem(string inputName) =>
            _inputName = inputName;

        public override void F_Act(InputAction.CallbackContext value)
        {
            if (value.ReadValue<float>() > 0 && !_button)
                _event.Invoke();

            _pressed = true;
            _button = true;
        }

        public override void F_Sleep(InputAction.CallbackContext value) 
        {     
            _pressed = false;
            _button = false;
        }
    }

    public class JoystickInput : InputMother
    {
        public Vector2 _value;

        public JoystickInput(string inputName)=>
            _inputName = inputName;
        public override void F_Act(InputAction.CallbackContext value)=>
            _value = value.ReadValue<Vector2>();

        public override void F_Sleep(InputAction.CallbackContext value)=>
            _value = Vector2.zero;
    }
    #endregion
}
