using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand.Demo;
using UnityEngine.InputSystem;

namespace Autohand.Demo{
    public class OpenXRHandPlayerControllerLink : MonoBehaviour{
        public AutoHandPlayer player;

        [Header("Input")]
        public InputActionProperty moveAxis;
        public InputActionProperty turnAxis;

        Vector3 axis;
        
        private void OnEnable() {
            if(moveAxis.action != null) moveAxis.action.Enable();
            if (moveAxis.action != null) moveAxis.action.performed += MoveAction;
            if (turnAxis.action != null) turnAxis.action.Enable();
            if (turnAxis.action != null) turnAxis.action.performed += TurnAction;
        }
        private void OnDisable() {
            if (moveAxis.action != null) moveAxis.action.performed -= MoveAction;
            if (turnAxis.action != null) turnAxis.action.performed -= TurnAction;
        }

        private void FixedUpdate()
        {
            var axis = moveAxis.action.ReadValue<Vector2>();
            player.Move(axis);
        }

        private void Update()
        {
            var axis = moveAxis.action.ReadValue<Vector2>();
            player.Move(axis);
        }

        void MoveAction(InputAction.CallbackContext a) {
            var axis = a.ReadValue<Vector2>();
            player.Move(axis);
        }

        void TurnAction(InputAction.CallbackContext a) {
            var axis = a.ReadValue<Vector2>();
            player.Turn(axis.x);
        }

        private void OnDestroy()
        {
            if (moveAxis.action != null) moveAxis.action.performed -= MoveAction;
            if (turnAxis.action != null) turnAxis.action.performed -= TurnAction;
        }


    }
}
