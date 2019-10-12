using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatGreedEVALENGINE : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	public float Calc (int[,] inputcb) {
		float eval = 0;
		foreach (int num in inputcb)
		{
			if(num == 1)
			{
				eval ++;
			}
			else if(num == 2)
			{
				eval += 3;
			}
			else if(num == 3)
			{
				eval += 3;
			}
			else if(num == 4)
			{
				eval += 5;
			}
			else if(num == 5)
			{
				eval += 9;
			}
			else if(num == 7)
			{
				eval -= 1;
			}
			else if(num == 8)
			{
				eval -= 3;
			}
			else if(num == 9)
			{
				eval -= 3;
			}
			else if(num == 10)
			{
				eval -= 5;
			}
			else if(num == 11)
			{
				eval -= 9;
			}
		}
		return eval;
	}
}
