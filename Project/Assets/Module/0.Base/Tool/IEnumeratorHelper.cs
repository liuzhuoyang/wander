using System;
using System.Collections;
using UnityEngine;

//封装处理coroutine，一次只执行一个coroutine，当有新的任务时，会自动结束上一次未完成的coroutine
public class CoroutineExcuter
{
    MonoBehaviour initiator;
    IEnumerator coroutine;
    public CoroutineExcuter(MonoBehaviour _context)=>initiator = _context;
    public void Excute(IEnumerator go){
        if(coroutine!=null) initiator.StopCoroutine(coroutine);
        coroutine = go;
        initiator.StartCoroutine(go);
    }
    public void Abort(){
        if(coroutine!=null) initiator.StopCoroutine(coroutine);
    }
}

//封装的迭代器，在coroutine里执行的循环执行某一段总时长为duration的程序
public class WaitForLoop: IEnumerator
{
    private IEnumerator m_coroutine;

    public WaitForLoop(float _duration, Action<float> _go){
        m_coroutine = ForLoopCoroutine(_duration, _go);
    }

    public object Current{get{return m_coroutine.Current;}}
    public bool MoveNext(){return m_coroutine.MoveNext();}
    public void Reset()=>m_coroutine.Reset();
    
    public static IEnumerator ForLoopCoroutine(float duration, Action<float> go){
        float speed = 1f/duration;
        for(float t=0; t<1; t+=Time.deltaTime*speed){
            go(t);
            yield return null;
        }
        go(1);
    }
}