//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/PlayerInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Horizontal"",
            ""id"": ""691d2ed6-9785-44cc-abe8-74332612e0cb"",
            ""actions"": [
                {
                    ""name"": ""HorizontalMove"",
                    ""type"": ""Value"",
                    ""id"": ""a16e13ce-316e-4172-bde8-6fec8a865179"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ShootMovement"",
                    ""type"": ""Value"",
                    ""id"": ""e1d83ea8-eb6f-435c-9737-f3a217964b2c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""HorizontalMove"",
                    ""id"": ""b4accdeb-5334-44e7-8c57-e0669f2b3a3c"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Negative"",
                    ""id"": ""45c9039a-e013-488b-8eb3-1c681ef1ed06"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Positive"",
                    ""id"": ""26eddadc-94ca-4cd8-9176-3fa56e784072"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""1a3bfd13-ae00-4459-8344-896ab8f415e1"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HorizontalMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5ea5727-783d-4b91-a71f-83c60f45b91e"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ShootMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Horizontal
        m_Horizontal = asset.FindActionMap("Horizontal", throwIfNotFound: true);
        m_Horizontal_HorizontalMove = m_Horizontal.FindAction("HorizontalMove", throwIfNotFound: true);
        m_Horizontal_ShootMovement = m_Horizontal.FindAction("ShootMovement", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Horizontal
    private readonly InputActionMap m_Horizontal;
    private IHorizontalActions m_HorizontalActionsCallbackInterface;
    private readonly InputAction m_Horizontal_HorizontalMove;
    private readonly InputAction m_Horizontal_ShootMovement;
    public struct HorizontalActions
    {
        private @PlayerInputActions m_Wrapper;
        public HorizontalActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @HorizontalMove => m_Wrapper.m_Horizontal_HorizontalMove;
        public InputAction @ShootMovement => m_Wrapper.m_Horizontal_ShootMovement;
        public InputActionMap Get() { return m_Wrapper.m_Horizontal; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(HorizontalActions set) { return set.Get(); }
        public void SetCallbacks(IHorizontalActions instance)
        {
            if (m_Wrapper.m_HorizontalActionsCallbackInterface != null)
            {
                @HorizontalMove.started -= m_Wrapper.m_HorizontalActionsCallbackInterface.OnHorizontalMove;
                @HorizontalMove.performed -= m_Wrapper.m_HorizontalActionsCallbackInterface.OnHorizontalMove;
                @HorizontalMove.canceled -= m_Wrapper.m_HorizontalActionsCallbackInterface.OnHorizontalMove;
                @ShootMovement.started -= m_Wrapper.m_HorizontalActionsCallbackInterface.OnShootMovement;
                @ShootMovement.performed -= m_Wrapper.m_HorizontalActionsCallbackInterface.OnShootMovement;
                @ShootMovement.canceled -= m_Wrapper.m_HorizontalActionsCallbackInterface.OnShootMovement;
            }
            m_Wrapper.m_HorizontalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @HorizontalMove.started += instance.OnHorizontalMove;
                @HorizontalMove.performed += instance.OnHorizontalMove;
                @HorizontalMove.canceled += instance.OnHorizontalMove;
                @ShootMovement.started += instance.OnShootMovement;
                @ShootMovement.performed += instance.OnShootMovement;
                @ShootMovement.canceled += instance.OnShootMovement;
            }
        }
    }
    public HorizontalActions @Horizontal => new HorizontalActions(this);
    public interface IHorizontalActions
    {
        void OnHorizontalMove(InputAction.CallbackContext context);
        void OnShootMovement(InputAction.CallbackContext context);
    }
}
