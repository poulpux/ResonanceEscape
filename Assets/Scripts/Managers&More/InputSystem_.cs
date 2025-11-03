using UnityEngine.InputSystem;

public partial class InputSystem_ : MonoSingleton<InputSystem_>
{
    #region Values
    PlayerInput playerInput;
    private InputActionMap DefaultActions;

    public ButtonInputSystem _echap = new ButtonInputSystem("Quit");
    public ButtonInputSystem _space = new ButtonInputSystem("Space");
    public ButtonInputSystem _leftClick = new ButtonInputSystem("LeftClick");
    public ButtonInputSystem _rightClick = new ButtonInputSystem("RightClick");
    
    public JoystickInput _leftJoystick = new JoystickInput("LeftJoystick");
    #endregion

    #region Callback
    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        DefaultActions = playerInput.actions.FindActionMap("Default");
    }


    private void OnEnable()=>
        EnableActionMap(DefaultActions);

    private void OnDisable()=>
        DisableActionMap(DefaultActions);

    #endregion
    #region Functions
    private void EnableActionMap(InputActionMap action)
    {
        EnableMapButton(action, _echap);
        EnableMapButton(action, _space);
        EnableMapButton(action, _leftClick);
        EnableMapButton(action, _rightClick);
        EnableMapButton(action, _leftJoystick);
    }
    private void DisableActionMap(InputActionMap action)
    {
        DisableMapButton(action, _echap);
        DisableMapButton(action, _space);
        DisableMapButton(action, _leftClick);
        DisableMapButton(action, _rightClick);
        DisableMapButton(action, _leftJoystick);
    }

    private void EnableMapButton(InputActionMap action, InputMother input)
    {
        action[input._inputName].performed += input.F_Act;
        action[input._inputName].canceled += input.F_Sleep;
    }
    
    private void DisableMapButton(InputActionMap action, InputMother input)
    {
        action[input._inputName].performed -= input.F_Act;
        action[input._inputName].canceled -= input.F_Sleep;
    }
    #endregion
}
