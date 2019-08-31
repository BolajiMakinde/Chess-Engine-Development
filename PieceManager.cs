using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PieceManager : MonoBehaviour {

	/* Piece Spacing--------
	 * 1.12875
	 * Piece Mapping--------
	 * Empty Square - 0
	 * White Pawn - 1 
	 * White Knight - 2
	 * White Bishop - 3
	 * White Rook - 4
	 * White Queen - 5
	 * White King - 6
	 * Black Pawn - 7
	 * Black Knight - 8
	 * Black Bishop - 9
	 * Black Rook - 10
	 * Black Queen - 11
	 * Black King - 12
	 */
	public int[,] chessboardarray = new int [,] { /*A File*/ {4,1,0,0,0,0,7,10}, /*B File*/ {2,1,0,0,0,0,7,8}, /*C File*/ {3,1,0,0,0,0,7,9}, /*D File*/ {5,1,0,0,0,0,7,11}, /*E File*/ {6,1,0,0,0,0,7,12}, /*F File*/ {3,1,0,0,0,0,7,9}, /*G File*/ {2,1,0,0,0,0,7,8}, /*H File*/ {4,1,0,0,0,0,7,10}};
	public int[,] originalChessBoardArray = new int [,] { /*A File*/ {4,1,0,0,0,0,7,10}, /*B File*/ {2,1,0,0,0,0,7,8}, /*C File*/ {3,1,0,0,0,0,7,9}, /*D File*/ {5,1,0,0,0,0,7,11}, /*E File*/ {6,1,0,0,0,0,7,12}, /*F File*/ {3,1,0,0,0,0,7,9}, /*G File*/ {2,1,0,0,0,0,7,8}, /*H File*/ {4,1,0,0,0,0,7,10}};
	public int[,] checkChessBoardArray = new int[,] { /*A File*/ {0,0,0,0,0,0,0,0}, /*B File*/ {0,0,0,0,0,0,0,0}, /*C File*/ {0,0,0,0,0,0,0,0}, /*D File*/ {0,0,0,0,0,0,0,0}, /*E File*/ {0,0,0,0,0,0,0,0}, /*F File*/ {0,0,0,0,0,0,0,0}, /*G File*/ {0,0,0,0,0,0,0,0}, /*H File*/ {0,0,0,0,0,0,0,0}};
	public List<int[,]> legalMoves = new List<int[,]>();
	public GameObject[] chesspieces;
	public string[] chesspiecenames; 
	public int[] chesspieceid;
	public Transform seletedobjected;
	public bool isDragging;
	public Vector3 originalLoc;
	public Sprite wpawnname;
	public Sprite wrookname;
	public Sprite wknightname;
	public Sprite wbishopname;
	public Sprite wkingname;
	public Sprite wqueenname;
	public Sprite bpawnname;
	public Sprite brookname;
	public Sprite bknightname;
	public Sprite bbishopname;
	public Sprite bkingname;
	public Sprite bqueenname;
	public string[] displaymatrix;
	public bool printingarray;
	public bool printingcheckarray;
	public bool whitesTurn;
	public bool generateLegalMoves;
	public bool isLeegal;
	public int[,]editedarray = new int[,] { /*A File*/ {0,0,0,0,0,0,0,0}, /*B File*/ {0,0,0,0,0,0,0,0}, /*C File*/ {0,0,0,0,0,0,0,0}, /*D File*/ {0,0,0,0,0,0,0,0}, /*E File*/ {0,0,0,0,0,0,0,0}, /*F File*/ {0,0,0,0,0,0,0,0}, /*G File*/ {0,0,0,0,0,0,0,0}, /*H File*/ {0,0,0,0,0,0,0,0}};
	// Use this for initialization
	void Start () {
		int count = 0;
		foreach (GameObject chesspiece in chesspieces) {
			chesspiecenames[count] = chesspiece.name;
			count = count + 1;
				
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			OnMouseDown();
		} else if (Input.GetMouseButton (0)) {
			OnMouseDrag ();
		} else if (Input.GetMouseButtonUp (0)) {
			OnMouseUp();
		}
		//moves the pieces
		if (isDragging == true) {
			Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			pz.z = -1;
			seletedobjected.transform.position = pz;
		}
		if (printingarray == true) {
			printArray (chessboardarray);
			printingarray = false;
		}
		if (printingcheckarray == true) {
			printArray (checkChessBoardArray);
			printingcheckarray = false;
		}
	}

	public void GenerateMatrix()
	{
		int[,] generatedarray= new int [,] { /*A File*/ {0,0,0,0,0,0,0,0}, /*B File*/ {0,0,0,0,0,0,0,0}, /*C File*/ {0,0,0,0,0,0,0,0}, /*D File*/ {0,0,0,0,0,0,0,0}, /*E File*/ {0,0,0,0,0,0,0,0}, /*F File*/ {0,0,0,0,0,0,0,0}, /*G File*/ {0,0,0,0,0,0,0,0}, /*H File*/ {0,0,0,0,0,0,0,0}};
		checkChessBoardArray = new int[,] { /*A File*/ {0,0,0,0,0,0,0,0}, /*B File*/ {0,0,0,0,0,0,0,0}, /*C File*/ {0,0,0,0,0,0,0,0}, /*D File*/ {0,0,0,0,0,0,0,0}, /*E File*/ {0,0,0,0,0,0,0,0}, /*F File*/ {0,0,0,0,0,0,0,0}, /*G File*/ {0,0,0,0,0,0,0,0}, /*H File*/ {0,0,0,0,0,0,0,0}};
		foreach (GameObject piece in chesspieces) {
			// which file piece is in
			float x;
			int x1;
			// which rank piece is in
			float y;
			int y1;

			x = piece.transform.position.x;
			y = piece.transform.position.y;
			x = x / ((1.12875f));
			y = y / ((1.12875f));

			//odd rounding for x
			if (Mathf.Round (Mathf.Abs(x)) % 2 != 0) {
				x = Mathf.Round (x);
			} else if (Mathf.Abs(x) - Mathf.Round (Mathf.Abs(x)) > 0) {
				if (x > 0) {
					x = Mathf.Round (x) + 1;
				} else {
					x = Mathf.Round (x) - 1;
				}
			} else {
				if (x > 0) {
					x = Mathf.Round (x) - 1;
				} else {
					x = Mathf.Round (x) + 1;
				}
			}

			x = x-1;
			x = x/2;
			x=x+4;
			x1= (int)x;

			//odd rounding for y
			if (Mathf.Round (Mathf.Abs(y)) % 2 != 0) {
				y = Mathf.Round (y);
			} else if (Mathf.Abs(y) - Mathf.Round (Mathf.Abs(y)) > 0) {
				if (y > 0) {
					y = Mathf.Round (y) + 1;
				} else {
					y = Mathf.Round (y) - 1;
				}
			} else {
				if (y > 0) {
					y = Mathf.Round (y) - 1;
				} else {
					y = Mathf.Round (y) + 1;
				}
			}
			y = y-1;
			y = y/2;
			y = y+4;
			y1 = (int)y;
			if(piece.GetComponent<SpriteRenderer>().sprite == wpawnname)
			{
				generatedarray[x1,y1] = 1;
				if(x1+1 <= 7)
				{
					checkChessBoardArray[x1+1,y1+1] = 1;
				}
				if(x1-1 >= 0)
				{
					checkChessBoardArray[x1-1,y1+1] = 1;
				}
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == wrookname)
			{
				generatedarray[x1,y1] = 4;
				for(int counteer = 0; counteer < 4; counteer ++)
				{
					bool blocked = false;
					int cou = 1;
					while(cou <= 7 && blocked == false)
					{
						if(counteer == 0 && (y1+cou >= 7 || chessboardarray[x1,y1+cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 1 && (y1-cou <= 0 || chessboardarray[x1,y1-cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 2 && (x1+cou >= 7 || chessboardarray[x1+cou,y1] != 0))
						{
							blocked = true;
						}
						if(counteer == 3 && (x1-cou <= 0 || chessboardarray[x1-cou,y1] != 0))
						{
							blocked = true;
						}
						if(counteer == 0 && blocked == false)
						{
							if(y1 + cou <= 7)
							{
								checkChessBoardArray[x1,y1+cou] = 1;
							}
						}
						if(counteer == 1 && blocked == false)
						{
							if(y1 - cou >= 0)
							{
								checkChessBoardArray[x1,y1-cou] = 1;
							}
						}
						if(counteer == 2 && blocked == false)
						{
							if(x1 + cou <= 7)
							{
								checkChessBoardArray[x1+cou,y1] = 1;
							}
						}
						if(counteer == 3 && blocked == false)
						{
							if(x1 - cou >= 0)
							{
								checkChessBoardArray[x1-cou,y1] = 1;
							}
						}
						cou++;
					}
				}
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == wknightname)
			{
				generatedarray[x1,y1] = 2;
				if(x1+1 <= 7 && y1+2 <= 7)
				{
					checkChessBoardArray[x1+1,y1+2] = 1;
				}
				if(x1-1 >= 0 && y1+2 <= 7)
				{
					checkChessBoardArray[x1-1,y1+2] = 1;
				}
				if(x1+1 <= 7 && y1-2 >= 0)
				{
					checkChessBoardArray[x1+1,y1-2] = 1;
				}
				if(x1-1 >= 0 && y1-2 >= 0)
				{
					checkChessBoardArray[x1-1,y1-2] = 1;
				}
				if(x1+2 <= 7 && y1+1 <= 7)
				{
					checkChessBoardArray[x1+2,y1+1] = 1;
				}
				if(x1-2 >= 0 && y1+1 <= 7)
				{
					checkChessBoardArray[x1-2,y1+1] = 1;
				}
				if(x1+2 <= 7 && y1-1 > 0)
				{
					checkChessBoardArray[x1+2,y1-1] = 1;
				}
				if(x1-2 >= 0 && y1-1 >= 0)
				{
					checkChessBoardArray[x1-2,y1-1] = 1;
				}
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == wbishopname)
			{
				generatedarray[x1,y1] = 3;
				for(int counteer = 0; counteer < 4; counteer ++)
				{
					bool blocked = false;
					int cou = 1;
					while(cou <= 8 && blocked == false)
					{
						if(counteer == 0 && (y1+cou >= 7 || x1+cou >= 7|| chessboardarray[x1+cou,y1+cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 1 && (y1-cou <= 0 || x1+cou >= 7|| chessboardarray[x1+cou,y1-cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 2 && (y1+cou >= 7 || x1-cou <= 0|| chessboardarray[x1-cou,y1+cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 3 && (y1-cou <= 0 || x1-cou <= 0|| chessboardarray[x1-cou,y1-cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 0 && blocked == false)
						{
							if(y1 + cou <= 7 && x1 + cou <= 7)
							{
								checkChessBoardArray[x1+cou,y1+cou] = 1;
							}
						}
						if(counteer == 1 && blocked == false)
						{
							if(y1 - cou >= 0 && x1 + cou <= 7)
							{
								checkChessBoardArray[x1+cou,y1-cou] = 1;
							}
						}
						if(counteer == 2 && blocked == false)
						{
							if(y1 + cou <= 7 && x1 - cou >= 0)
							{
								checkChessBoardArray[x1-cou,y1+cou] = 1;
							}
						}
						if(counteer == 3 && blocked == false)
						{
							if(y1 - cou >= 0 && x1 - cou >= 0)
							{
								checkChessBoardArray[x1-cou,y1-cou] = 1;
							}
						}
						cou++;
					}
				}
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == wkingname)
			{
				generatedarray[x1,y1] = 6;
				if(x1+1 <= 7)
				{
					if(y1+1 <= 7)
					{
						checkChessBoardArray[x1+1,y1+1] = 1;
					}
					checkChessBoardArray[x1+1,y1] = 1;
					if(y1-1 >= 0)
					{
						checkChessBoardArray[x1+1,y1-1] = 1;
					}
				}
				if(x1-1 >= 0)
				{
					if(y1+1 <= 7)
					{
						checkChessBoardArray[x1-1,y1+1] = 1;
					}
					checkChessBoardArray[x1-1,y1] = 1;
					if(y1-1 >= 0)
					{
						checkChessBoardArray[x1-1,y1-1] = 1;
					}
				}
				if(y1+1 <=7)
				{
					checkChessBoardArray[x1,y1+1] = 1;
				}
				if(y1-1 >= 0)
				{
					checkChessBoardArray[x1,y1-1] = 1;
				}
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == wqueenname)
			{
				generatedarray[x1,y1] = 5;
				for(int counteer = 0; counteer < 8; counteer ++)
				{
					bool blocked = false;
					int cou = 1;
					while(cou <= 8 && blocked == false)
					{
						if(counteer == 0 && (y1+cou >= 7 || x1+cou >= 7|| chessboardarray[x1+cou,y1+cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 1 && (y1-cou <= 0 || x1+cou >= 7|| chessboardarray[x1+cou,y1-cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 2 && (y1+cou >= 7 || x1-cou <= 0|| chessboardarray[x1-cou,y1+cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 3 && (y1-cou <= 0 || x1-cou <= 0|| chessboardarray[x1-cou,y1-cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 0 && blocked == false)
						{
							if(y1 + cou <= 7 && x1 + cou <= 7)
							{
								checkChessBoardArray[x1+cou,y1+cou] = 1;
							}
						}
						if(counteer == 1 && blocked == false)
						{
							if(y1 - cou >= 0 && x1 + cou <= 8)
							{
								checkChessBoardArray[x1+cou,y1-cou] = 1;
							}
						}
						if(counteer == 2 && blocked == false)
						{
							if(y1 + cou <= 7 && x1 - cou >= 0)
							{
								checkChessBoardArray[x1-cou,y1+cou] = 1;
							}
						}
						if(counteer == 3 && blocked == false)
						{
							if(y1 - cou >= 0 && x1 - cou >= 0)
							{
								checkChessBoardArray[x1-cou,y1-cou] = 1;
							}
						}
						if(counteer == 4 && (y1+cou >= 7 || chessboardarray[x1,y1+cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 5 && (y1-cou <= 0 || chessboardarray[x1,y1-cou] != 0))
						{
							blocked = true;
						}
						if(counteer == 6 && (x1+cou >= 7 || chessboardarray[x1+cou,y1] != 0))
						{
							blocked = true;
						}
						if(counteer == 7 && (x1-cou <= 0 || chessboardarray[x1-cou,y1] != 0))
						{
							blocked = true;
						}
						if(counteer == 4 && blocked == false)
						{
							if(y1 + cou <= 7)
							{
								checkChessBoardArray[x1,y1+cou] = 1;
							}
						}
						if(counteer == 5 && blocked == false)
						{
							if(y1 - cou >= 0)
							{
								checkChessBoardArray[x1,y1-cou] = 1;
							}
						}
						if(counteer == 6 && blocked == false)
						{
							if(x1 + cou <= 7)
							{
								checkChessBoardArray[x1+cou,y1] = 1;
							}
						}
						if(counteer == 7 && blocked == false)
						{
							if(x1 - cou >= 0)
							{
								checkChessBoardArray[x1-cou,y1] = 1;
							}
						}
						cou++;
					}
				}
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == bpawnname)
			{
				generatedarray[x1,y1] = 7;
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == brookname)
			{
				generatedarray[x1,y1] = 10;
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == bknightname)
			{
				generatedarray[x1,y1] = 8;
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == bbishopname)
			{
				generatedarray[x1,y1] = 9;
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == bkingname)
			{
				generatedarray[x1,y1] = 12;
			}
			else if(piece.GetComponent<SpriteRenderer>().sprite == bqueenname)
			{
				generatedarray[x1,y1] = 11;
			}
		}
		isLegal(chessboardarray);
		isLeegal = false;
		foreach (int[,] move in legalMoves)
		{
			string combinedmove = "";
			string combinedboard = "";
			foreach(int num in move)
			{
				combinedmove = combinedmove + num.ToString(); 
			}
			foreach(int num1 in generatedarray)
			{
				combinedboard = combinedboard + num1.ToString(); 
			}
			if(combinedmove == combinedboard)
			{
				isLeegal = true;
			}
		}
		if(isLeegal == false)
		{
			seletedobjected.transform.position = originalLoc;
		}
		else{
			chessboardarray = generatedarray;
			whitesTurn = !whitesTurn;
		}
	}

	public void printArray( int[,] printarray) {
		int col = 0;
		int rank = 0;
		string text = "";
		while (rank < 8) {
			while (col < 8) {
				text = text + " | " + printarray [col, rank].ToString();
				col++;
			}
			print (text);
			text = "";
			rank++;
			col = 0;
		}
	}

	public void isLegal (int[,] testmatrix)
	{
		//return all legal moves (legal matricies)
		legalMoves.Clear ();
		int col = 0;
		int rank = -1;
		foreach(int num in testmatrix)
		{
			if (col <= 7) {
				if (rank < 7) {
					rank++;
				} else {
					col++;
					rank = 0;
				}
			}
			editedarray = (int[,])testmatrix.Clone();
            if (num == 1 && whitesTurn == true) {
                if (testmatrix[col, rank + 1] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col, rank + 1] = 1;
                    legalMoves.Add(editedarray);
                }

                editedarray = testmatrix;

                if (rank == 1 && testmatrix[col, rank + 2] == 0 && testmatrix[col, rank + 1] == 0) {
                    //editedarray = testmatrix;
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col, rank + 2] = 1;
                    legalMoves.Add(editedarray);
                }
            }
            //Black Pawn Legal Moves
            else if (num == 7 && whitesTurn == false) {
                if (rank - 1 >= 0 && testmatrix[col, rank - 1] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col, rank - 1] = 1;
                    legalMoves.Add(editedarray);
                }
                editedarray = testmatrix;
                if (rank == 6 && rank - 2 >= 0 && testmatrix[col, rank - 2] == 0 && testmatrix[col, rank - 1] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col, rank - 2] = 1;
                    legalMoves.Add(editedarray);

                }
            }
            else if (num == 2 && whitesTurn == true) {
                if (col + 1 <= 7 && rank + 2 <= 7 && testmatrix[col + 1, rank + 2] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 1, rank + 2] = 2;
                    legalMoves.Add(editedarray);
                }
                if (col + 1 <= 7 && rank - 2 >= 0 && testmatrix[col + 1, rank - 2] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 1, rank - 2] = 2;
                    legalMoves.Add(editedarray);
                }
                if (col - 1 >= 0 && rank + 2 <= 7 && testmatrix[col - 1, rank + 2] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 1, rank + 2] = 2;
                    legalMoves.Add(editedarray);
                }
                if (col - 1 >= 0 && rank - 2 >= 0 && testmatrix[col - 1, rank - 2] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 1, rank - 2] = 2;
                    legalMoves.Add(editedarray);
                }
                if (col + 2 <= 7 && rank + 1 <= 7 && testmatrix[col + 2, rank + 1] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 2, rank + 1] = 2;
                    legalMoves.Add(editedarray);
                }
                if (col + 2 <= 7 && rank - 1 >= 0 && testmatrix[col + 2, rank - 1] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 2, rank - 1] = 2;
                    legalMoves.Add(editedarray);
                }
                if (col - 2 >= 0 && rank + 1 <= 7 && testmatrix[col - 2, rank + 1] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 2, rank + 1] = 2;
                    legalMoves.Add(editedarray);
                }
                if (col - 2 >= 0 && rank - 1 >= 0 && testmatrix[col - 2, rank - 1] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 2, rank - 1] = 2;
                    legalMoves.Add(editedarray);

                }
            }
            else if (num == 3 && whitesTurn == true)//bishop white moves
            {
                if (col + 1 <= 7 && rank + 1 <= 7 && testmatrix[col + 1, rank + 1] == 0)
                {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 1, rank + 1] = 3;
                    legalMoves.Add(editedarray);
                    if (col + 2 <= 7 && rank + 2 <= 7 && testmatrix[col + 2, rank + 2] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col + 2, rank + 2] = 3;
                        legalMoves.Add(editedarray);
                        if (col + 3 <= 7 && rank + 3 <= 7 && testmatrix[col + 3, rank + 3] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col + 3, rank + 3] = 3;
                            legalMoves.Add(editedarray);
                            if (col + 4 <= 7 && rank + 4 <= 7 && testmatrix[col + 4, rank + 4] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col + 4, rank + 4] = 3;
                                legalMoves.Add(editedarray);
                                if (col + 5 <= 7 && rank + 5 <= 7 && testmatrix[col + 5, rank + 5] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col + 5, rank + 5] = 3;
                                    legalMoves.Add(editedarray);
                                    if (col + 6 <= 7 && rank + 6 <= 7 && testmatrix[col + 6, rank + 6] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col + 6, rank + 6] = 3;
                                        legalMoves.Add(editedarray);
                                        if (col + 7 <= 7 && rank + 7 <= 7 && testmatrix[col + 7, rank + 7] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col + 7, rank + 7] = 3;
                                            legalMoves.Add(editedarray);
                                        }
                                    }
                                }

                            }
                        }


                    }

                }
                if (col - 1 >= 0 && rank + 1 <= 7 && testmatrix[col - 1, rank + 1] == 0)
                {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 1, rank + 1] = 3;
                    legalMoves.Add(editedarray);
                    if (col - 2 >= 0 && rank + 2 <= 7 && testmatrix[col - 2, rank + 2] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col - 2, rank + 2] = 3;
                        legalMoves.Add(editedarray);
                        if (col - 3 >= 0 && rank + 3 <= 7 && testmatrix[col - 3, rank + 3] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col - 3, rank + 3] = 3;
                            legalMoves.Add(editedarray);
                            if (col - 4 >= 0 && rank + 4 <= 7 && testmatrix[col - 4, rank + 4] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col - 4, rank + 4] = 3;
                                legalMoves.Add(editedarray);
                                if (col - 5 >= 0 && rank + 5 <= 7 && testmatrix[col - 5, rank + 5] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col - 5, rank + 5] = 3;
                                    legalMoves.Add(editedarray);
                                    if (col - 6 >= 0 && rank + 6 <= 7 && testmatrix[col - 6, rank + 6] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col - 6, rank + 6] = 3;
                                        legalMoves.Add(editedarray);
                                        if (col - 7 >= 0 && rank + 7 <= 7 && testmatrix[col - 7, rank + 7] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col - 7, rank + 7] = 3;
                                            legalMoves.Add(editedarray);
                                        }
                                    }
                                }

                            }
                        }


                    }

                }
                if (col + 1 <= 7 && rank - 1 >= 0 && testmatrix[col + 1, rank - 1] == 0)
                {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 1, rank - 1] = 3;
                    legalMoves.Add(editedarray);
                    if (col + 2 <= 7 && rank - 2 >= 0 && testmatrix[col + 2, rank - 2] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col + 2, rank - 2] = 3;
                        legalMoves.Add(editedarray);
                        if (col + 3 <= 7 && rank - 3 >= 0 && testmatrix[col + 3, rank - 3] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col + 3, rank - 3] = 3;
                            legalMoves.Add(editedarray);
                            if (col + 4 <= 7 && rank - 4 >= 0 && testmatrix[col + 4, rank - 4] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col + 4, rank - 4] = 3;
                                legalMoves.Add(editedarray);
                                if (col + 5 <= 7 && rank - 5 >= 0 && testmatrix[col + 5, rank - 5] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col + 5, rank - 5] = 3;
                                    legalMoves.Add(editedarray);
                                    if (col + 6 <= 7 && rank - 6 >= 0 && testmatrix[col + 6, rank - 6] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col + 6, rank - 6] = 3;
                                        legalMoves.Add(editedarray);
                                        if (col + 7 <= 7 && rank - 7 >= 0 && testmatrix[col + 7, rank - 7] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col + 7, rank - 7] = 3;
                                            legalMoves.Add(editedarray);
                                        }
                                    }
                                }

                            }
                        }


                    }

                }
                if (col - 1 >= 0 && rank - 1 >= 0 && testmatrix[col - 1, rank - 1] == 0)
                {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 1, rank - 1] = 3;
                    legalMoves.Add(editedarray);
                    if (col - 2 >= 0 && rank - 2 >= 0 && testmatrix[col - 2, rank - 2] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col - 2, rank - 2] = 3;
                        legalMoves.Add(editedarray);
                        if (col - 3 >= 0 && rank - 3 >= 0 && testmatrix[col - 3, rank - 3] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col - 3, rank - 3] = 3;
                            legalMoves.Add(editedarray);
                            if (col - 4 >= 0 && rank - 4 >= 0 && testmatrix[col - 4, rank - 4] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col - 4, rank - 4] = 3;
                                legalMoves.Add(editedarray);
                                if (col - 5 <= 7 && rank - 5 >= 0 && testmatrix[col - 5, rank - 5] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col - 5, rank - 5] = 3;
                                    legalMoves.Add(editedarray);
                                    if (col - 6 >= 0 && rank - 6 >= 0 && testmatrix[col - 6, rank - 6] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col - 6, rank - 6] = 3;
                                        legalMoves.Add(editedarray);
                                        if (col - 7 >= 0 && rank - 7 >= 0 && testmatrix[col - 7, rank - 7] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col - 7, rank - 7] = 3;
                                            legalMoves.Add(editedarray);
                                        }
                                    }
                                }

                            }
                        }


                    }

                }
            }
            else if (num == 4 && whitesTurn == true)//white rook legal moves 
            {
                if (rank + 1 <= 7 && testmatrix[col, rank + 1] == 0)
                {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col, rank + 1] = 4;
                    legalMoves.Add(editedarray);
                    if (rank + 2 <= 7 && testmatrix[col, rank + 2] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col, rank + 2] = 4;
                        legalMoves.Add(editedarray);
                        if (rank + 3 <= 7 && testmatrix[col, rank + 3] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col, rank + 3] = 4;
                            legalMoves.Add(editedarray);
                            if (rank + 4 <= 7 && testmatrix[col, rank + 4] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col, rank + 4] = 4;
                                legalMoves.Add(editedarray);
                                if (rank + 5 <= 7 && testmatrix[col, rank + 5] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col, rank + 5] = 4;
                                    legalMoves.Add(editedarray);
                                    if (rank + 6 <= 7 && testmatrix[col, rank + 6] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col, rank + 6] = 4;
                                        legalMoves.Add(editedarray);
                                        if (rank + 7 <= 7 && testmatrix[col, rank + 7] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col, rank + 7] = 4;
                                            legalMoves.Add(editedarray);
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
                if (col + 1 <= 7 && testmatrix[col + 1, rank] == 0)
                {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 1, rank] = 4;
                    legalMoves.Add(editedarray);
                    if (col + 2 <= 7  && testmatrix[col + 2, rank] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col + 2, rank] = 4;
                        legalMoves.Add(editedarray);
                        if (col + 3 <= 7 && testmatrix[col + 3, rank] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col + 3, rank] = 4;
                            legalMoves.Add(editedarray);
                            if (col + 4 <= 7 && testmatrix[col + 4, rank] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col + 4, rank] = 4;
                                legalMoves.Add(editedarray);
                                if (col + 5 <= 7  && testmatrix[col + 5, rank] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col + 5, rank] = 4;
                                    legalMoves.Add(editedarray);
                                    if (col + 6 <= 7  && testmatrix[col + 6, rank] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col + 6, rank] = 4;
                                        legalMoves.Add(editedarray);
                                        if (col + 7 <= 7  && testmatrix[col + 7, rank] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col + 7, rank] = 4;
                                            legalMoves.Add(editedarray);

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (rank - 1 >= 0 && testmatrix[col, rank - 1] == 0)
                {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col, rank - 1] = 4;
                    legalMoves.Add(editedarray);
                    if (rank - 2 >= 0 && testmatrix[col, rank - 2] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col, rank - 2] = 4;
                        legalMoves.Add(editedarray);
                        if (rank - 3 >= 0 && testmatrix[col, rank - 3] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col, rank - 3] = 4;
                            legalMoves.Add(editedarray);
                            if (rank - 4 >= 0 && testmatrix[col, rank - 4] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col, rank - 4] = 4;
                                legalMoves.Add(editedarray);
                                if (rank - 5 >= 0 && testmatrix[col, rank - 5] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col, rank - 5] = 4;
                                    legalMoves.Add(editedarray);
                                    if (rank - 6 >= 0 && testmatrix[col, rank - 6] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col, rank - 6] = 4;
                                        legalMoves.Add(editedarray);
                                        if (rank - 7 >= 0 && testmatrix[col, rank - 7] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col, rank - 7] = 4;
                                            legalMoves.Add(editedarray);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (col - 1 >= 0 && testmatrix[col - 1, rank] == 0)
                {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 1, rank] = 4;
                    legalMoves.Add(editedarray);
                    if (col - 2 >= 0  && testmatrix[col - 2, rank] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col - 2, rank] = 4;
                        legalMoves.Add(editedarray);
                        if (col - 3 >= 0 && testmatrix[col - 3, rank] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col - 3, rank] = 4;
                            legalMoves.Add(editedarray);
                            if (col - 4 >= 0  && testmatrix[col - 4, rank] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col - 4, rank] = 4;
                                legalMoves.Add(editedarray);
                                if (col - 5 >= 0  && testmatrix[col - 5, rank] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col - 5, rank] = 4;
                                    legalMoves.Add(editedarray);
                                    if (col - 6 >= 0 && testmatrix[col - 6, rank] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col - 6, rank] = 4;
                                        legalMoves.Add(editedarray);
                                        if (col - 7 >= 0 && testmatrix[col - 7, rank] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col - 7, rank] = 4;
                                            legalMoves.Add(editedarray);


                                        }
                                    }
                                }
                            }
                        }
                    }
                }
			}
			//white queen legal moves
            else if (num == 5 && whitesTurn == true)
                {
                    if (rank + 1 <= 7 && testmatrix[col, rank + 1] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col, rank + 1] = 5;
                        legalMoves.Add(editedarray);
                        if (rank + 2 <= 7 && testmatrix[col, rank + 2] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col, rank + 2] = 5;
                            legalMoves.Add(editedarray);
                            if (rank + 3 <= 7 && testmatrix[col, rank + 3] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col, rank + 3] = 5;
                                legalMoves.Add(editedarray);
                                if (rank + 4 <= 7 && testmatrix[col, rank + 4] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col, rank + 4] = 5;
                                    legalMoves.Add(editedarray);
                                    if (rank + 5 <= 7 && testmatrix[col, rank + 5] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col, rank + 5] = 5;
                                        legalMoves.Add(editedarray);
                                        if (rank + 6 <= 7 && testmatrix[col, rank + 6] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col, rank + 6] = 5;
                                            legalMoves.Add(editedarray);
                                            if (rank + 7 <= 7 && testmatrix[col, rank + 7] == 0)
                                            {
                                                editedarray = (int[,])testmatrix.Clone();
                                                editedarray[col, rank] = 0;
                                                editedarray[col, rank + 7] = 5;
                                                legalMoves.Add(editedarray);
                                            }

                                        }
                                    }
                                }
                            }

                        }
                    }
                    if (col + 1 <= 7 && testmatrix[col + 1, rank] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col + 1, rank] = 5;
                        legalMoves.Add(editedarray);
                        if (col + 2 <= 7  && testmatrix[col + 2, rank] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col + 2, rank] = 5;
                            legalMoves.Add(editedarray);
                            if (col + 3 <= 7  && testmatrix[col + 3, rank] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col + 3, rank] = 5;
                                legalMoves.Add(editedarray);
                                if (col + 4 <= 7  && testmatrix[col + 4, rank] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col + 4, rank] = 5;
                                    legalMoves.Add(editedarray);
                                    if (col + 5 <= 7  && testmatrix[col + 5, rank] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col + 5, rank] = 5;
                                        legalMoves.Add(editedarray);
                                        if (col + 6 <= 7  && testmatrix[col + 6, rank] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col + 6, rank] = 5;
                                            legalMoves.Add(editedarray);
                                            if (col + 7 <= 7 && testmatrix[col + 7, rank] == 0)
                                            {
                                                editedarray = (int[,])testmatrix.Clone();
                                                editedarray[col, rank] = 0;
                                                editedarray[col + 7, rank] = 5;
                                                legalMoves.Add(editedarray);

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (rank - 1 >= 0 && testmatrix[col, rank - 1] == 0)
                    {
                        editedarray = (int[,])testmatrix.Clone();
                        editedarray[col, rank] = 0;
                        editedarray[col, rank - 1] = 5;
                        legalMoves.Add(editedarray);
                        if (rank - 2 >= 0 && testmatrix[col, rank - 2] == 0)
                        {
                            editedarray = (int[,])testmatrix.Clone();
                            editedarray[col, rank] = 0;
                            editedarray[col, rank - 2] = 5;
                            legalMoves.Add(editedarray);
                            if (rank - 3 >= 0 && testmatrix[col, rank - 3] == 0)
                            {
                                editedarray = (int[,])testmatrix.Clone();
                                editedarray[col, rank] = 0;
                                editedarray[col, rank - 3] = 5;
                                legalMoves.Add(editedarray);
                                if (rank - 4 >= 0 && testmatrix[col, rank - 4] == 0)
                                {
                                    editedarray = (int[,])testmatrix.Clone();
                                    editedarray[col, rank] = 0;
                                    editedarray[col, rank - 4] = 5;
                                    legalMoves.Add(editedarray);
                                    if (rank - 5 >= 0 && testmatrix[col, rank - 5] == 0)
                                    {
                                        editedarray = (int[,])testmatrix.Clone();
                                        editedarray[col, rank] = 0;
                                        editedarray[col, rank - 5] = 5;
                                        legalMoves.Add(editedarray);
                                        if (rank - 6 >= 0 && testmatrix[col, rank - 6] == 0)
                                        {
                                            editedarray = (int[,])testmatrix.Clone();
                                            editedarray[col, rank] = 0;
                                            editedarray[col, rank - 6] = 5;
                                            legalMoves.Add(editedarray);
                                            if (rank - 7 >= 0 && testmatrix[col, rank - 7] == 0)
                                            {
                                                editedarray = (int[,])testmatrix.Clone();
                                                editedarray[col, rank] = 0;
                                                editedarray[col, rank - 7] = 5;
                                                legalMoves.Add(editedarray);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
				if (col - 1 >= 0 && testmatrix [col - 1, rank] == 0) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col - 1, rank] = 5;
					legalMoves.Add (editedarray);
					if (col - 2 >= 0 && testmatrix [col - 2, rank] == 0) {
						editedarray = (int[,])testmatrix.Clone ();
						editedarray [col, rank] = 0;
						editedarray [col - 2, rank] = 5;
						legalMoves.Add (editedarray);
						if (col - 3 >= 0 && testmatrix [col - 3, rank] == 0) {
							editedarray = (int[,])testmatrix.Clone ();
							editedarray [col, rank] = 0;
							editedarray [col - 3, rank] = 5;
							legalMoves.Add (editedarray);
							if (col - 4 >= 0 && testmatrix [col - 4, rank] == 0) {
								editedarray = (int[,])testmatrix.Clone ();
								editedarray [col, rank] = 0;
								editedarray [col - 4, rank] = 5;
								legalMoves.Add (editedarray);
								if (col - 5 >= 0 && testmatrix [col - 5, rank] == 0) {
									editedarray = (int[,])testmatrix.Clone ();
									editedarray [col, rank] = 0;
									editedarray [col - 5, rank] = 5;
									legalMoves.Add (editedarray);
									if (col - 6 >= 0 && testmatrix [col - 6, rank] == 0) {
										editedarray = (int[,])testmatrix.Clone ();
										editedarray [col, rank] = 0;
										editedarray [col - 6, rank] = 5;
										legalMoves.Add (editedarray);
										if (col - 7 >= 0 && testmatrix [col - 7, rank] == 0) {
											editedarray = (int[,])testmatrix.Clone ();
											editedarray [col, rank] = 0;
											editedarray [col - 7, rank] = 5;
											legalMoves.Add (editedarray);
										}
									}
								}
							}
						}
					}
				}
                                            if (col + 1 <= 7 && rank + 1 <= 7 && testmatrix[col + 1, rank + 1] == 0)
                                            {
                                                editedarray = (int[,])testmatrix.Clone();
                                                editedarray[col, rank] = 0;
                                                editedarray[col + 1, rank + 1] = 5;
                                                legalMoves.Add(editedarray);
                                                if (col + 2 <= 7 && rank + 2 <= 7 && testmatrix[col + 2, rank + 2] == 0)
                                                {
                                                    editedarray = (int[,])testmatrix.Clone();
                                                    editedarray[col, rank] = 0;
                                                    editedarray[col + 2, rank + 2] = 5;
                                                    legalMoves.Add(editedarray);
                                                    if (col + 3 <= 7 && rank + 3 <= 7 && testmatrix[col + 3, rank + 3] == 0)
                                                    {
                                                        editedarray = (int[,])testmatrix.Clone();
                                                        editedarray[col, rank] = 0;
                                                        editedarray[col + 3, rank + 3] = 5;
                                                        legalMoves.Add(editedarray);
                                                        if (col + 4 <= 7 && rank + 4 <= 7 && testmatrix[col + 4, rank + 4] == 0)
                                                        {
                                                            editedarray = (int[,])testmatrix.Clone();
                                                            editedarray[col, rank] = 0;
                                                            editedarray[col + 4, rank + 4] = 5;
                                                            legalMoves.Add(editedarray);
                                                            if (col + 5 <= 7 && rank + 5 <= 7 && testmatrix[col + 5, rank + 5] == 0)
                                                            {
                                                                editedarray = (int[,])testmatrix.Clone();
                                                                editedarray[col, rank] = 0;
                                                                editedarray[col + 5, rank + 5] = 5;
                                                                legalMoves.Add(editedarray);
                                                                if (col + 6 <= 7 && rank + 6 <= 7 && testmatrix[col + 6, rank + 6] == 0)
                                                                {
                                                                    editedarray = (int[,])testmatrix.Clone();
                                                                    editedarray[col, rank] = 0;
                                                                    editedarray[col + 6, rank + 6] = 5;
                                                                    legalMoves.Add(editedarray);
                                                                    if (col + 7 <= 7 && rank + 7 <= 7 && testmatrix[col + 7, rank + 7] == 0)
                                                                    {
                                                                        editedarray = (int[,])testmatrix.Clone();
                                                                        editedarray[col, rank] = 0;
                                                                        editedarray[col + 7, rank + 7] = 5;
                                                                        legalMoves.Add(editedarray);
                                                                    }
                                                                }
                                                            }

                                                        }
                                                    }


                                                }

                                            }
                                            if (col - 1 >= 0 && rank + 1 <= 7 && testmatrix[col - 1, rank + 1] == 0)
                                            {
                                                editedarray = (int[,])testmatrix.Clone();
                                                editedarray[col, rank] = 0;
                                                editedarray[col - 1, rank + 1] = 5;
                                                legalMoves.Add(editedarray);
                                                if (col - 2 >= 0 && rank + 2 <= 7 && testmatrix[col - 2, rank + 2] == 0)
                                                {
                                                    editedarray = (int[,])testmatrix.Clone();
                                                    editedarray[col, rank] = 0;
                                                    editedarray[col - 2, rank + 2] = 5;
                                                    legalMoves.Add(editedarray);
                                                    if (col - 3 >= 0 && rank + 3 <= 7 && testmatrix[col - 3, rank + 3] == 0)
                                                    {
                                                        editedarray = (int[,])testmatrix.Clone();
                                                        editedarray[col, rank] = 0;
                                                        editedarray[col - 3, rank + 3] = 5;
                                                        legalMoves.Add(editedarray);
                                                        if (col - 4 >= 0 && rank + 4 <= 7 && testmatrix[col - 4, rank + 4] == 0)
                                                        {
                                                            editedarray = (int[,])testmatrix.Clone();
                                                            editedarray[col, rank] = 0;
                                                            editedarray[col - 4, rank + 4] = 5;
                                                            legalMoves.Add(editedarray);
                                                            if (col - 5 >= 0 && rank + 5 <= 7 && testmatrix[col - 5, rank + 5] == 0)
                                                            {
                                                                editedarray = (int[,])testmatrix.Clone();
                                                                editedarray[col, rank] = 0;
                                                                editedarray[col - 5, rank + 5] = 5;
                                                                legalMoves.Add(editedarray);
                                                                if (col - 6 >= 0 && rank + 6 <= 7 && testmatrix[col - 6, rank + 6] == 0)
                                                                {
                                                                    editedarray = (int[,])testmatrix.Clone();
                                                                    editedarray[col, rank] = 0;
                                                                    editedarray[col - 6, rank + 6] = 5;
                                                                    legalMoves.Add(editedarray);
                                                                    if (col - 7 >= 0 && rank + 7 <= 7 && testmatrix[col - 7, rank + 7] == 0)
                                                                    {
                                                                        editedarray = (int[,])testmatrix.Clone();
                                                                        editedarray[col, rank] = 0;
                                                                        editedarray[col - 7, rank + 7] = 5;
                                                                        legalMoves.Add(editedarray);
                                                                    }
                                                                }
                                                            }

                                                        }
                                                    }


                                                }

                                            }
                                            if (col + 1 <= 7 && rank - 1 >= 0 && testmatrix[col + 1, rank - 1] == 0)
                                            {
                                                editedarray = (int[,])testmatrix.Clone();
                                                editedarray[col, rank] = 0;
                                                editedarray[col + 1, rank - 1] = 5;
                                                legalMoves.Add(editedarray);
                                                if (col + 2 <= 7 && rank - 2 >= 0 && testmatrix[col + 2, rank - 2] == 0)
                                                {
                                                    editedarray = (int[,])testmatrix.Clone();
                                                    editedarray[col, rank] = 0;
                                                    editedarray[col + 2, rank - 2] = 5;
                                                    legalMoves.Add(editedarray);
                                                    if (col + 3 <= 7 && rank - 3 >= 0 && testmatrix[col + 3, rank - 3] == 0)
                                                    {
                                                        editedarray = (int[,])testmatrix.Clone();
                                                        editedarray[col, rank] = 0;
                                                        editedarray[col + 3, rank - 3] = 5;
                                                        legalMoves.Add(editedarray);
                                                        if (col + 4 <= 7 && rank - 4 >= 0 && testmatrix[col + 4, rank - 4] == 0)
                                                        {
                                                            editedarray = (int[,])testmatrix.Clone();
                                                            editedarray[col, rank] = 0;
                                                            editedarray[col + 4, rank - 4] = 5;
                                                            legalMoves.Add(editedarray);
                                                            if (col + 5 <= 7 && rank - 5 >= 0 && testmatrix[col + 5, rank - 5] == 0)
                                                            {
                                                                editedarray = (int[,])testmatrix.Clone();
                                                                editedarray[col, rank] = 0;
                                                                editedarray[col + 5, rank - 5] = 5;
                                                                legalMoves.Add(editedarray);
                                                                if (col + 6 <= 7 && rank - 6 >= 0 && testmatrix[col + 6, rank - 6] == 0)
                                                                {
                                                                    editedarray = (int[,])testmatrix.Clone();
                                                                    editedarray[col, rank] = 0;
                                                                    editedarray[col + 6, rank - 6] = 5;
                                                                    legalMoves.Add(editedarray);
                                                                    if (col + 7 <= 7 && rank - 7 >= 0 && testmatrix[col + 7, rank - 7] == 0)
                                                                    {
                                                                        editedarray = (int[,])testmatrix.Clone();
                                                                        editedarray[col, rank] = 0;
                                                                        editedarray[col + 7, rank - 7] = 5;
                                                                        legalMoves.Add(editedarray);
                                                                    }
                                                                }
                                                            }

                                                        }
                                                    }


                                                }

                                            }
				if (col - 1 >= 0 && rank - 1 >= 0 && testmatrix [col - 1, rank - 1] == 0) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col - 1, rank - 1] = 5;
					legalMoves.Add (editedarray);
					if (col - 2 >= 0 && rank - 2 >= 0 && testmatrix [col - 2, rank - 2] == 0) {
						editedarray = (int[,])testmatrix.Clone ();
						editedarray [col, rank] = 0;
						editedarray [col - 2, rank - 2] = 5;
						legalMoves.Add (editedarray);
						if (col - 3 >= 0 && rank - 3 >= 0 && testmatrix [col - 3, rank - 3] == 0) {
							editedarray = (int[,])testmatrix.Clone ();
							editedarray [col, rank] = 0;
							editedarray [col - 3, rank - 3] = 5;
							legalMoves.Add (editedarray);
							if (col - 4 >= 0 && rank - 4 >= 0 && testmatrix [col - 4, rank - 4] == 0) {
								editedarray = (int[,])testmatrix.Clone ();
								editedarray [col, rank] = 0;
								editedarray [col - 4, rank - 4] = 5;
								legalMoves.Add (editedarray);
								if (col - 5 <= 7 && rank - 5 >= 0 && testmatrix [col - 5, rank - 5] == 0) {
									editedarray = (int[,])testmatrix.Clone ();
									editedarray [col, rank] = 0;
									editedarray [col - 5, rank - 5] = 5;
									legalMoves.Add (editedarray);
									if (col - 6 >= 0 && rank - 6 >= 0 && testmatrix [col - 6, rank - 6] == 0) {
										editedarray = (int[,])testmatrix.Clone ();
										editedarray [col, rank] = 0;
										editedarray [col - 6, rank - 6] = 5;
										legalMoves.Add (editedarray);
										if (col - 7 >= 0 && rank - 7 >= 0 && testmatrix [col - 7, rank - 7] == 0) {
											editedarray = (int[,])testmatrix.Clone ();
											editedarray [col, rank] = 0;
											editedarray [col - 7, rank - 7] = 5;
											legalMoves.Add (editedarray);
										}
									}
								}
							}
						}
					}
				}
            }
            else if (num == 6 && whitesTurn == true) //white king legal moves
            {
                if(col + 1 <= 7 && rank + 1 <= 7 && testmatrix[col + 1, rank + 1] == 0) 
                    {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
					editedarray[col + 1, rank + 1] = 6;
					legalMoves.Add(editedarray);
                }
            }
		}
	}
		

                //Runs on FIRST frame of mouse click.
	void OnMouseDown () {
		seletedobjected = null;
		RaycastHit2D hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		hit = Physics2D.Raycast (ray.origin, ray.direction);
		if (hit)
		{
			//print out the name if the raycast hits something
			isDragging = true;
			seletedobjected = hit.transform;
			originalLoc = seletedobjected.transform.position;
		}
	}

	//Runs on frames while mouse is pressed and moved
	void OnMouseDrag ()    {
		if (seletedobjected != null) {
		}
	}

	//Runs on LAST frame of mouse click
	void OnMouseUp () {
		if (seletedobjected != null) {
			
			isDragging = false;

			float x;

			float y;

			Vector3 p;

			x = seletedobjected.position.x;
			y = seletedobjected.position.y;
			x = x / ((1.12875f));
			y = y / ((1.12875f));

			//odd rounding for x
			if (Mathf.Round (Mathf.Abs(x)) % 2 != 0) {
				x = Mathf.Round (x);
			} else if (Mathf.Abs(x) - Mathf.Round (Mathf.Abs(x)) > 0) {
				if (x > 0) {
					x = Mathf.Round (x) + 1;
				} else {
					x = Mathf.Round (x) - 1;
				}
			} else {
				if (x > 0) {
					x = Mathf.Round (x) - 1;
				} else {
					x = Mathf.Round (x) + 1;
				}
			}

			//odd rounding for y
			if (Mathf.Round (Mathf.Abs(y)) % 2 != 0) {
				y = Mathf.Round (y);
			} else if (Mathf.Abs(y) - Mathf.Round (Mathf.Abs(y)) > 0) {
				if (y > 0) {
					y = Mathf.Round (y) + 1;
				} else {
					y = Mathf.Round (y) - 1;
				}
			} else {
				if (y > 0) {
					y = Mathf.Round (y) - 1;
				} else {
					y = Mathf.Round (y) + 1;
				}
			}

			if(x > 7|| x < -7 || y > 7 || y < -7)
			{
				seletedobjected.transform.position = originalLoc;
				return;
			}

			x = x * (1.12875f);
			y = y * (1.12875f);
			p.x = x;
			p.y = y;
			p.z = -1;
			seletedobjected.position = p;
			GenerateMatrix ();
		}
	}
}
