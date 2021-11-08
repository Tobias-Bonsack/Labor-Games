using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonController
{
    public class PlayerMovement : MonoBehaviour, PlayerInteraction.IPlayerInteraction
    {
        #region Parameters
        private CharacterController _controller;
        [SerializeField] Transform _mainCamera;
        [SerializeField] Animator _animator;
        [SerializeField] PlayerEvents _playerEvents;

        [Header("Jump")]
        [SerializeField] float _jumpHeight;
        [SerializeField] float _inMoveResistanceMultiplikator;

        [Header("Move")]
        public bool _isMoving = false;
        public Vector3 _direction = Vector3.zero;
        [SerializeField] float _moveResistance;
        [SerializeField] float _basicSpeedResistance;
        [SerializeField] float _maxSpeed;
        [SerializeField] float _acceleration;
        [SerializeField] float _turnTime;
        private float _turnSmoothVelocity;
        private float _angle = 0f;

        [Header("Gravity")]
        private float _gravity = -9.81f;
        private Vector3 _velocity = Vector3.zero;
        public float _basicDown = -4f;
        #endregion

        #region Animation Parameter
        private Vector3 _oldPosition = Vector3.zero;
        #endregion

        #region Unity Events
        void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            _oldPosition = transform.position;

            CalculateMovement();
            CalculateResistance();
            CalculateGravity();

            //TODO here place for extern forces, maybe as an list of extern calls

            _controller.Move(_velocity * Time.fixedDeltaTime);
            UpdateAnimation();

        }
        #endregion

        #region FixedUpdate Methods
        private void CalculateMovement()
        {
            Vector3 moveDir;
            if (StaticProperties._currentCamera == 0 && _isMoving && _controller.isGrounded)
            { //normal camera
                float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
                _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnTime);
                transform.rotation = Quaternion.Euler(0f, _angle, 0f);

                moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                AddForce(moveDir * _acceleration * Time.fixedDeltaTime, false);

                //TODO could be troublesome wiht extern force maybe, maybe not
                _velocity.x = Mathf.Clamp(_velocity.x, -_maxSpeed, _maxSpeed);
                _velocity.z = Mathf.Clamp(_velocity.z, -_maxSpeed, _maxSpeed);
            }
            else if (StaticProperties._currentCamera == 1 && _isMoving && _controller.isGrounded)
            { // throw camera
                moveDir = transform.TransformDirection(new Vector3(_direction.x, 0f, _direction.z));
                AddForce(moveDir * _acceleration * Time.fixedDeltaTime, false);

                //TODO could be troublesome wiht extern force maybe, maybe not
                _velocity.x = Mathf.Clamp(_velocity.x, -_maxSpeed, _maxSpeed);
                _velocity.z = Mathf.Clamp(_velocity.z, -_maxSpeed, _maxSpeed);
            }
        }
        private void CalculateResistance()
        {
            float absVelocityX = Mathf.Abs(_velocity.x);
            float absVelocityZ = Mathf.Abs(_velocity.z);

            float brakeXPercent = absVelocityX / _maxSpeed;
            float brakeZPercent = absVelocityZ / _maxSpeed;

            Vector3 brakeForce = Vector3.zero;

            // resistance * overall percent * opposite direction
            brakeForce.x = _moveResistance * brakeXPercent * OppositeSign(_velocity.x) * (_isMoving ? _inMoveResistanceMultiplikator : 1f);
            brakeForce.z = _moveResistance * brakeZPercent * OppositeSign(_velocity.z) * (_isMoving ? _inMoveResistanceMultiplikator : 1f);

            if (!_isMoving)
            {
                if (absVelocityX < _basicSpeedResistance && absVelocityX != 0f)
                {
                    brakeForce.x = -_velocity.x;
                }
                if (absVelocityZ < _basicSpeedResistance && absVelocityZ != 0f)
                {
                    brakeForce.z = -_velocity.z;
                }
            }

            AddForce(brakeForce, false);
        }
        private void CalculateGravity()
        {
            if (!_controller.isGrounded) { _velocity.y += (_gravity * Time.fixedDeltaTime); }
            else if (_velocity.y < _basicDown) { _velocity.y = _basicDown; }
        }

        private void UpdateAnimation()
        {
            if (_controller.isGrounded)
            {
                Vector3 difPosition = transform.position - _oldPosition;

                float difRotation = Vector3.Angle(Vector3.forward, transform.forward);
                Quaternion rotation = Quaternion.Euler(0f, transform.forward.x > 0f ? -difRotation : difRotation, 0f);
                difPosition = rotation * difPosition;

                _animator.SetFloat("MoveXAxis", difPosition.x / Time.deltaTime);
                _animator.SetFloat("MoveYAxis", difPosition.z / Time.deltaTime);
            }
            _animator.SetBool("isGrounded", _controller.isGrounded);
        }
        #endregion

        internal void Rotate(Vector2 vector2)
        {
            gameObject.transform.Rotate(new Vector3(0f, vector2.x, 0f));
        }

        public void Jump(bool isContextStarted)
        {
            if (isContextStarted && _controller.isGrounded)
            {
                float forceUp = Mathf.Sqrt(_jumpHeight * -2f * _gravity) - _velocity.y;
                Debug.Log(forceUp + _basicDown);
                AddForce(new Vector3(0f, forceUp, 0f), false);
            }
        }

        //TODO check if public is needed at any time
        public void AddForce(Vector3 force, bool reset) => _velocity = reset ? force : force + _velocity;

        private int OppositeSign(float number) => number > 0f ? -1 : 1;

        #region IPlayerInteraction
        public void MudChanges(bool isEnterTrigger, float[] changeArray)
        { // 0 = jumbHeight, 1 = maxSpeed, 2 = acceleration
            _jumpHeight = isEnterTrigger ? _jumpHeight * changeArray[0] : _jumpHeight / changeArray[0];
            _maxSpeed = isEnterTrigger ? _maxSpeed * changeArray[1] : _maxSpeed / changeArray[1];
            _acceleration = isEnterTrigger ? _acceleration * changeArray[2] : _acceleration / changeArray[2];
        }

        public void GravityChange(bool isEnterTrigger, float gravityChange)
        {
            _velocity.y = gravityChange;
        }
        #endregion
    }
}