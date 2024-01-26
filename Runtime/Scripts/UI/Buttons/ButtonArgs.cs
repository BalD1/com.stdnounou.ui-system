using UnityEngine;

namespace StdNounou.UI
{
	public abstract class ButtonArgs<T> : MonoBehaviour
	{
		public T args;
		public T GetArgs { get => args; }
	} 
}
