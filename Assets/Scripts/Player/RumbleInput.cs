using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleInput : MonoBehaviour {
    [SerializeField] float rumbleTime;
    
    private PlayerInput _playerInput;
    private Gamepad _gamepad;

    private void Awake() {
        _playerInput = GetComponent<PlayerInput>();
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDestroy() {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change) {
        if (_playerInput.devices.Count > 0)
        {
            _gamepad = _playerInput.devices[0] as Gamepad;
        }
    }
    
    public void Rumble()
    {
        if (_gamepad == null)
            return;

        StartCoroutine(RumbleCoroutine(2));
    }
    
    private IEnumerator RumbleCoroutine(float intensity) {
        _gamepad.SetMotorSpeeds(intensity, intensity);
        yield return new WaitForSeconds(rumbleTime);
        _gamepad.SetMotorSpeeds(0, 0);
    }
}