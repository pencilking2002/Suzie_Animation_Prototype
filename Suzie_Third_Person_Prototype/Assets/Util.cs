using UnityEngine;
using System.Collections;

public class Util : MonoBehaviour {
	
	public static Util Instance;
	
	private void Awake ()
	{
		if (Instance == null)
			Instance = this;
	}
	
	/// <summary>
	/// Determines whether this instance is ground the specified obj.
	/// </summary>
	/// <returns><c>true</c> if this specifed GO is some ort of ground otherwise, <c>false</c>.</returns>
	/// <param name="obj">Object.</param>
	public static bool IsGround(GameObject obj)
	{
		return obj.layer == 8;
	}


}
