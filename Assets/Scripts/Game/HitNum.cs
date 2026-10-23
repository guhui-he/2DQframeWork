using UnityEngine;
using QFramework;

namespace Game
{
	public partial class HitNum : ViewController
	{
		void Start()
		{
			// Code Here
		}
		void Update()
		{
			transform.Translate(Vector3.up*80*Time.deltaTime);
			Destroy(gameObject, 0.9f);
		}

	}
}
