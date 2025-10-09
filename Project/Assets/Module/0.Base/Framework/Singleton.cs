﻿using UnityEngine;
using System.Collections;

public abstract class Singleton<T> : MonoBehaviour
	where T:MonoBehaviour
{
	private static T m_instance = null;
	
	public static T Instance 
	{
		get{ return m_instance; }
	}

	protected virtual void Awake()
	{
		m_instance = this as T;
		//if(m_instance== null)
		//      {
		//	m_instance = this as T;
		//      }
		//      else
		//      {
		//	throw new System.Exception("异常");
		//      }
	}
}
