using UnityEngine;

namespace StdNounou.UI
{
	[CreateAssetMenu(fileName = "New SO_TooltipData", menuName = "StdNounou/Scriptables/UI/TooltipData", order = 110)]
	public class SO_TooltipData : ScriptableObject
	{
		[field: SerializeField] public string Header { get; private set; } = "";
		[field: SerializeField] public string Content { get; private set; } = "";
        [field: SerializeField] public float DelayBeforeShow { get; private set; } = .5f;
    }
}