using UnityEngine;
using System.Collections;

public class ProceduralNumberGenerator {

	public static int GetNextNumber() {
		string currentNum = Random.Range(1, 5).ToString();
		return int.Parse (currentNum);
	}
}
