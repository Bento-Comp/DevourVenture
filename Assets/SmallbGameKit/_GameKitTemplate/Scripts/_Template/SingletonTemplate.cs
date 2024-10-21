using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Template
{
	[DefaultExecutionOrder(-32000)]
	[AddComponentMenu("TemplateFolder/SingletonTemplate")]
	public class SingletonTemplate : UniSingleton.Singleton<SingletonTemplate>
	{
	}
}