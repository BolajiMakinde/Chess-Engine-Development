using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomENGINE : MonoBehaviour {

	/// <summary>
    /// Returns 1 if passed true or -1 if passed false.
    /// </summary>
    /// <param name="myBool">Parameter value to pass.</param>
    /// <returns>Returns an integer based on the passed value.</returns>

	public PieceManager pm;
	public int[,] Calc (int[,] input)
	{
		int index = 0;
		pm.isLegal(pm.chessboardarray);
		if(pm.legalMoves.Count > 0)
		{
			index = Random.Range(0, pm.legalMoves.Count-1);
		}
		else
		{
			print("no legal moves");
			Debug.Break();
			return input;
		}
		return pm.legalMoves[index];
	}
}
