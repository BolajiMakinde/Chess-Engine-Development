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
		//pm.GenerateMatrix();
		pm.isLegal(pm.chessboardarray, false);
		if(pm.legalMoves.Count > 0)
		{
			index = Random.Range(0, pm.legalMoves.Count);
		}
		else
		{
			if (pm.legalMoves.Count == 0) {
			//GenerateMatrix();
				if(pm.kinginchecks(pm.blackkingpos, pm.chessboardarray))
				{
					pm.blackkingincheck = true;
				}
				else if (pm.kinginchecks(pm.whitekingpos, pm.chessboardarray))
				{
					pm.whitekingincheck = true;
				}
			}
			if(pm.RinseandRepeat == true)
				{
					print("no legal moves");
					//pm.ResetBoard();
					//pm.whitesTurn = true;
					return input;
				}
				else
				{
					Debug.Break();
				}
		}
		return pm.legalMoves[index];
	}
}
