using System;
using System.Collections.Generic;

/// <summary>
/// 观察者模式 用于UI监听数据的更新，比如战斗中金币变化，血量变化等
/// 
/// </summary>
public class Subject<T>
{
    private T _value;
    private List<Action<T>> _observers = new List<Action<T>>();

    public T Value
    {
        get => _value;
        set
        {
            if (!_value.Equals(value))
            {
                _value = value;
                NotifyObservers();
            }
        }
    }

    public Subject(T initialValue)
    {
        _value = initialValue;
    }

    //隐式转换打开
    public static implicit operator T(Subject<T> subject) => subject.Value;

    public void Subscribe(Action<T> observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Unsubscribe(Action<T> observer)
    {
        if (_observers.Contains(observer))
            _observers.Remove(observer);
    }

    //通知观察者
    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer?.Invoke(_value);
        }
    }
}