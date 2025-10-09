using System;
using UnityEngine;

namespace BattleActor.Unit
{
    public class UnitMovement : MonoBehaviour
    {
        private float moveSpeed = 1f;
        private Vector2 velocityVector;
        private Vector2 externalForce;
        private Rigidbody2D m_rigid;

        public void Init(Rigidbody2D _rigid)
        {
            m_rigid = _rigid;
        }
        public void SwitchModule(bool isActivated) => this.enabled = isActivated;

        public Vector2 GetVector()
        {
            return velocityVector;
        }

        public Vector2 GetVelocity()
        {
            return velocityVector * moveSpeed;
        }

        public void SetMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        public void SetVelocityVector(Vector3 targetVector, bool normalized = true)
        {
            if (normalized)
                this.velocityVector = targetVector.normalized;
            else
                this.velocityVector = targetVector;
        }
        public void StopMovement()
        {
            velocityVector = Vector2.zero;
            m_rigid.linearVelocity = Vector2.zero;
            this.enabled = false;
        }
        public void BeginMovement()
        {
            this.enabled = true;
        }

        public void SlerpVelocity(Vector3 targetVector, float lerpSpeed = 2, bool normalized = true)
        {
            targetVector.z = 0;
            if (normalized)
                targetVector = targetVector.normalized;

            this.velocityVector = Vector3.Slerp(this.velocityVector, targetVector, Time.deltaTime * lerpSpeed);
        }

        public void UpdateMovement()
        {

            Vector2 rigidPos = m_rigid.position;
            if (this.enabled)
            {
                rigidPos += velocityVector * moveSpeed * Time.fixedDeltaTime;
            }

            if (externalForce.sqrMagnitude >= 0.01f)
            {
                externalForce *= 0.95f;
                rigidPos += externalForce * Time.fixedDeltaTime;
            }
            else
                externalForce = Vector2.zero;

            rigidPos += externalForce * Time.fixedDeltaTime;
            m_rigid.MovePosition(rigidPos);
        }
        public void AddForce(Vector2 force)
        {
            m_rigid.MovePosition(m_rigid.position + force * Time.fixedDeltaTime);
        }
        public void AddImpulse(Vector2 impulse)
        {
            externalForce += impulse;
        }
    }
}