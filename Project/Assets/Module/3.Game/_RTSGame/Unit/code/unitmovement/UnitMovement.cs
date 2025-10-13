using System;
using UnityEngine;

namespace RTSDemo.Unit
{
    public class UnitMovement : MonoBehaviour
    {
        protected float moveSpeed = 1f;
        protected Vector2 targetPoint;
        protected Vector2 velocityVector;
        protected Vector2 externalForce;
        protected Rigidbody2D rigid;
        protected bool isUpdating = true;

        public float maxSpeed => moveSpeed;

        public virtual void Init(Rigidbody2D _rigid)
        {
            rigid = _rigid;
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

        public virtual void SetMoveSpeed(float speed)
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
        public void MoveTowards(Vector2 targetPos)
        {
            targetPoint = targetPos;
            Vector2 diff = targetPoint - (Vector2)transform.position;

            SetVelocityVector(diff);
        }
        public void StopMovement()
        {
            velocityVector = Vector2.zero;
            rigid.linearVelocity = Vector2.zero;
            isUpdating = false;
        }
        public void BeginMovement()
        {
            isUpdating = true;
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
            Vector2 rigidPos = rigid.position;
            rigidPos = CalculateMovement(rigidPos);

            //External Force Handle
            if (externalForce.sqrMagnitude >= 0.01f)
            {
                externalForce *= 0.95f;
                rigidPos += externalForce * Time.fixedDeltaTime;
            }
            else
                externalForce = Vector2.zero;

            rigid.MovePosition(rigidPos);
        }
        protected virtual Vector2 CalculateMovement(Vector2 currentPos)
        {
            if (isUpdating)
                currentPos += velocityVector * moveSpeed * Time.fixedDeltaTime;
            return currentPos;
        }

        public void AddForce(Vector2 force)
        {
            rigid.MovePosition(rigid.position + force * Time.fixedDeltaTime);
        }
        public void AddImpulse(Vector2 impulse)
        {
            externalForce += impulse;
        }
    }
}