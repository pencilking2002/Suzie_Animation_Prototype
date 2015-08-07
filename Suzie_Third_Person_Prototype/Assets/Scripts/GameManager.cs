using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public static GameManager Instance;
	public static bool debug = true;			// Toggle debug mode
	public CharState charState;
	
	private void Awake ()
	{
		if (Instance == null)
			Instance = this;
	}
}
