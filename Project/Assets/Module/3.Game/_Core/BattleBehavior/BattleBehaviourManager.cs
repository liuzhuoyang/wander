using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleBehaviourManager : Singleton<BattleBehaviourManager>
{
    public bool IsPaused => isPaused;
    private bool isPaused = false;
    List<BattleBehaviour> _behaviours = new List<BattleBehaviour>();
    HashSet<Rigidbody2D> _battleDynamics = new HashSet<Rigidbody2D>();

    public void RegisterBehaviour(BattleBehaviour behaviour)
    {
        Rigidbody2D rigid = behaviour.GetComponent<Rigidbody2D>();
        if (rigid != null && rigid.bodyType == RigidbodyType2D.Dynamic)
        {
            _battleDynamics.Add(rigid);
        }
        if (!_behaviours.Contains(behaviour))
                _behaviours.Add(behaviour);
    }

    public void UnregisterBehaviour(BattleBehaviour behaviour)
    {
        behaviour.MarkCleanUp(true);

        Rigidbody2D rigid = behaviour.GetComponent<Rigidbody2D>();
        if (rigid != null && rigid.bodyType == RigidbodyType2D.Dynamic)
        {
            _battleDynamics.Remove(rigid);
        }
    }

    public void Update()
    {
        if (isPaused)
            return;

        BattleBehaviour front = null;
        for (int i = _behaviours.Count - 1; i >= 0; i--)
        {
            front = _behaviours[i];
            if (front == null)
            {
                _behaviours.RemoveAt(i);
                continue;
            }
            else if (front.NeedCleanUp)
            {
                front.MarkCleanUp(false);
                _behaviours.RemoveAt(i);
                continue;
            }

            front.BattleUpdate();
        }
    }

    void FixedUpdate()
    {
        if (isPaused)
            return;

        for (int i = _behaviours.Count - 1; i >= 0; i--)
        {
            if(_behaviours[i].NeedCleanUp)
                continue;
            _behaviours[i].BattleFixedUpdate();
        }
    }

    public void LateUpdate()
    {
        if (isPaused)
            return;

        for (int i = _behaviours.Count - 1; i >= 0; i--)
        {
            if(_behaviours[i].NeedCleanUp)
                continue;
            _behaviours[i].BattleLateUpdate();
        }
    }

    public void SetPause(bool isPaused)
    {
        this.isPaused = isPaused;
        foreach (var rigid in _battleDynamics)
        {
            if (rigid != null)
            {
                rigid.simulated = !isPaused;
            }
        }
    }
}