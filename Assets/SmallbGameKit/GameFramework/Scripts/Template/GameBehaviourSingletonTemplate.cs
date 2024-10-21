using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameFramework;

[AddComponentMenu("TemplateFolder/GameBehaviourSingletonTemplate")]
public class GameBehaviourSingletonTemplate : GameBehaviour 
{
	static GameBehaviourSingletonTemplate instance;
		
	static public GameBehaviourSingletonTemplate Instance
	{
		get
		{
			return instance;
		}
	}
	
	protected override void OnAwake()
	{
		if(instance == null)
		{
			instance = this;
		}
		else
		{
			Debug.LogWarning("A singleton can only be instantiated once!");
			Destroy(gameObject);
			return;
		}
	}
	
	protected override void OnAwakeEnd()
	{
		if(instance == this)
		{
			instance = null;
		}
	}
}