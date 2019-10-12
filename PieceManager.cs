using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
/*To do List:
Random Promote
Castle Adjustments
Check and mate fine tuning
 */
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
	public enum PlayerTypes
	{
		Human = 0,
		Random = 1,
		Greed = 2
	}
	public GameObject[] Engines;
	public PlayerTypes _blackPlayerType;
	public PlayerTypes _whitePlayerType;
	public int[,] chessboardarray = new int [,] { /*A File*/ {4,1,0,0,0,0,7,10}, /*B File*/ {2,1,0,0,0,0,7,8}, /*C File*/ {3,1,0,0,0,0,7,9}, /*D File*/ {5,1,0,0,0,0,7,11}, /*E File*/ {6,1,0,0,0,0,7,12}, /*F File*/ {3,1,0,0,0,0,7,9}, /*G File*/ {2,1,0,0,0,0,7,8}, /*H File*/ {4,1,0,0,0,0,7,10}};
	public int[,] originalChessBoardArray = new int [,] { /*A File*/ {4,1,0,0,0,0,7,10}, /*B File*/ {2,1,0,0,0,0,7,8}, /*C File*/ {3,1,0,0,0,0,7,9}, /*D File*/ {5,1,0,0,0,0,7,11}, /*E File*/ {6,1,0,0,0,0,7,12}, /*F File*/ {3,1,0,0,0,0,7,9}, /*G File*/ {2,1,0,0,0,0,7,8}, /*H File*/ {4,1,0,0,0,0,7,10}};
	public int[,] checkChessBoardArray = new int[,] { /*A File*/ {0,1,1,0,0,2,2,0}, /*B File*/ {1,1,1,0,0,2,2,2}, /*C File*/ {1,1,1,0,0,2,2,2}, /*D File*/ {1,1,1,0,0,2,2,2}, /*E File*/ {1,1,1,0,0,2,2,2}, /*F File*/ {1,1,1,0,0,2,2,2}, /*G File*/ {1,1,1,0,0,2,2,2}, /*H File*/ {0,1,1,0,0,2,2,0}};
	public List<int[,]> legalMoves = new List<int[,]>();
	public GameObject[] chesspieces;
	public string[] chesspiecenames; 
	public int[] chesspieceid;
	public Transform seletedobjected;
	public bool isDragging;
	public Vector3 originalLoc;
	public int[] originalLoccoord;
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
	public bool whiteperspective;
	public bool printingarray;
	public bool printinggamearray;
	public bool printingcheckarray;
	public bool whitesTurn;
	public bool generateLegalMoves;
	public bool isLeegal;
	public int[,]editedarray = new int[,] { /*A File*/ {0,0,0,0,0,0,0,0}, /*B File*/ {0,0,0,0,0,0,0,0}, /*C File*/ {0,0,0,0,0,0,0,0}, /*D File*/ {0,0,0,0,0,0,0,0}, /*E File*/ {0,0,0,0,0,0,0,0}, /*F File*/ {0,0,0,0,0,0,0,0}, /*G File*/ {0,0,0,0,0,0,0,0}, /*H File*/ {0,0,0,0,0,0,0,0}};
	public int[,]gametocodearray = new int[,] { /*A File*/ {0,0,0,0,0,0,0,0}, /*B File*/ {0,0,0,0,0,0,0,0}, /*C File*/ {0,0,0,0,0,0,0,0}, /*D File*/ {0,0,0,0,0,0,0,0}, /*E File*/ {0,0,0,0,0,0,0,0}, /*F File*/ {0,0,0,0,0,0,0,0}, /*G File*/ {0,0,0,0,0,0,0,0}, /*H File*/ {0,0,0,0,0,0,0,0}};
	public int[] enpassant = new int[] {0,0,0};
	public int[] storedenpassant = new int[] {0,0,0};
	public int[] entrack = new int[] {0,0};
	public bool printlegality;
	public bool whitekingmoved;
	public bool blackkingmoved;
	public GameObject wkingrook;
	public bool movedwikingrook;
	public GameObject wqueenrook;
	public bool movedwqueenrook;
	public GameObject bkingrook;
	public bool movedbkingrook;
	public GameObject bqueenrook;
	public bool movedbqueenrook;
	public GameObject selectedrook;
	public Vector3 origionalrookLoc;
	public Vector3 selectionscalefactor;
	public bool whitekingincheck;
	public int[] whitekingpos = new int[]{4,0};
	public bool blackkingincheck;
	public int[] blackkingpos = new int[] {4,7};
	public int[] wanttocapture = new int[] {-1,-1,-1,-1,-1};
	public int bpromotepref = 5;
	public int wpromotepref = 11;
	public int promotetooo = 0;
	public bool generatelegalmovesamount;
	public bool printalllegalmoves;
	public bool printgeneratedmovedecision;
	public bool checkupcall = false;
	public bool fcheckupcall = false;
	public bool ffcheck = false;
	public float wprocessdelay;
	public float bprocessdelay;
	public bool mwqueen = false;
	public bool mbqueen = false;
	public int moveCount = 0;
	public bool _createPGN;
	string movedpiece = "";
	public string lastpgnline = "";
	// Use this for initialization
	void Start () {
		int count = 0;
		foreach (GameObject chesspiece in chesspieces) {
			chesspiecenames[count] = chesspiece.name;
			count = count + 1;
				
		}
		isLegal (chessboardarray);
	}
	
	// Update is called once per frame

	void Update () {

		if (legalMoves.Count == 0) {
			GenerateMatrix();
			if (blackkingincheck == true) {
				print ("1-0: Checkmate. White is Victorius");
				Debug.Break();
			}
			else if (whitekingincheck == true) {
				print ("0-1: Checkmate. Black is Victorius");
				Debug.Break();
			}
			else
			{
				print ("1/2-1/2: Stalemate");
				Debug.Break();
			}
		}
		if((whitesTurn == true && _whitePlayerType == PlayerTypes.Human) || (whitesTurn == false && _blackPlayerType == PlayerTypes.Human))
		{
			if (Input.GetMouseButtonDown (0)) {
				OnMouseDown();
			} else if (Input.GetMouseButton (0)) {
				OnMouseDrag ();
			} else if (Input.GetMouseButtonUp (0)) {
				OnMouseUp();
			}
			//moves the pieces
			if (isDragging == true) {
				if((whitesTurn == true && _whitePlayerType == PlayerTypes.Human) || (whitesTurn == false && _blackPlayerType == PlayerTypes.Human))
				{
					Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					pz.z = -3;
					seletedobjected.transform.position = pz;
				}
			}
		}
		else if((whitesTurn == true && _whitePlayerType == PlayerTypes.Random) || (whitesTurn == false && _blackPlayerType == PlayerTypes.Random))
		{
			if(checkupcall == false)
			{
				seletedobjected = null;
				if(whitesTurn == true && fcheckupcall == false && ffcheck == false)
				{
					StartCoroutine(DelayProcess(wprocessdelay));
				}
				else if (whitesTurn == false && fcheckupcall == false && ffcheck == false)
				{
					StartCoroutine(DelayProcess(bprocessdelay));
				}
				if(fcheckupcall == true)
				{
					fcheckupcall = false;
					checkupcall = true;
					GenerateMatrix();
					int[,] feedcb = Engines[0].GetComponent<RandomENGINE>().Calc(chessboardarray);
					Interpreter(chessboardarray, feedcb, false);
					if(_createPGN == true)
					{
						UpdatePGN(chessboardarray, feedcb);
					}
					chessboardarray = feedcb;
					whitesTurn = !whitesTurn;
					GenerateMatrix();
					ffcheck = false;
					//isLegal(feedcb);
				}
				//fcheckupcall = false;
				
			}
			else
			{
				print("check in prevented");
			}
		}
		//fcheckupcall = false;
		if (printingarray == true) {
			printArray (chessboardarray);
			printingarray = false;
		}
		if (printinggamearray == true) {
			printArray (gametocodearray);
			printinggamearray = false;
		}
		if (printingcheckarray == true) {
			printArray (checkChessBoardArray);
			printingcheckarray = false;
		}
		if (generatelegalmovesamount == true) {
			isLegal (chessboardarray);
			print ("Amount of Legal moves is: " + legalMoves.Count);
			generatelegalmovesamount = false;
		}
		if (generateLegalMoves == true)
		{
			GenerateMatrix();
		}
		if(printalllegalmoves == true)
		{
			foreach (int[,] legalMove in legalMoves) {
				printArray (legalMove);
				print ("_____________________________________");
			}
			printalllegalmoves = false;
		}
		fcheckupcall = false;
	}

	public bool kinginchecks (int[] kingloc, int[,] kcheckarray)
	{
		int trueking;
		if (kcheckarray[kingloc[0],kingloc[1]] == 6) {
			trueking = 6;
		} else {
			trueking = 12;
		}
		for(int counteer = 0; counteer < 8; counteer ++)
		{
			int x1 = 0;
			int y1 = 0;
			bool blocked = false;
			int cou = 1;
			x1 = kingloc [0];
			y1 = kingloc [1];
			while(cou <= 8 && blocked == false)
			{
				if(blocked == false && counteer == 0 && (y1+cou > 7 || x1+cou > 7|| (kcheckarray[x1+cou,y1+cou] != 0 && kcheckarray[x1+cou,y1+cou] != 98 && kcheckarray[x1+cou,y1+cou] != 99)))
				{
					blocked = true;
					if (x1+cou <= 7 && y1 + cou <= 7 && ((trueking == 6 && (kcheckarray [x1 + cou, y1 + cou] == 9 || kcheckarray [x1 + cou, y1 + cou] == 11)) || (trueking == 12 && (kcheckarray [x1 + cou, y1 + cou] == 3 || kcheckarray [x1 + cou, y1 + cou] == 5)))) {
						return true;
					}
				}
				if(blocked == false && counteer == 1 && (y1-cou < 0 || x1+cou > 7|| (kcheckarray[x1+cou,y1-cou] != 0 && kcheckarray[x1+cou,y1-cou] != 98 && kcheckarray[x1+cou,y1-cou] != 99)))
				{
					blocked = true;
					if (x1+cou <= 7 && y1 - cou >= 0 && ((trueking == 6 && (kcheckarray [x1 + cou, y1 - cou] == 9 || kcheckarray [x1 + cou, y1 - cou] == 11)) || (trueking == 12 && (kcheckarray [x1 + cou, y1 - cou] == 3 || kcheckarray [x1 + cou, y1 - cou] == 5)))) {
						return true;
					}
				}
				if(blocked == false && counteer == 2 && (y1+cou > 7 || x1-cou < 0|| (kcheckarray[x1-cou,y1+cou] != 0 && kcheckarray[x1-cou,y1+cou] != 98 && kcheckarray[x1-cou,y1+cou] != 99)))
				{
					blocked = true;
					if (x1-cou >= 0 && y1 + cou <= 7 && ((trueking == 6 && (kcheckarray [x1 - cou, y1 + cou] == 9 || kcheckarray [x1 - cou, y1 + cou] == 11)) || (trueking == 12 && (kcheckarray [x1 - cou, y1 + cou] == 3 || kcheckarray [x1 - cou, y1 + cou] == 5)))) {
						return true;
					}
				}
				if(blocked == false && counteer == 3 && (y1-cou < 0 || x1-cou < 0|| (kcheckarray[x1-cou,y1-cou] != 0 && kcheckarray[x1-cou,y1-cou] != 98 && kcheckarray[x1-cou,y1-cou] != 99)))
				{
					blocked = true;
					if (x1-cou >= 0 && y1 - cou >= 0 && ((trueking == 6 && (kcheckarray [x1 - cou, y1 - cou] == 9 || kcheckarray [x1 - cou, y1 - cou] == 11)) || (trueking == 12 && (kcheckarray [x1 - cou, y1 - cou] == 3 || kcheckarray [x1 - cou, y1 - cou] == 5)))) {
						return true;
					}
				}
				if(blocked == false && counteer == 4 && (y1+cou > 7 || (kcheckarray[x1,y1+cou] != 0)))
				{
					blocked = true;
					if (y1 + cou <= 7 && ((trueking == 6 && (kcheckarray [x1, y1 + cou] == 10 || kcheckarray [x1, y1 + cou] == 11)) || (trueking == 12 && (kcheckarray [x1, y1 + cou] == 4 || kcheckarray [x1, y1 + cou] == 5)))) {
						return true;
					}
				}
				if(blocked == false && counteer == 5 && (y1-cou < 0 || (kcheckarray[x1,y1-cou] != 0 && kcheckarray[x1,y1-cou] != 12)))
				{
					blocked = true;
					if (y1 - cou >= 0 && ((trueking == 6 && (kcheckarray [x1, y1 - cou] == 10 || kcheckarray [x1, y1 - cou] == 11)) || (trueking == 12 && (kcheckarray [x1, y1 - cou] == 4 || kcheckarray [x1, y1 - cou] == 5)))) {
						return true;
					}
				}
				if(blocked == false && counteer == 6 && (x1+cou > 7 || (kcheckarray[x1+cou,y1] != 0 && kcheckarray[x1+cou,y1] != 12)))
				{
					blocked = true;
					if (x1+cou <= 7 && ((trueking == 6 && (kcheckarray [x1 +cou, y1] == 10 || kcheckarray [x1 +cou, y1] == 11)) || (trueking == 12 && (kcheckarray [x1 + cou, y1] == 4 || kcheckarray [x1 + cou, y1] == 5)))) {
						return true;
					}
				}
				if(blocked == false && counteer == 7 && (x1-cou < 0 || (kcheckarray[x1-cou,y1] != 0 && kcheckarray[x1-cou,y1] != 12)))
				{
					blocked = true;
					if (x1-cou >= 0 && ((trueking == 6 && (kcheckarray [x1 - cou, y1] == 10 || kcheckarray [x1 - cou, y1] == 11)) || (trueking == 12 && (kcheckarray [x1 - cou, y1] == 4 || kcheckarray [x1 - cou, y1] == 5)))) {
						return true;
					}
				}
				cou++;
			}
		}
		//coverblackpawns
		if (/*white pawn checks*/trueking == 12 && ((kingloc[0]+1 <= 7 && kingloc[1]-1 >= 0 && kcheckarray[kingloc[0]+1,kingloc[1]-1] == 1) || (kingloc[0]-1 >= 0 && kingloc[1]-1 >= 0 && kcheckarray[kingloc[0]-1,kingloc[1]-1] == 1))) {
			return true;
		}
		else if (/*black pawn checks*/trueking == 6 && ((kingloc[0]+1 <= 7 && kingloc[1]+1 <= 7 && kcheckarray[kingloc[0]+1,kingloc[1]+1] == 7) || (kingloc[0]-1 >= 0 && kingloc[1]+1 <= 7 && kcheckarray[kingloc[0]-1,kingloc[1]+1] == 7))) {
			return true;
		}
		else if (/*black knight checks*/trueking == 6 && ((kingloc[0]+1 <= 7 && kingloc[1]+2 <= 7 && kcheckarray[kingloc[0]+1,kingloc[1]+2] == 8) || (kingloc[0]+1 <= 7 && kingloc[1]-2 >= 0 && kcheckarray[kingloc[0]+1,kingloc[1]-2] == 8) || (kingloc[0]-1 >= 0 && kingloc[1]+2 <= 7 && kcheckarray[kingloc[0]-1,kingloc[1]+2] == 8) || (kingloc[0]-1 >= 0 && kingloc[1]-2 >= 0 && kcheckarray[kingloc[0]-1,kingloc[1]-2] == 8) || (kingloc[0]+2 <= 7 && kingloc[1]+1 <= 7 && kcheckarray[kingloc[0]+2,kingloc[1]+1] == 8) || (kingloc[0]+2 <= 7 && kingloc[1]-1 >= 0 && kcheckarray[kingloc[0]+2,kingloc[1]-1] == 8) || (kingloc[0]-2 >= 0 && kingloc[1]+1 <= 7 && kcheckarray[kingloc[0]-2,kingloc[1]+1] == 8) || (kingloc[0]-2 >= 0 && kingloc[1]-1 >= 0 && kcheckarray[kingloc[0]-2,kingloc[1]-1] == 8))) {
			return true;
		}
			else if (/*white knight checks*/trueking == 12 && ((kingloc[0]+1 <= 7 && kingloc[1]+2 <= 7 && kcheckarray[kingloc[0]+1,kingloc[1]+2] == 2) || (kingloc[0]+1 <= 7 && kingloc[1]-2 >= 0 && kcheckarray[kingloc[0]+1,kingloc[1]-2] == 2) || (kingloc[0]-1 >= 0 && kingloc[1]+2 <= 7 && kcheckarray[kingloc[0]-1,kingloc[1]+2] == 2) || (kingloc[0]-1 >= 0 && kingloc[1]-2 >= 0 && kcheckarray[kingloc[0]-1,kingloc[1]-2] == 2) || (kingloc[0]+2 <= 7 && kingloc[1]+1 <= 7 && kcheckarray[kingloc[0]+2,kingloc[1]+1] == 2) || (kingloc[0]+2 <= 7 && kingloc[1]-1 >= 0 && kcheckarray[kingloc[0]+2,kingloc[1]-1] == 2) || (kingloc[0]-2 >= 0 && kingloc[1]+1 <= 7 && kcheckarray[kingloc[0]-2,kingloc[1]+1] == 2) || (kingloc[0]-2 >= 0 && kingloc[1]-1 >= 0 && kcheckarray[kingloc[0]-2,kingloc[1]-1] == 2))) {
			return true;
		}
		else if (/*white king checks*/trueking == 12 && ((kingloc[0]+1 <= 7 && kingloc[1]+1 <= 7 && kcheckarray[kingloc[0]+1,kingloc[1]+1] == 6) || (kingloc[0]+1 <= 7 && kingloc[1]-1 >= 0 && kcheckarray[kingloc[0]+1,kingloc[1]-1] == 6) || (kingloc[0]+1 <= 7 && kcheckarray[kingloc[0]+1,kingloc[1]] == 6) || (kingloc[1]+1 <= 7 && kcheckarray[kingloc[0],kingloc[1]+1] == 6) || (kingloc[1]-1 >= 0 && kcheckarray[kingloc[0],kingloc[1]-1] == 6) || (kingloc[0]-1 >= 0 && kingloc[1]+1 <= 7 && kcheckarray[kingloc[0]-1,kingloc[1]+1] == 6) || (kingloc[0]-1 >= 0 && kcheckarray[kingloc[0]-1,kingloc[1]] == 6) || (kingloc[0]-1 >= 0 && kingloc[1]-1 >= 0 && kcheckarray[kingloc[0]-1,kingloc[1]-1] == 6))) {
			return true;
		}
		else if (/*black king checks*/trueking == 6 && ((kingloc[0]+1 <= 7 && kingloc[1]+1 <= 7 && kcheckarray[kingloc[0]+1,kingloc[1]+1] == 12) || (kingloc[0]+1 <= 7 && kingloc[1]-1 >= 0 && kcheckarray[kingloc[0]+1,kingloc[1]-1] == 12) || (kingloc[0]+1 <= 7 && kcheckarray[kingloc[0]+1,kingloc[1]] == 12) || (kingloc[1]+1 <= 7 && kcheckarray[kingloc[0],kingloc[1]+1] == 12) || (kingloc[1]-1 >= 0 && kcheckarray[kingloc[0],kingloc[1]-1] == 12) || (kingloc[0]-1 >= 0 && kingloc[1]+1 <= 7 && kcheckarray[kingloc[0]-1,kingloc[1]+1] == 12) || (kingloc[0]-1 >= 0 && kcheckarray[kingloc[0]-1,kingloc[1]] == 12) || (kingloc[0]-1 >= 0 && kingloc[1]-1 >= 0 && kcheckarray[kingloc[0]-1,kingloc[1]-1] == 12))) {
			return true;
		}
		else {
			return false;
		}
//		int rank = 0;
//		int col = 0;
//		while (rank < 8) {
//			while (col < 8) {
//				if (kcheckarray [col, rank] == 1) {
//				}
//			}
//			rank++;
//			col = 0;
//		}
	}

	public void GenerateMatrix()
	{
		int k = 0;
		//[0,1] white coords and [2,3] black coords
		int[] kingpostemp = new int[] {4,0,4,7};
		gametocodearray = new int[,] { /*A File*/ {0,0,0,0,0,0,0,0}, /*B File*/ {0,0,0,0,0,0,0,0}, /*C File*/ {0,0,0,0,0,0,0,0}, /*D File*/ {0,0,0,0,0,0,0,0}, /*E File*/ {0,0,0,0,0,0,0,0}, /*F File*/ {0,0,0,0,0,0,0,0}, /*G File*/ {0,0,0,0,0,0,0,0}, /*H File*/ {0,0,0,0,0,0,0,0}};
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
			if(x1 == -1)
			{
				print(piece.name + piece.transform.position.x);
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
			y = y-1;
			y = y/2;
			y = y+4;
			y1 = (int)y;
			if(piece.activeInHierarchy == true && k!=wanttocapture[2])
			{
				//gametocodearray[x1,y1] = k;
				if(x1 == wanttocapture[0] && y1 == wanttocapture[1] && x1 != -1 && y1 != -1 && ((whitesTurn == true && _whitePlayerType == PlayerTypes.Human) || (whitesTurn == false && _blackPlayerType == PlayerTypes.Human)))
				{
					print("im not supposed to be here, [0] is " + wanttocapture[0] + ", x1 is: "+ x1+ "[1] is "+ wanttocapture[1]+ ", y1 is "+ y1);
					if((wanttocapture[3] !=1 || y1 !=7) && (y1!=0 || wanttocapture[3] !=7))
					{
						generatedarray[x1,y1] = wanttocapture[3];
					}
					else{
						if(wanttocapture[3] == 1)// && piece.GetComponent<SpriteRenderer>().sprite == wpawnname)
						{
							promotetooo = wpromotepref;
						}
						else if(wanttocapture[3] == 7)// && piece.GetComponent<SpriteRenderer>().sprite == bpawnname)
						{
							promotetooo = bpromotepref;
						}
							generatedarray[x1,y1] = promotetooo;
					}
				}
				else{
					if(piece.GetComponent<SpriteRenderer>().sprite == wpawnname && y1 != 7)
					{
						generatedarray[x1,y1] = 1;
					}
					else if(piece.GetComponent<SpriteRenderer>().sprite == wpawnname && y1 == 7)
					{
						generatedarray[x1,y1] = promotetooo;
					}
					else if(piece.GetComponent<SpriteRenderer>().sprite == wrookname)
					{
						generatedarray[x1,y1] = 4;
					}
					else if(piece.GetComponent<SpriteRenderer>().sprite == wknightname)
					{
						generatedarray[x1,y1] = 2;
					}
					else if(piece.GetComponent<SpriteRenderer>().sprite == wbishopname)
					{
						generatedarray[x1,y1] = 3;
					}
					else if(piece.GetComponent<SpriteRenderer>().sprite == wkingname)
					{
						generatedarray[x1,y1] = 6;
					}
					else if(piece.GetComponent<SpriteRenderer>().sprite == wqueenname)
					{
						generatedarray[x1,y1] = 5;
					}
					else if(piece.GetComponent<SpriteRenderer>().sprite == bpawnname)
					{
						generatedarray[x1,y1] = 7;
					}
					else if(piece.GetComponent<SpriteRenderer>().sprite == bpawnname && y1 == 0)
					{
						generatedarray[x1,y1] = promotetooo;
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
			}
			k++;
		}
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
			if(piece.activeInHierarchy == true)
			{
				gametocodearray[x1,y1] = k;
			//	print(k);
				if(piece.GetComponent<SpriteRenderer>().sprite == wpawnname && y>7)
				{
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
					for(int counteer = 0; counteer < 4; counteer ++)
					{
						bool blocked = false;
						bool squareahead = false;
						int cou = 1;
						while(cou <= 7 && (blocked == false || squareahead == true))
						{
							if(squareahead == true)
							{
								squareahead = false;
							}
							if(blocked == false && squareahead == false && counteer == 0 && (y1+cou > 7 || (generatedarray[x1,y1+cou] != 0 && generatedarray[x1,y1+cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(blocked == false && squareahead == false && counteer == 1 && (y1-cou < 0 || (generatedarray[x1,y1-cou] != 0 && generatedarray[x1,y1-cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(blocked == false && squareahead == false && counteer == 2 && (x1+cou > 7 || (generatedarray[x1+cou,y1] != 0 && generatedarray[x1+cou,y1] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(blocked == false && squareahead == false && counteer == 3 && (x1-cou < 0 || (generatedarray[x1-cou,y1] != 0 && generatedarray[x1-cou,y1] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(counteer == 0 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7)
								{
									checkChessBoardArray[x1,y1+cou] = 1;
								}
							}
							if(counteer == 1 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0)
								{
									checkChessBoardArray[x1,y1-cou] = 1;
								}
							}
							if(counteer == 2 && (blocked == false || squareahead == true))
							{
								if(x1 + cou <= 7)
								{
									checkChessBoardArray[x1+cou,y1] = 1;
								}
							}
							if(counteer == 3 && (blocked == false || squareahead == true))
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
					if(x1+2 <= 7 && y1-1 >= 0)
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
					for(int counteer = 0; counteer < 4; counteer ++)
					{
						bool blocked = false;
						bool squareahead = false;
						int cou = 1;
						while(cou <= 8 && blocked == false)
						{
							if(squareahead == true)
							{
								squareahead = false;
							}
							if(blocked == false  && squareahead == false && counteer == 0 && (y1+cou > 7 || x1+cou > 7|| (generatedarray[x1+cou,y1+cou] != 0 && generatedarray[x1+cou,y1+cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(blocked == false && squareahead == false && counteer == 1 && (y1-cou < 0 || x1+cou > 7|| (generatedarray[x1+cou,y1-cou] != 0 && generatedarray[x1+cou,y1-cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(blocked == false && squareahead == false && counteer == 2 && (y1+cou > 7 || x1-cou < 0|| (generatedarray[x1-cou,y1+cou] != 0 && generatedarray[x1-cou,y1+cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(blocked == false && squareahead == false && counteer == 3 && (y1-cou < 0 || x1-cou < 0|| (generatedarray[x1-cou,y1-cou] != 0 && generatedarray[x1-cou,y1-cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(counteer == 0 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7 && x1 + cou <= 7)
								{
									checkChessBoardArray[x1+cou,y1+cou] = 1;
								}
							}
							if(counteer == 1 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0 && x1 + cou <= 7)
								{
									checkChessBoardArray[x1+cou,y1-cou] = 1;
								}
							}
							if(counteer == 2 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7 && x1 - cou >= 0)
								{
									checkChessBoardArray[x1-cou,y1+cou] = 1;
								}
							}
							if(counteer == 3 && (blocked == false || squareahead == true))
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
					kingpostemp[0] = x1;
					kingpostemp[1] = y1;
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
					for(int counteer = 0; counteer < 8; counteer ++)
					{
						bool blocked = false;
						bool squareahead = false;
						int cou = 1;
						while(cou <= 8 && blocked == false)
						{
							if(squareahead == true)
							{
								squareahead = false;
							}
							if(squareahead == false && blocked == false && counteer == 0 && (y1+cou > 7 || x1+cou > 7|| (generatedarray[x1+cou,y1+cou] != 0 && generatedarray[x1+cou,y1+cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 1 && (y1-cou < 0 || x1+cou > 7|| (generatedarray[x1+cou,y1-cou] != 0 && generatedarray[x1+cou,y1-cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 2 && (y1+cou > 7 || x1-cou < 0|| (generatedarray[x1-cou,y1+cou] != 0 && generatedarray[x1-cou,y1+cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 3 && (y1-cou < 0 || x1-cou < 0|| (generatedarray[x1-cou,y1-cou] != 0 && generatedarray[x1-cou,y1-cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(counteer == 0 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7 && x1 + cou <= 7)
								{
									checkChessBoardArray[x1+cou,y1+cou] = 1;
								}
							}
							if(counteer == 1 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0 && x1 + cou <= 7)
								{
									checkChessBoardArray[x1+cou,y1-cou] = 1;
								}
							}
							if(counteer == 2 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7 && x1 - cou >= 0)
								{
									checkChessBoardArray[x1-cou,y1+cou] = 1;
								}
							}
							if(counteer == 3 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0 && x1 - cou >= 0)
								{
									checkChessBoardArray[x1-cou,y1-cou] = 1;
								}
							}
							if(squareahead == false && blocked == false && counteer == 4 && (y1+cou > 7 || (generatedarray[x1,y1+cou] != 0 && generatedarray[x1,y1+cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 5 && (y1-cou < 0 || (generatedarray[x1,y1-cou] != 0 && generatedarray[x1,y1-cou] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 6 && (x1+cou > 7 || (generatedarray[x1+cou,y1] != 0 && generatedarray[x1+cou,y1] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 7 && (x1-cou < 0 || (generatedarray[x1-cou,y1] != 0 && generatedarray[x1-cou,y1] != 12)))
							{
								blocked = true;
								squareahead = true;
							}
							if(counteer == 4 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7)
								{
									checkChessBoardArray[x1,y1+cou] = 1;
								}
							}
							if(counteer == 5 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0)
								{
									checkChessBoardArray[x1,y1-cou] = 1;
								}
							}
							if(counteer == 6 && (blocked == false || squareahead == true))
							{
								if(x1 + cou <= 7)
								{
									checkChessBoardArray[x1+cou,y1] = 1;
								}
							}
							if(counteer == 7 && (blocked == false || squareahead == true))
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
				else if(piece.GetComponent<SpriteRenderer>().sprite == bpawnname && y1>0)
				{
					if(x1+1 <= 7 && y1-1 >=0)
					{
						if(checkChessBoardArray[x1+1,y1-1] == 0 || checkChessBoardArray[x1+1,y1-1] == 2){
							checkChessBoardArray[x1+1,y1-1] = 2;
						}
						else
						{
							checkChessBoardArray[x1+1,y1-1] = 3;
						}
					}
					if(x1-1 >= 0 && y1-1 >=0)
					{
						if(checkChessBoardArray[x1-1,y1-1] == 0 || checkChessBoardArray[x1-1,y1-1] == 2){
							checkChessBoardArray[x1-1,y1-1] = 2;
						}
						else
						{
							checkChessBoardArray[x1-1,y1-1] = 3;
						}
					}
				}
				else if(piece.GetComponent<SpriteRenderer>().sprite == brookname)
				{
					for(int counteer = 0; counteer < 4; counteer ++)
					{
						bool blocked = false;
						bool squareahead = false;
						int cou = 1;
						while(cou <= 7 && blocked == false)
						{
							if(squareahead == true)
							{
								squareahead = false;
							}
							if(squareahead == false && blocked == false && counteer == 0 && (y1+cou > 7 || (generatedarray[x1,y1+cou] != 0 && generatedarray[x1,y1+cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 1 && (y1-cou < 0 || (generatedarray[x1,y1-cou] != 0 && generatedarray[x1,y1-cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 2 && (x1+cou > 7 || (generatedarray[x1+cou,y1] != 0 && generatedarray[x1+cou,y1] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 3 && (x1-cou < 0 || (generatedarray[x1-cou,y1] != 0 && generatedarray[x1-cou,y1] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(counteer == 0 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7)
								{
									if(checkChessBoardArray[x1,y1+cou] == 0 || checkChessBoardArray[x1,y1+cou] == 2){
										checkChessBoardArray[x1,y1+cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1,y1+cou] = 3;
									}
								}
							}
							if(counteer == 1 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0)
								{
									if(checkChessBoardArray[x1,y1-cou] == 0 || checkChessBoardArray[x1,y1-cou] == 2){
										checkChessBoardArray[x1,y1-cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1,y1-cou] = 3;
									}
								}
							}
							if(counteer == 2 && (blocked == false || squareahead == true))
							{
								if(x1 + cou <= 7)
								{
									if(checkChessBoardArray[x1+cou,y1] == 0 || checkChessBoardArray[x1+cou,y1] == 2){
										checkChessBoardArray[x1+cou,y1] = 2;
									}
									else
									{
										checkChessBoardArray[x1+cou,y1] = 3;
									}
								}
							}
							if(counteer == 3 && (blocked == false || squareahead == true))
							{
								if(x1 - cou >= 0)
								{
									if(checkChessBoardArray[x1-cou,y1] == 0 || checkChessBoardArray[x1-cou,y1] == 2){
										checkChessBoardArray[x1-cou,y1] = 2;
									}
									else
									{
										checkChessBoardArray[x1-cou,y1] = 3;
									}
								}
							}
							cou++;
						}
					}
				}
				else if(piece.GetComponent<SpriteRenderer>().sprite == bknightname)
				{
					if(x1+1 <= 7 && y1+2 <= 7)
					{
						if(checkChessBoardArray[x1+1,y1+2] == 0 || checkChessBoardArray[x1+1,y1+2] == 2){
							checkChessBoardArray[x1+1,y1+2] = 2;
						}
						else
						{
							checkChessBoardArray[x1+1,y1+2] = 3;
						}
					}
					if(x1-1 >= 0 && y1+2 <= 7)
					{
						if(checkChessBoardArray[x1-1,y1+2] == 0 || checkChessBoardArray[x1-1,y1+2] == 2){
							checkChessBoardArray[x1-1,y1+2] = 2;
						}
						else
						{
							checkChessBoardArray[x1-1,y1+2] = 3;
						}
					}
					if(x1+1 <= 7 && y1-2 >= 0)
					{
						if(checkChessBoardArray[x1+1,y1-2] == 0 || checkChessBoardArray[x1+1,y1-2] == 2){
							checkChessBoardArray[x1+1,y1-2] = 2;
						}
						else
						{
							checkChessBoardArray[x1+1,y1-2] = 3;
						}
					}
					if(x1-1 >= 0 && y1-2 >= 0)
					{
						if(checkChessBoardArray[x1-1,y1-2] == 0 || checkChessBoardArray[x1-1,y1-2] == 2){
							checkChessBoardArray[x1-1,y1-2] = 2;
						}
						else
						{
							checkChessBoardArray[x1-1,y1-2] = 3;
						}
					}
					if(x1+2 <= 7 && y1+1 <= 7)
					{
						if(checkChessBoardArray[x1+2,y1+1] == 0 || checkChessBoardArray[x1+2,y1+1] == 2){
							checkChessBoardArray[x1+2,y1+1] = 2;
						}
						else
						{
							checkChessBoardArray[x1+2,y1+1] = 3;
						}
					}
					if(x1-2 >= 0 && y1+1 <= 7)
					{
						if(checkChessBoardArray[x1-2,y1+1] == 0 || checkChessBoardArray[x1-2,y1+1] == 2){
							checkChessBoardArray[x1-2,y1+1] = 2;
						}
						else
						{
							checkChessBoardArray[x1-2,y1+1] = 3;
						}
					}
					if(x1+2 <= 7 && y1-1 >= 0)
					{
						if(checkChessBoardArray[x1+2,y1-1] == 0 || checkChessBoardArray[x1+2,y1-1] == 2){
							checkChessBoardArray[x1+2,y1-1] = 2;
						}
						else
						{
							checkChessBoardArray[x1+2,y1-1] = 3;
						}
					}
					if(x1-2 >= 0 && y1-1 >= 0)
					{
						if(checkChessBoardArray[x1-2,y1-1] == 0 || checkChessBoardArray[x1-2,y1-1] == 2){
							checkChessBoardArray[x1-2,y1-1] = 2;
						}
						else
						{
							checkChessBoardArray[x1-2,y1-1] = 3;
						}
					}
				}
				else if(piece.GetComponent<SpriteRenderer>().sprite == bbishopname)
				{
					for(int counteer = 0; counteer < 4; counteer ++)
					{
						bool blocked = false;
						bool squareahead = false;
						int cou = 1;
						while(cou <= 8 && blocked == false)
						{
							if(squareahead == true)
							{
								squareahead = false;
							}
							if(squareahead == false && blocked == false && counteer == 0 && (y1+cou > 7 || x1+cou > 7|| (generatedarray[x1+cou,y1+cou] != 0 && generatedarray[x1+cou,y1+cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 1 && (y1-cou < 0 || x1+cou > 7|| (generatedarray[x1+cou,y1-cou] != 0 && generatedarray[x1+cou,y1-cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 2 && (y1+cou > 7 || x1-cou < 0|| (generatedarray[x1-cou,y1+cou] != 0 && generatedarray[x1-cou,y1+cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 3 && (y1-cou < 0 || x1-cou < 0|| (generatedarray[x1-cou,y1-cou] != 0 && generatedarray[x1-cou,y1-cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(counteer == 0 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7 && x1 + cou <= 7)
								{
									if(checkChessBoardArray[x1+cou,y1+cou] == 0 || checkChessBoardArray[x1+cou,y1+cou] == 2){
										checkChessBoardArray[x1+cou,y1+cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1+cou,y1+cou] = 3;
									}
								}
							}
							if(counteer == 1 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0 && x1 + cou <= 7)
								{
									if(checkChessBoardArray[x1+cou,y1-cou] == 0 || checkChessBoardArray[x1+cou,y1-cou] == 2){
										checkChessBoardArray[x1+cou,y1-cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1+cou,y1-cou] = 3;
									}
								}
							}
							if(counteer == 2 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7 && x1 - cou >= 0)
								{
									if(checkChessBoardArray[x1-cou,y1+cou] == 0 || checkChessBoardArray[x1-cou,y1+cou] == 2){
										checkChessBoardArray[x1-cou,y1+cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1-cou,y1+cou] = 3;
									}
								}
							}
							if(counteer == 3 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0 && x1 - cou >= 0)
								{
									if(checkChessBoardArray[x1-cou,y1-cou] == 0 || checkChessBoardArray[x1-cou,y1-cou] == 2){
										checkChessBoardArray[x1-cou,y1-cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1-cou,y1-cou] = 3;
									}
								}
							}
							cou++;
						}
					}
				}
				else if(piece.GetComponent<SpriteRenderer>().sprite == bkingname)
				{
					kingpostemp[2] = x1;
					kingpostemp[3] = y1;
					if(x1+1 <= 7)
					{
						if(y1+1 <= 7)
						{
							if(checkChessBoardArray[x1+1,y1+1] == 0 || checkChessBoardArray[x1+1,y1+1] == 2)
							{
								checkChessBoardArray[x1+1,y1+1] = 2;
							}
							else
							{
								checkChessBoardArray[x1+1,y1+1] = 3;
							}
						}
						if(checkChessBoardArray[x1+1,y1] == 0 || checkChessBoardArray[x1+1,y1] == 2)
						{
							checkChessBoardArray[x1+1,y1] = 2;
						}
						else
						{
							checkChessBoardArray[x1+1,y1] = 3;
						}
						if(y1-1 >= 0)
						{
							if(checkChessBoardArray[x1+1,y1-1] == 0 || checkChessBoardArray[x1+1,y1-1] == 2)
							{
								checkChessBoardArray[x1+1,y1-1] = 2;
							}
							else
							{
								checkChessBoardArray[x1+1,y1-1] = 3;
							}
						}
					}
					if(x1-1 >= 0)
					{
						if(y1+1 <= 7)
						{
							if(checkChessBoardArray[x1-1,y1+1] == 0 || checkChessBoardArray[x1-1,y1+1] == 2)
							{
								checkChessBoardArray[x1-1,y1+1] = 2;
							}
							else
							{
								checkChessBoardArray[x1-1,y1+1] = 3;
							}
						}
						if(checkChessBoardArray[x1-1,y1] == 0 || checkChessBoardArray[x1-1,y1] == 2)
						{
							checkChessBoardArray[x1-1,y1] = 2;
						}
						else
						{
							checkChessBoardArray[x1-1,y1] = 3;
						}
						if(y1-1 >= 0)
						{
							if(checkChessBoardArray[x1-1,y1-1] == 0 || checkChessBoardArray[x1-1,y1-1] == 2)
							{
								checkChessBoardArray[x1-1,y1-1] = 2;
							}
							else
							{
								checkChessBoardArray[x1-1,y1-1] = 3;
							}
						}
					}
					if(y1+1 <=7)
					{
						if(checkChessBoardArray[x1,y1+1] == 0 || checkChessBoardArray[x1,y1+1] == 2)
						{
							checkChessBoardArray[x1,y1+1] = 2;
						}
						else
						{
							checkChessBoardArray[x1,y1+1] = 3;
						}
					}
					if(y1-1 >= 0)
					{
						if(checkChessBoardArray[x1,y1-1] == 0 || checkChessBoardArray[x1,y1-1] == 2)
						{
							checkChessBoardArray[x1,y1-1] = 2;
						}
						else
						{
							checkChessBoardArray[x1,y1-1] = 3;
						}
					}
				}
				else if(piece.GetComponent<SpriteRenderer>().sprite == bqueenname)
				{
					for(int counteer = 0; counteer < 8; counteer ++)
					{
						bool blocked = false;
						bool squareahead = false;
						int cou = 1;
						while(cou <= 8 && blocked == false)
						{
							if(squareahead == true)
							{
								squareahead = false;
							}
							if(squareahead == false && blocked == false && counteer == 0 && (y1+cou > 7 || x1+cou > 7|| (generatedarray[x1+cou,y1+cou] != 0 && generatedarray[x1+cou,y1+cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 1 && (y1-cou < 0 || x1+cou > 7|| (generatedarray[x1+cou,y1-cou] != 0 && generatedarray[x1+cou,y1-cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 2 && (y1+cou > 7 || x1-cou < 0|| (generatedarray[x1-cou,y1+cou] != 0 && generatedarray[x1-cou,y1+cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 3 && (y1-cou < 0 || x1-cou < 0|| (generatedarray[x1-cou,y1-cou] != 0 && generatedarray[x1-cou,y1-cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(counteer == 0 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7 && x1 + cou <= 7)
								{
									if(checkChessBoardArray[x1+cou,y1+cou] == 0 || checkChessBoardArray[x1+cou,y1+cou] == 2)
									{
										checkChessBoardArray[x1+cou,y1+cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1+cou,y1+cou] = 3;
									}
								}
							}
							if(counteer == 1 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0 && x1 + cou <= 7)
								{
									if(checkChessBoardArray[x1+cou,y1-cou] == 0 || checkChessBoardArray[x1+cou,y1-cou] == 2)
									{
										checkChessBoardArray[x1+cou,y1-cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1+cou,y1-cou] = 3;
									}
								}
							}
							if(counteer == 2 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7 && x1 - cou >= 0)
								{
									if(checkChessBoardArray[x1-cou,y1+cou] == 0 || checkChessBoardArray[x1-cou,y1+cou] == 2)
									{
										checkChessBoardArray[x1-cou,y1+cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1-cou,y1+cou] = 3;
									}
								}
							}
							if(counteer == 3 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0 && x1 - cou >= 0)
								{
									if(checkChessBoardArray[x1-cou,y1-cou] == 0 || checkChessBoardArray[x1-cou,y1-cou] == 2)
									{
										checkChessBoardArray[x1-cou,y1-cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1-cou,y1-cou] = 3;
									}
								}
							}
							if(squareahead == false && blocked == false && counteer == 4 && (y1+cou > 7 || (generatedarray[x1,y1+cou] != 0 && generatedarray[x1,y1+cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 5 && (y1-cou < 0 || (generatedarray[x1,y1-cou] != 0 && generatedarray[x1,y1-cou] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 6 && (x1+cou > 7 || (generatedarray[x1+cou,y1] != 0 && generatedarray[x1+cou,y1] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(squareahead == false && blocked == false && counteer == 7 && (x1-cou < 0 || (generatedarray[x1-cou,y1] != 0 && generatedarray[x1-cou,y1] != 6)))
							{
								blocked = true;
								squareahead = true;
							}
							if(counteer == 4 && (blocked == false || squareahead == true))
							{
								if(y1 + cou <= 7)
								{
									if(checkChessBoardArray[x1,y1+cou] == 0 || checkChessBoardArray[x1,y1+cou] == 2)
									{
										checkChessBoardArray[x1,y1+cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1,y1+cou] = 3;
									}
								}
							}
							if(counteer == 5 && (blocked == false || squareahead == true))
							{
								if(y1 - cou >= 0)
								{
									if(checkChessBoardArray[x1,y1-cou] == 0 || checkChessBoardArray[x1,y1-cou] == 2)
									{
										checkChessBoardArray[x1,y1-cou] = 2;
									}
									else
									{
										checkChessBoardArray[x1,y1-cou] = 3;
									}
								}
							}
							if(counteer == 6 && (blocked == false || squareahead == true))
							{
								if(x1 + cou <= 7)
								{
									if(checkChessBoardArray[x1+cou,y1] == 0 || checkChessBoardArray[x1+cou,y1] == 2)
									{
										checkChessBoardArray[x1+cou,y1] = 2;
									}
									else
									{
										checkChessBoardArray[x1+cou,y1] = 3;
									}
								}
							}
							if(counteer == 7 && (blocked == false || squareahead == true))
							{
								if(x1 - cou >= 0)
								{
									if(checkChessBoardArray[x1-cou,y1] == 0 || checkChessBoardArray[x1-cou,y1] == 2)
									{
										checkChessBoardArray[x1-cou,y1] = 2;
									}
									else
									{
										checkChessBoardArray[x1-cou,y1] = 3;
									}
								}
							}
							cou++;
						}
					}
				}
			}
			k++;
		}
		if(enpassant[0] != 0 || enpassant[1] != 0)
		{
		//	print("kjdaji");
			if(enpassant[2]==1) {
				generatedarray[enpassant[0],enpassant[1]] = 98;
				//storedenpassant = enpassant;
			//	print("dafdsd");
			}
			else if(enpassant[2]==0){
				generatedarray[enpassant[0],enpassant[1]] = 99;
			//	print(enpassant[0]+"dkjaiojf"+ enpassant[1]);
				//storedenpassant = enpassant;
				//enpassant = new int[] {0,0,0};
			}
		}
		if(wanttocapture[0] !=-1)
		{
			gametocodearray[wanttocapture[0],wanttocapture[1]] = wanttocapture[4];
		}
		if(storedenpassant[2] == 0 && (storedenpassant[0]!= 0 || storedenpassant[1] != 0) && generatedarray[storedenpassant[0],storedenpassant[1]] == 7)
		{
			generatedarray [storedenpassant[0],storedenpassant[1]+1] = 0;
			chesspieces[gametocodearray[storedenpassant[0],storedenpassant[1]+1]-32].SetActive(false);
		}
		else if (storedenpassant[2] == 1 && (storedenpassant[0]!= 0 || storedenpassant[1] != 0) && generatedarray[storedenpassant[0],storedenpassant[1]] == 1)
		{
			generatedarray [storedenpassant[0],storedenpassant[1]-1] = 0;
			chesspieces[gametocodearray[storedenpassant[0],storedenpassant[1]-1]-32].SetActive(false);
		}
		isLegal(chessboardarray);
		if((whitesTurn == true && _whitePlayerType != PlayerTypes.Random)|| (whitesTurn == false && _blackPlayerType != PlayerTypes.Random))
		{
			isLeegal = false;
		}
		else
		{
			isLeegal = true;
		}
		foreach (int[,] move in legalMoves)
		{
			string combinedmove = "";
			string combinedboard = "";
			foreach(int num in move)
			{
				combinedmove = combinedmove + num.ToString();
			//	print(combinedmove);
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
//		if(checkChessBoardArray[kingpostemp[0],kingpostemp[1]] == 2 || checkChessBoardArray[kingpostemp[0],kingpostemp[1]] == 3)
//		{
//			whitekingincheck = true;
//		}
//		else
//		{
//			whitekingincheck = false;
//		}


		if((seletedobjected != null && (isLeegal == false || (whitekingincheck== true && whitesTurn == true))) && ((whitesTurn == true && _whitePlayerType == PlayerTypes.Human) || (whitesTurn == false && _blackPlayerType == PlayerTypes.Human)))
		{
			seletedobjected.transform.position = originalLoc;
			if(printlegality){
				print("Not Legal!");
				if(printgeneratedmovedecision)
				{
					printArray(generatedarray);
				}
			}
			enpassant = storedenpassant;
			//printArray(generatedarray);
		//	if (promotetooo!=0)
		//	{
	//			print ("promote to is: "+ ", generated array is: ");
	//			printArray(generatedarray);
			//}
			//enpassant = storedenpassant;
			//if(enpassant[2] == 1 && (enpassant[0] != 0 || enpassant[1] != 0))
			//{
			//	chessboardarray[enpassant[0],enpassant[1]] = 98;
			//}
//////////			if(enpassant[2] == 0 && (enpassant[0] != 0 || enpassant[1] != 0))
//			{
//				chessboardarray[enpassant[0],enpassant[1]] = 99;
//\\\\\			}
			if(origionalrookLoc != Vector3.zero)
			{
				selectedrook.transform.position = origionalrookLoc;
			}
			if(checkChessBoardArray[whitekingpos[0],whitekingpos[1]] == 2 || checkChessBoardArray[whitekingpos[0],whitekingpos[1]] == 3)
			{
			//	whitekingincheck = true;
			}
			else
			{
				whitekingincheck = false;
			}
			if(checkChessBoardArray[kingpostemp[2],kingpostemp[3]] == 1 || checkChessBoardArray[kingpostemp[2],kingpostemp[3]] == 3)
			{
			//	blackkingincheck = true;
			}
			else
			{
				blackkingincheck = false;
			}
			//print("want to capture is: " + wanttocapture[0]+","+wanttocapture[1]+","+wanttocapture[2]+","+wanttocapture[3]);
			wanttocapture = new int[] {-1,-1,-1,-1,-1};
			promotetooo = 0;
		}
		else{
			if(seletedobjected != null && ((whitesTurn == true && _whitePlayerType == PlayerTypes.Human) || (whitesTurn == false && _blackPlayerType == PlayerTypes.Human)))
			{
				if(wanttocapture[2]> -1)
				{
					chesspieces[wanttocapture[2]].SetActive(false);
					chesspieces[wanttocapture[2]].transform.Translate(40,40,40);
					print(wanttocapture[2]);
				}
				wanttocapture = new int[] {-1,-1,-1,-1,-1};
				if(_createPGN == true)
				{
					UpdatePGN(chessboardarray, generatedarray);
				}
				chessboardarray = generatedarray;
				if((whitesTurn == true && PlayerTypes.Human == _whitePlayerType) || (PlayerTypes.Human == _blackPlayerType && whitesTurn == false)) {
					whitesTurn = !whitesTurn;
					
				}
				moveCount++;
				if(printlegality){
					print("Legal!");
					
			//		printArray(generatedarray);
				}
				if(checkChessBoardArray[kingpostemp[2],kingpostemp[3]] == 1 || checkChessBoardArray[kingpostemp[2],kingpostemp[3]] == 3)
				{
					blackkingincheck = true;
				}
				else
				{
					blackkingincheck = false;
				}
				
				//foreach(GameObject piece in chesspieces)
				//{
				//	int kad = 0;
				//	if(piece.activeInHierarchy == true)
				//	{
				//		
				//	}
				//	kad++;
				//}
				if (promotetooo!=0)
				{
				//	print ("promote to is: "+ ", generated array is: ");
				//	printArray(generatedarray);
				}
				if(seletedobjected.GetComponent<SpriteRenderer>().sprite == wpawnname )
				{
					if(promotetooo == 2 && _whitePlayerType == PlayerTypes.Human)
					{
						seletedobjected.GetComponent<SpriteRenderer>().sprite = wknightname;
						promotetooo = 0;
					}
					else if(promotetooo == 3 && _whitePlayerType == PlayerTypes.Human)
					{
						seletedobjected.GetComponent<SpriteRenderer>().sprite = wbishopname;
						promotetooo = 0;
					}
					else if(promotetooo == 4 && _whitePlayerType == PlayerTypes.Human)
					{
						seletedobjected.GetComponent<SpriteRenderer>().sprite = wrookname;
						promotetooo = 0;
					}
					else if(promotetooo == 5 && _whitePlayerType == PlayerTypes.Human)
					{
						seletedobjected.GetComponent<SpriteRenderer>().sprite = wqueenname;
						promotetooo = 0;
					}
				}
				else if(seletedobjected.GetComponent<SpriteRenderer>().sprite == bpawnname)
				{
					if(promotetooo == 8 && _blackPlayerType == PlayerTypes.Human)
					{
						seletedobjected.GetComponent<SpriteRenderer>().sprite = bknightname;
						promotetooo = 0;
					}
					else if(promotetooo == 9 && _blackPlayerType == PlayerTypes.Human)
					{
						seletedobjected.GetComponent<SpriteRenderer>().sprite = bbishopname;
						promotetooo = 0;
					}
					else if(promotetooo == 10 && _blackPlayerType == PlayerTypes.Human)
					{
						seletedobjected.GetComponent<SpriteRenderer>().sprite = brookname;
						promotetooo = 0;
					}
					else if(promotetooo == 11 && _blackPlayerType == PlayerTypes.Human)
					{
						seletedobjected.GetComponent<SpriteRenderer>().sprite = bqueenname;
						promotetooo = 0;
					}
				}
				promotetooo = 0;
				//loosing castling rights
				whitekingpos = new int[] {kingpostemp[0],kingpostemp[1]};
				blackkingpos = new int[] {kingpostemp[2],kingpostemp[3]};
				if(seletedobjected.GetComponent<SpriteRenderer>().sprite == wkingname) {
					whitekingmoved = true;
				}
				else if(seletedobjected.GetComponent<SpriteRenderer>().sprite == bkingname) {
					blackkingmoved = true;
				}
				else if (seletedobjected.gameObject == wkingrook)
				{
					movedwikingrook = true;
				}
				else if (seletedobjected.gameObject == wqueenrook)
				{
					movedwqueenrook = true;
				}
				else if (seletedobjected.gameObject == bkingrook)
				{
					movedbkingrook = true;
				}
				else if (seletedobjected.gameObject == bqueenrook)
				{
					movedbqueenrook = true;
				}
				storedenpassant = enpassant;
			//	storedenpassant = new int[] {0,0,0};
			//	print("stored enpassant is :" +storedenpassant[0] + ","+ storedenpassant[1]);
				entrack = new int[] {0,0};
				enpassant = new int[] {0,0,0};
				
			}
			if((whitesTurn == true && _whitePlayerType == PlayerTypes.Human) || (whitesTurn == false && _blackPlayerType == PlayerTypes.Human))
			{
				origionalrookLoc = Vector3.zero;
				seletedobjected = null;
			}
		}
	}

	public void printArray( int[,] printarray) {
		int col = 0;
		int rank = 0;
		string text = "";
		if (whiteperspective == false) {
			while (rank < 8) {
				while (col < 8) {
					text = text + " | " + printarray [col, rank].ToString ();
					col++;
				}
				print (text);
				text = "";
				rank++;
				col = 0;
			}
		} else {
			col = 0;
			rank = 7;
			while (rank >= 0) {
				while (col <8) {
					text = text + " | " + printarray [col, rank].ToString ();
					col++;
				}
				print (text);
				text = "";
				rank--;
				col = 0;
			}
		}
	}

	public void isLegal (int[,] tessstermatrix)
	{
		//return all legal moves (legal matricies)
		int[,] testmatrix = tessstermatrix;
		if (storedenpassant[0] != 0 || storedenpassant[1] !=0) {
			testmatrix [storedenpassant [0], storedenpassant [1]] = 0;
		}
		legalMoves.Clear ();
		legalMoves.Capacity = 0;
		//int[,] zeroizedmatrix = new int[,] { /*A File*/{ 0, 0, 0, 0, 0, 0, 0, 0 }, /*B File*/{ 0, 0, 0, 0, 0, 0, 0, 0 }, /*C File*/ {0,0,0,0,0,0,0,0}, /*D File*/ {0,0,0,0,0,0,0,0}, /*E File*/ {0,0,0,0,0,0,0,0}, /*F File*/ {0,0,0,0,0,0,0,0}, /*G File*/ {0,0,0,0,0,0,0,0}, /*H File*/ {0,0,0,0,0,0,0,0}};
		//legalMoves.RemoveAll (zeroizedmatrix);
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
			bool endder = false;
			editedarray = (int[,])testmatrix.Clone();
			if (num == 98 || num == 99) {
				testmatrix [col, rank] = 0;
			}
			//testmatrix[storedenpassant[0],storedenpassant[1]] = 0;
			//white pawn legal moves
            if (num == 1 && whitesTurn == true) {
				editedarray = testmatrix;
                if (testmatrix[col, rank + 1] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
					if (rank != 6) {
						editedarray [col, rank + 1] = 1;
					} else {
						promotetooo = wpromotepref;
						editedarray [col, rank + 1] = promotetooo;
					}
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }

                }

                editedarray = testmatrix;
                if (rank == 1 && testmatrix[col, rank + 2] == 0 && testmatrix[col, rank + 1] == 0) {
                    //editedarray = testmatrix;
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col, rank + 2] = 1;
					editedarray[col,rank+1] = 99;
					if (kinginchecks (whitekingpos, editedarray) == false) {
						legalMoves.Add (editedarray);
						if(col == 3)
						{
						//	printArray (editedarray);
						}
					} else {
					//	print ("failed tests");
					}
                }
				// capture
				if (col+1<=7 && rank+1<=7 && testmatrix[col+1, rank + 1] > 6) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					if (rank != 6) {
						editedarray [col+1, rank + 1] = 1;
					} else {
						promotetooo = wpromotepref;
						editedarray [col+1, rank + 1] = promotetooo;
					}
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					//printArray (editedarray);
				}
				if (col-1>=0 && rank-1>=0 && testmatrix[col-1, rank + 1] > 6) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					if (rank != 6) {
						editedarray [col-1, rank + 1] = 1;
					} else {
						promotetooo = wpromotepref;
						editedarray [col-1, rank + 1] = promotetooo;
					}
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					//printArray (editedarray);
				}
				if (col+1<=7 && col+1 == storedenpassant[0] && rank + 1 == storedenpassant[1] && storedenpassant[2] == 1) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray [col+1, rank + 1] = 1;
					editedarray [col + 1, rank] = 0;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if (col-1>=0 && col-1 == storedenpassant[0] && rank + 1 == storedenpassant[1] && storedenpassant[2] == 1) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col-1, rank + 1] = 1;
					editedarray [col-1, rank] = 0;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
            }
            //Black Pawn Legal Moves
            else if (num == 7 && whitesTurn == false) {
				editedarray = testmatrix;
                if (rank - 1 >= 0 && testmatrix[col, rank - 1] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
					editedarray[col, rank] = 0;
					if (rank != 1) {
						editedarray [col, rank - 1] = 7;
					} else {
						promotetooo = bpromotepref;
						editedarray [col, rank - 1] = promotetooo;
					}
                    if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
                }
                editedarray = testmatrix;
                if (rank == 6 && rank - 2 >= 0 && testmatrix[col, rank - 2] == 0 && testmatrix[col, rank - 1] == 0) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col, rank - 2] = 7;
					editedarray[col, rank - 1] = 98;
                    if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }

                }
				// capture
				if (col + 1 <= 7 && rank - 1 >= 0 && testmatrix[col + 1, rank - 1] < 6 && testmatrix[col + 1, rank - 1] != 0) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					if (rank != 1) {
						editedarray [col+1, rank - 1] = 7;
					} else {
						promotetooo = bpromotepref;
						editedarray [col+1, rank - 1] = promotetooo;
					}
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if (col - 1 >= 0 && rank - 1 >= 0 && testmatrix[col - 1, rank - 1] < 6 && testmatrix[col - 1, rank - 1] != 0) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					if (rank != 1) {
						editedarray [col-1, rank - 1] = 7;
					} else {
						promotetooo = bpromotepref;
						editedarray [col-1, rank - 1] = promotetooo;
					}
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				//black enpassant
				if (col+1<=7 && col+1 == storedenpassant[0] && rank - 1 == storedenpassant[1] && storedenpassant[2] == 0) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col+1, rank - 1] = 7;
					editedarray [col + 1, rank] = 0;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if (col-1>=0 && col-1 == storedenpassant[0] && rank - 1 == storedenpassant[1] && storedenpassant[2] == 0) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col-1, rank - 1] = 7;
					editedarray [col-1, rank] = 0;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
            }
			//White Knight Legal Moves
            else if (num == 2 && whitesTurn == true) {
				if (col + 1 <= 7 && rank + 2 <= 7 && (testmatrix[col + 1, rank + 2] > 6 || testmatrix[col + 1, rank + 2] == 0)) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 1, rank + 2] = 2;
					//editedarray[storedenpassant[0],storedenpassant[1]] = 0;
                    if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
                }
				if (col + 1 <= 7 && rank - 2 >= 0 && (testmatrix[col + 1, rank - 2] > 6 || testmatrix[col + 1, rank - 2] == 0)) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 1, rank - 2] = 2;
					//editedarray[storedenpassant[0],storedenpassant[1]] = 0;
                    if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
                }
				if (col - 1 >= 0 && rank + 2 <= 7 && (testmatrix[col - 1, rank + 2] > 6 || testmatrix[col - 1, rank + 2] == 0)) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 1, rank + 2] = 2;
					//editedarray[storedenpassant[0],storedenpassant[1]] = 0;
                    if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
                }
				if (col - 1 >= 0 && rank - 2 >= 0 && (testmatrix[col - 1, rank - 2] > 6 || testmatrix[col - 1, rank - 2] == 0)) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 1, rank - 2] = 2;
					//editedarray[storedenpassant[0],storedenpassant[1]] = 0;
                    if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
                }
				if (col + 2 <= 7 && rank + 1 <= 7 && (testmatrix[col + 2, rank + 1] > 6 || testmatrix[col + 2, rank + 1] == 0)) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 2, rank + 1] = 2;
                    if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
                }
				if (col + 2 <= 7 && rank - 1 >= 0 && (testmatrix[col + 2, rank - 1] > 6 || testmatrix[col + 2, rank - 1] == 0)) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col + 2, rank - 1] = 2;
                    if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
                }
				if (col - 2 >= 0 && rank + 1 <= 7 && (testmatrix[col - 2, rank + 1] > 6 || testmatrix[col - 2, rank + 1] == 0)) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 2, rank + 1] = 2;
                    if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
                }
				if (col - 2 >= 0 && rank - 1 >= 0 && (testmatrix[col - 2, rank - 1] > 6 || testmatrix[col - 2, rank - 1] == 0)) {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
                    editedarray[col - 2, rank - 1] = 2;
                    if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }

                }
            }
			//Black Knight Legal Moves
			else if (num == 8 && whitesTurn == false) {
				if (col + 1 <= 7 && rank + 2 <= 7 && testmatrix[col + 1, rank + 2] < 6) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + 1, rank + 2] = 8;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if (col + 1 <= 7 && rank - 2 >= 0 && testmatrix[col + 1, rank - 2] < 6) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + 1, rank - 2] = 8;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if (col - 1 >= 0 && rank + 2 <= 7 && testmatrix[col - 1, rank + 2] < 6) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col - 1, rank + 2] = 8;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if (col - 1 >= 0 && rank - 2 >= 0 && testmatrix[col - 1, rank - 2] < 6) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col - 1, rank - 2] = 8;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if (col + 2 <= 7 && rank + 1 <= 7 && testmatrix[col + 2, rank + 1] < 6) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + 2, rank + 1] = 8;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if (col + 2 <= 7 && rank - 1 >= 0 && testmatrix[col + 2, rank - 1] < 6) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + 2, rank - 1] = 8;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if (col - 2 >= 0 && rank + 1 <= 7 && testmatrix[col - 2, rank + 1] < 6) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col - 2, rank + 1] = 8;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if (col - 2 >= 0 && rank - 1 >= 0 && testmatrix[col - 2, rank - 1] < 6) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col - 2, rank - 1] = 8;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }

				}
			}
            else if (num == 3 && whitesTurn == true)//bishop white moves
            {
				
				for (int i = 1;col + i <= 7 && rank + i <= 7 && (testmatrix [col + i, rank + i] > 6 || testmatrix[col + i, rank + i] == 0) && (i == 1 || testmatrix[col + i-1, rank + i-1] == 0) && endder == false; i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col + i, rank + i] = 3;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank + i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col - i >= 0 && rank + i <= 7 && (testmatrix[col - i, rank + i] > 6 || testmatrix[col - i, rank + i] == 0 && endder == false) && (i == 1 || testmatrix [col - i + 1, rank + i - 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col - i, rank + i] = 3;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank + i] != 0) {
						endder = true;
					}

				}
				endder = false;
				for (int i = 1;col + i <= 7 && rank - i >= 0 && (testmatrix[col + i, rank - i] > 6 || testmatrix[col + i, rank - i] == 0 && endder == false) && (i == 1 || testmatrix [col + i - 1, rank - i + 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col + i, rank - i] = 3;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank - i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col - i >= 0 && rank - i >= 0 && (testmatrix[col - i, rank - i] > 6 || testmatrix[col - i, rank - i] == 0 && endder == false) && (i == 1 || testmatrix [col - i + 1, rank - i + 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col - i, rank - i] = 3;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank - i] != 0) {
						endder = true;
					}
				}
            }
			else if (num == 9 && whitesTurn == false)//bishop black moves
			{
				endder = false;
				for (int i = 1;col + i <= 7 && rank + i <= 7 && testmatrix [col + i, rank + i] < 6 && (i == 1 || testmatrix [col + i - 1, rank + i - 1] == 0) && endder == false; i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col + i, rank + i] = 9;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank + i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col - i >= 0 && rank + i <= 7 && testmatrix[col - i, rank + i] < 6 && (i == 1 || testmatrix [col - i + 1, rank + i - 1] == 0) && endder == false; i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col - i, rank + i] = 9;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank + i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col + i <= 7 && rank - i >= 0 && testmatrix[col + i, rank - i] < 6 && (i == 1 || testmatrix [col + i - 1, rank - i + 1] == 0) && endder == false; i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col + i, rank - i] = 9;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank - i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col - i >= 0 && rank - i >= 0 && testmatrix[col - i, rank - i] < 6 && (i == 1 || testmatrix [col - i + 1, rank - i + 1] == 0) && endder == false; i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col - i, rank - i] = 9;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank - i] != 0) {
						endder = true;
					}
				}
			}
            else if (num == 4 && whitesTurn == true)//white rook legal moves 
            {
				endder = false;
				for (int i = 1; rank + i <= 7 && (testmatrix[col,rank+i] > 6 || testmatrix[col, rank + i] == 0) && (i == 1 || testmatrix [col, rank + i - 1] == 0) && endder == false;i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank + i] = 4;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col, rank + i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; col + i <= 7 && (testmatrix[col+i,rank] > 6 || testmatrix[col+i, rank] == 0) && (i == 1 || testmatrix [col + i - 1, rank] == 0) && endder == false;i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + i, rank] = 4;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; rank - i >= 0 && (testmatrix[col,rank-i] > 6 || testmatrix[col, rank -i] == 0) && (i == 1 || testmatrix [col, rank - i + 1] == 0) && endder == false;i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank - i] = 4;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col, rank - i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; col - i >= 0 && (testmatrix[col-i,rank] > 6 || testmatrix[col-i, rank] == 0) && endder == false && (i == 1 || testmatrix [col - i + 1, rank] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col-i, rank] = 4;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank] != 0) {
						endder = true;
					}
				}
			}
			else if (num == 10 && whitesTurn == false)//black rook legal moves 
			{
				endder = false;
				for (int i = 1; rank + i <= 7 && testmatrix[col, rank + i] < 6 && (i == 1 || testmatrix [col, rank + i -1] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank + i] = 10;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col, rank + i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; col + i <= 7 && testmatrix[col + i, rank] < 6  && (i == 1 || testmatrix [col + i - 1, rank] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + i, rank] = 10;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; rank - i >= 0 && testmatrix[col, rank - i] < 6 && (i == 1 || testmatrix [col, rank - i + 1] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank - i] = 10;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col, rank - i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; col - i >= 0 && testmatrix[col - i, rank] < 6 && (i == 1 || testmatrix [col - i + 1, rank] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col-i, rank] = 10;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank] != 0) {
						endder = true;
					}
				}
			}
			//white queen legal moves
			else if (num == 5 && whitesTurn == true) {
				//diagonal moves
				endder = false;
				for (int i = 1;col + i <= 7 && rank + i <= 7 && (testmatrix [col + i, rank + i] > 6 || testmatrix [col + i, rank + i] == 0) && (i == 1 || testmatrix [col + i - 1, rank + i - 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col + i, rank + i] = 5;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank + i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col - i >= 0 && rank + i <= 7 && (testmatrix [col - i, rank + i] > 6 || testmatrix [col - i, rank + i] == 0) && (i == 1 || testmatrix [col - i + 1, rank + i - 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col - i, rank + i] = 5;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank + i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col + i <= 7 && rank - i >= 0 && (testmatrix [col + i, rank - i] > 6 || testmatrix [col + i, rank - i] == 0) && (i == 1 || testmatrix [col + i - 1, rank - i + 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col + i, rank - i] = 5;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank-i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col - i >= 0 && rank - i >= 0 && (testmatrix [col - i, rank - i] > 6 || testmatrix [col - i, rank - i] == 0) && (i == 1 || testmatrix [col - i + 1, rank - i + 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col - i, rank - i] = 5;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank-i] != 0) {
						endder = true;
					}
				}
				endder = false;
				//horizontal and vertical moves
				for (int i = 1; rank + i <= 7 && (testmatrix [col, rank + i] > 6 || testmatrix [col, rank + i] == 0) && (i == 1 || testmatrix [col, rank + i-1] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank + i] = 5;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col, rank +i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; col + i <= 7 && (testmatrix [col + i, rank] > 6 || testmatrix [col + i, rank] == 0) && (i == 1 || testmatrix [col + i - 1, rank] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + i, rank] = 5;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; rank - i >= 0 && (testmatrix [col, rank - i] > 6 || testmatrix [col, rank - i] == 0) && (i == 1 || testmatrix [col, rank - i + 1] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank - i] = 5;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col, rank-i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; col - i >= 0 && (testmatrix [col - i, rank] > 6 || testmatrix [col - i, rank] == 0) && (i == 1 || testmatrix [col - i + 1, rank] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col-i, rank] = 5;
					if(kinginchecks(whitekingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank] != 0) {
						endder = true;
					}
				}
            }
			//black queen legal moves
			else if (num == 11 && whitesTurn == false) {
				//diagonal moves
				endder = false;
				for (int i = 1;col + i <= 7 && rank + i <= 7 && testmatrix [col + i, rank + i] == 0 && (i == 1 || testmatrix [col + i - 1, rank + i - 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col + i, rank + i] = 11;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank + i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col - i >= 0 && rank + i <= 7 && testmatrix[col - i, rank + i] == 0 && (i == 1 || testmatrix [col - i + 1, rank + i - 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col - i, rank + i] = 11;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank + i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col + i <= 7 && rank - i >= 0 && testmatrix[col + i, rank - i] < 6 && (i == 1 || testmatrix [col + i - 1, rank - i + 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col + i, rank - i] = 11;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank - i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1;col - i >= 0 && rank - i >= 0 && testmatrix[col - i, rank - i] < 6 && (i == 1 || testmatrix [col - i + 1, rank - i + 1] == 0); i++) {
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [col, rank] = 0;
					editedarray [col - i, rank - i] = 11;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank - i] != 0) {
						endder = true;
					}
				}
				//horizontal and vertical moves
				endder = false;
				for (int i = 1; rank + i <= 7 && testmatrix[col, rank + i] < 6 && (i == 1 || testmatrix [col, rank + i - 1] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank + i] = 11;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col, rank + i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; col + i <= 7 && testmatrix[col + i, rank] < 6 && (i == 1 || testmatrix [col + i - 1, rank] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + i, rank] = 11;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col + i, rank] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; rank - i >= 0 && testmatrix[col, rank - i] < 6 && (i == 1 || testmatrix [col, rank - i + 1] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank - i] = 11;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col, rank - i] != 0) {
						endder = true;
					}
				}
				endder = false;
				for (int i = 1; col - i >= 0 && testmatrix[col - i, rank] < 6 && (i == 1 || testmatrix [col - i + 1, rank] == 0);i++) {
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col-i, rank] = 11;
					if(kinginchecks(blackkingpos,editedarray) == false) { legalMoves.Add(editedarray); }
					if (testmatrix [col - i, rank] != 0) {
						endder = true;
					}
				}
			}
            else if (num == 6 && whitesTurn == true) //white king legal moves
            {
				if(col + 1 <= 7 && (testmatrix[col+1, rank] > 6 || testmatrix[col + 1, rank] == 0)) 
                    {
                    editedarray = (int[,])testmatrix.Clone();
                    editedarray[col, rank] = 0;
					editedarray[col + 1, rank] = 6;
					if(kinginchecks(new int[] {col+1, rank},editedarray) == false) { legalMoves.Add(editedarray); }
                }
				if(col - 1 >= 0 && (testmatrix[col - 1, rank] > 6 || testmatrix[col - 1, rank] == 0)) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col - 1, rank] = 6;
					if(kinginchecks(new int[] {col-1, rank},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(rank + 1 <= 7 && (testmatrix[col, rank + 1] > 6 || testmatrix[col, rank +1] == 0)) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank + 1] = 6;
					if(kinginchecks(new int[] {col, rank+1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(rank - 1 >= 0 && (testmatrix[col, rank - 1] > 6 || testmatrix[col, rank - 1] == 0)) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank - 1] = 6;
					if(kinginchecks(new int[] {col, rank-1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(col + 1 <= 7 && rank - 1 >= 0 && (testmatrix[col+1, rank - 1] > 6 || testmatrix[col + 1, rank - 1] == 0)) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + 1, rank - 1] = 6;
					if(kinginchecks(new int[] {col+1, rank-1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(col - 1 >= 0 && rank - 1 >= 0 && (testmatrix[col - 1, rank - 1] > 6 || testmatrix[col - 1, rank - 1] == 0)) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col - 1, rank - 1] = 6;
					if(kinginchecks(new int[] {col-1, rank-1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(col + 1 <= 7 && rank + 1 <= 7 && (testmatrix[col + 1, rank + 1] > 6 || testmatrix[col + 1, rank + 1] == 0)) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + 1, rank + 1] = 6;
					if(kinginchecks(new int[] {col+1, rank+1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(col - 1 >= 0 && rank + 1 <= 7 && (testmatrix[col - 1, rank + 1] > 6 || testmatrix[col - 1, rank + 1] == 0)) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col - 1, rank + 1] = 6;
					if(kinginchecks(new int[] {col-1, rank+1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				//castling
				//print("testmatrix [4,0]: "+ testmatrix [4, 0]);
				//print("testmatrix [5,0]: "+ testmatrix [5, 0]);
				//print("testmatrix [6,0]: "+ testmatrix [6, 0]);
				//print("testmatrix [7,0]: "+ testmatrix [7, 0]);
				if (whitekingmoved == false && movedwikingrook == false && testmatrix [5, 0] == 0 && checkChessBoardArray [5,0] != 2 && checkChessBoardArray[5,0] != 3 && checkChessBoardArray [6,0] != 2 && checkChessBoardArray[6,0] != 3 && testmatrix [6, 0] == 0 && testmatrix [7, 0] == 4) {
					if(checkChessBoardArray[whitekingpos[0],whitekingpos[1]] == 2 || checkChessBoardArray[whitekingpos[0],whitekingpos[1]] == 3)
					{
					//	whitekingincheck = true;
					}
					else
					{
						whitekingincheck = false;
					}
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [4, 0] = 0;
					editedarray [5, 0] = 4;
					editedarray [6, 0] = 6;
					editedarray [7, 0] = 0;
					//print ("can castle king side");
					legalMoves.Add (editedarray);
					
				//	print ("indieodak:");
				//	printArray (editedarray);
				}
				if (whitekingmoved == false && movedwqueenrook == false&& testmatrix [3, 0] == 0 && checkChessBoardArray [3,0] != 2 && checkChessBoardArray [3,0] != 3 && checkChessBoardArray [2,0] != 2 && checkChessBoardArray [3,0] !=3 && testmatrix [2, 0] == 0 && testmatrix[1,0] == 0 && testmatrix [0, 0] == 4) {
					if(checkChessBoardArray[whitekingpos[0],whitekingpos[1]] == 2 || checkChessBoardArray[whitekingpos[0],whitekingpos[1]] == 3)
					{
				//		whitekingincheck = true;
					}
					else
					{
						whitekingincheck = false;
					}
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [4, 0] = 0;
					editedarray [3, 0] = 4;
					editedarray [2, 0] = 6;
					editedarray [1, 0] = 0;
					editedarray [0, 0] = 0;
				//	print ("can castle queen side");
					legalMoves.Add(editedarray);
				//	printArray (editedarray);
				}
            }
			else if (num == 12 && whitesTurn == false) //black king legal moves
			{
				if(col + 1 <= 7 && testmatrix[col + 1, rank] < 6) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + 1, rank] = 12;
					if(kinginchecks(new int[] {col+1, rank},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(col - 1 >= 0 && testmatrix[col - 1, rank] < 6) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col - 1, rank] = 12;
					if(kinginchecks(new int[] {col-1, rank},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(rank + 1 <= 7 && testmatrix[col, rank + 1] < 6) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank + 1] = 12;
					if(kinginchecks(new int[] {col, rank+1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(rank - 1 >= 0 && testmatrix[col, rank - 1] < 6) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col, rank - 1] = 12;
					if(kinginchecks(new int[] {col, rank-1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(col + 1 <= 7 && rank - 1 >= 0 && testmatrix[col + 1, rank - 1] < 6) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + 1, rank - 1] = 12;
					if(kinginchecks(new int[] {col+1, rank-1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(col - 1 >= 0 && rank - 1 >= 0 && testmatrix[col - 1, rank - 1] < 6) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col - 1, rank - 1] = 12;
					if(kinginchecks(new int[] {col-1, rank-1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(col + 1 <= 7 && rank + 1 <= 7 && testmatrix[col + 1, rank + 1] < 6) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col + 1, rank + 1] = 12;
					if(kinginchecks(new int[] {col+1, rank+1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				if(col - 1 >= 0 && rank + 1 <= 7 && testmatrix[col - 1, rank + 1] < 6) 
				{
					editedarray = (int[,])testmatrix.Clone();
					editedarray[col, rank] = 0;
					editedarray[col - 1, rank + 1] = 12;
					if(kinginchecks(new int[] {col-1, rank+1},editedarray) == false) { legalMoves.Add(editedarray); }
				}
				//castling
			//	print("testmatrix [4,7]: "+ testmatrix [4, 7]);
			//	print("testmatrix [5,7]: "+ testmatrix [5, 7]);
			//	print("testmatrix [6,7]: "+ testmatrix [6, 7]);
			//	print("testmatrix [7,7]: "+ testmatrix [7, 7]);
				if (blackkingmoved == false && movedbkingrook == false && testmatrix [5, 7] == 0 && checkChessBoardArray [5,7] != 1 && checkChessBoardArray [5,7] != 3 && checkChessBoardArray[6,7] != 1 && checkChessBoardArray [6,7] != 3 && testmatrix [6, 7] == 0 && testmatrix [7, 7] == 10) {
					if(checkChessBoardArray[blackkingpos[0],blackkingpos[1]] == 1 || checkChessBoardArray[blackkingpos[0],blackkingpos[1]] == 3)
					{
					//	blackkingincheck = true;
					}
					else
					{
					//	blackkingincheck = false;
					}
				//	print ("black castle is legal");
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [4, 7] = 0;
					editedarray [5, 7] = 10;
					editedarray [6, 7] = 12;
					editedarray [7, 7] = 0;
					legalMoves.Add(editedarray);
			//		printArray (editedarray);
				}
				if (blackkingmoved == false && movedbqueenrook == false && testmatrix [3, 7] == 0 && checkChessBoardArray [3,7] != 1 && checkChessBoardArray [3,7] != 3 && checkChessBoardArray [2,7] != 1 && checkChessBoardArray [2,7] != 3 && testmatrix [2, 7] == 0 && testmatrix[1,7] == 0 && testmatrix [0, 7] == 10) {
					if(checkChessBoardArray[blackkingpos[0],blackkingpos[1]] == 1 || checkChessBoardArray[blackkingpos[0],blackkingpos[1]] == 3)
					{
					//	blackkingincheck = true;
					}
					else
					{
					//	blackkingincheck = false;
					}
					editedarray = (int[,])testmatrix.Clone ();
					editedarray [4, 7] = 0;
					editedarray [3, 7] = 10;
					editedarray [2, 7] = 12;
					editedarray [1, 7] = 0;
					editedarray [0, 7] = 0;
					legalMoves.Add(editedarray);
				//	printArray (editedarray);
				}
			}
		}
		if (legalMoves.Count == 1) {
		//	printArray (legalMoves[0]);
		}
		
	}
		

                //Runs on FIRST frame of mouse click.
	void OnMouseDown () {
		if((whitesTurn == true && _whitePlayerType == PlayerTypes.Human) || (whitesTurn == false && _blackPlayerType == PlayerTypes.Human))
		{
			seletedobjected = null;
			RaycastHit2D hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			hit = Physics2D.Raycast (ray.origin, ray.direction);
			if (hit)
			{
				//print out the name if the raycast hits something
				isDragging = true;
				seletedobjected = hit.transform;
				seletedobjected.localScale = selectionscalefactor;
				//Vector3 bringtofront = new Vector3 (seletedobjected.position.x, seletedobjected.position.y, 1);
				//seletedobjected.transform.position = bringtofront;
				originalLoc = seletedobjected.transform.position;
					float x;

					float y;

					//Vector3 p;

					x = seletedobjected.position.x;
					y = seletedobjected.position.y;
					x = x / ((1.12875f));
					y = y / ((1.12875f));

					//odd rounding for x
					if (Mathf.Round (Mathf.Abs (x)) % 2 != 0) {
						x = Mathf.Round (x);
					} else if (Mathf.Abs (x) - Mathf.Round (Mathf.Abs (x)) > 0) {
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
					x = x - 1;
					x = x / 2;
					x = x + 4;
					//odd rounding for y
					if (Mathf.Round (Mathf.Abs (y)) % 2 != 0) {
						y = Mathf.Round (y);
					} else if (Mathf.Abs (y) - Mathf.Round (Mathf.Abs (y)) > 0) {
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
					y = y - 1;
					y = y / 2;
					y = y + 4;
					originalLoccoord = new int[] {(int)x,(int)y};
				if (seletedobjected.GetComponent<SpriteRenderer> ().sprite == bpawnname || seletedobjected.GetComponent<SpriteRenderer> ().sprite == wpawnname) {
					if (seletedobjected.GetComponent<SpriteRenderer> ().sprite == wpawnname) {
						enpassant = new int[]{(int)x, (int)y + 1, 0 };
						entrack = new int[]{ (int)x, (int)y + 2,};
					} else if (seletedobjected.GetComponent<SpriteRenderer> ().sprite == bpawnname) {
						enpassant = new int[]{ (int)x, (int)y - 1, 1 };
						entrack = new int[]{ (int)x, (int)y - 2};
					}
				} else {
					enpassant = new int[]{ 0, 0, 0 };
					entrack = new int[]{ 0,0};
				}
			}
		}
	}

	//Runs on frames while mouse is pressed and moved
	void OnMouseDrag ()    {
		if (seletedobjected != null) {
		}
	}

	//Runs on LAST frame of mouse click
	void OnMouseUp () {
		if((whitesTurn == true && _whitePlayerType == PlayerTypes.Human) || (whitesTurn == false && _blackPlayerType == PlayerTypes.Human))
		{
			if (seletedobjected != null) {

				seletedobjected.localScale = Vector3.one;
				
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
					if((whitesTurn == true && _whitePlayerType == PlayerTypes.Human) || (whitesTurn == false && _blackPlayerType == PlayerTypes.Human))
					{
						seletedobjected.transform.position = originalLoc;
					}
					return;
				}
			//	print ("x is :" + x);
			//	print ("y is:" + y);
				//white kingside castle
				if (seletedobjected.GetComponent<SpriteRenderer> ().sprite == wkingname && whitekingincheck == false && whitekingmoved == false && movedwikingrook == false && ((x == 5 && y == -7) || (x== 7 && y==-7))) {
					//print ("inhereke");
					x = 3 * (1.12875f);
					y = -7 * (1.12875f);
					p.x = x;
					p.y = y;
					p.z = -1;
					wkingrook.transform.position = p;
					selectedrook = wkingrook;
					origionalrookLoc.x = 7 * (1.12875f);
					origionalrookLoc.y = -7 * (1.12875f);
					origionalrookLoc.z = -1;
					x = 5 * (1.12875f);
					y = -7 * (1.12875f);
					p.x = x;
					p.y = y;
					p.z = -1;
					//EditorApplication.isPaused = true;
				}
				//white queenside castle
				else if (seletedobjected.GetComponent<SpriteRenderer> ().sprite == wkingname && whitekingincheck == false && whitekingmoved == false && movedwqueenrook == false && ((x == -3 && y == -7) || (x== -7 && y==-7))) {
					//print ("inhereke");
					x = -1 * (1.12875f);
					y = -7 * (1.12875f);
					p.x = x;
					p.y = y;
					p.z = -1;
					wqueenrook.transform.position = p;
					selectedrook = wqueenrook;
					origionalrookLoc.x = -7 * (1.12875f);
					origionalrookLoc.y = -7 * (1.12875f);
					origionalrookLoc.z = -1;
					x = -3 * (1.12875f);
					y = -7 * (1.12875f);
					p.x = x;
					p.y = y;
					p.z = -1;
					//EditorApplication.isPaused = true;
				}
				//black kingside castle
				else if (seletedobjected.GetComponent<SpriteRenderer> ().sprite == bkingname && blackkingincheck == false && blackkingmoved == false && movedbkingrook == false && ((x == 5 && y == 7) || (x== 7 && y== 7))) {
			//		print ("inhereke");
					x = 3 * (1.12875f);
					y = 7 * (1.12875f);
					p.x = x;
					p.y = y;
					p.z = -1;
					bkingrook.transform.position = p;
					selectedrook = bkingrook;
					origionalrookLoc.x = 7 * (1.12875f);
					origionalrookLoc.y = 7 * (1.12875f);
					origionalrookLoc.z = -1;
					x = 5 * (1.12875f);
					y = 7 * (1.12875f);
					p.x = x;
					p.y = y;
					p.z = -1;
					//EditorApplication.isPaused = true;
				}
				//black queenside castle
				else if (seletedobjected.GetComponent<SpriteRenderer> ().sprite == bkingname && blackkingincheck == false && blackkingmoved == false && movedbqueenrook == false && ((x == -3 && y == 7)|| (x==-7 && y==7))) {
					//print ("inhereke");
					x = -1 * (1.12875f);
					y = 7 * (1.12875f);
					p.x = x;
					p.y = y;
					p.z = -1;
					bqueenrook.transform.position = p;
					selectedrook = bqueenrook;
					origionalrookLoc.x = -7 * (1.12875f);
					origionalrookLoc.y = 7 * (1.12875f);
					origionalrookLoc.z = -1;
					x = -3 * (1.12875f);
					y = 7 * (1.12875f);
					p.x = x;
					p.y = y;
					p.z = -1;
					//EditorApplication.isPaused = true;
				}
				// capture
				else if (chessboardarray[(int)(((x - 1) / 2))+4, (int)(((y - 1) / 2)+4)] !=0)
				{
					wanttocapture = new int[] {(int)(((x - 1) / 2)+4), (int)(((y - 1) / 2)+4),gametocodearray[(int)(((x - 1) / 2)+4), (int)(((y - 1) / 2)+4)]-32,chessboardarray[originalLoccoord[0],originalLoccoord[1]],gametocodearray[originalLoccoord[0],originalLoccoord[1]]}; //xcoord,ycoord,goal take id, selection piece type, selection piece id
				//	print(gametocodearray[originalLoccoord[0],originalLoccoord[0]]-32);
					if (entrack [0] != (int)(((x - 1) / 2) + 4) || entrack [1] != (int)(((y - 1) / 2) + 4)) {
						enpassant = new int[]{ 0, 0, 0 };
						entrack = new int[] { 0, 0 };

					}
					x = x * (1.12875f);
					y = y * (1.12875f);
					p.x = x;
					p.y = y;
					p.z = -1;
				}
				else {
					if (entrack [0] != (int)(((x - 1) / 2) + 4) || entrack [1] != (int)(((y - 1) / 2) + 4)) {
						enpassant = new int[]{ 0, 0, 0 };
						entrack = new int[] { 0, 0 };

					}
					x = x * (1.12875f);
					y = y * (1.12875f);
					p.x = x;
					p.y = y;
					p.z = -1;
				}
				seletedobjected.position = p;

				GenerateMatrix ();
				if(checkChessBoardArray[whitekingpos[0],whitekingpos[1]] == 2 || checkChessBoardArray[whitekingpos[0],whitekingpos[1]] == 3)
				{
				//	whitekingincheck = true;
				}
				else
				{
				//	whitekingincheck = false;
				}
				if(checkChessBoardArray[blackkingpos[0],blackkingpos[1]] == 1 || checkChessBoardArray[blackkingpos[0],blackkingpos[1]] == 3)
				{
				//	blackkingincheck = true;

				}
				else
				{
			//		blackkingincheck = false;
				}
			}
			isLegal (chessboardarray);
		}
	}

	public void Interpreter (int[,] prevcb, int[,] newcb, bool usepgn)
	{
		int[,]deltacb = new int[,] { /*A File*/ {0,0,0,0,0,0,0,0}, /*B File*/ {0,0,0,0,0,0,0,0}, /*C File*/ {0,0,0,0,0,0,0,0}, /*D File*/ {0,0,0,0,0,0,0,0}, /*E File*/ {0,0,0,0,0,0,0,0}, /*F File*/ {0,0,0,0,0,0,0,0}, /*G File*/ {0,0,0,0,0,0,0,0}, /*H File*/ {0,0,0,0,0,0,0,0}};
		int col = 0;
		int rank = 0;
		List<int> storedcols = new List<int>{};
		List<int> storedranks = new List<int> {};
		int selcol = -1;
		int selrank = -1;
		int destcol = -1;
		int destrank = -1;
		int countertest = 0;
		int dx = -1;
		int dy = -1;
		int epx = -1;
		int epy = -1;
		lastpgnline = "";
		foreach(int num in prevcb)
		{
			countertest++;	
			if ((int)prevcb[col,rank] != (int)newcb[col,rank])
			{
				if(prevcb[col,rank] == 99)
				{
					if(newcb[col, rank] == 7)
					{
						storedcols.Add(col);
					    storedranks.Add(rank);
					}
					else
					{

					}
				}
				else if(prevcb[col,rank] == 98)
				{
					if(newcb[col, rank] == 1)
					{
						storedcols.Add(col);
						storedranks.Add(rank);
					}
					else
					{
						
					}
				}
				else if (newcb[col,rank] == 99)
				{
					
				}
				else if (newcb[col,rank] == 98)
				{
					
				}
				else if(newcb[col,rank] == 0)
				{
					selcol = col;
					selrank = rank;
			//		print("intuit");
				}
				else
				{
					deltacb[col,rank] = newcb[col,rank];
					storedcols.Add(col);
					storedranks.Add(rank);
			//		print("destination is: " + col + ", " + rank);
				}
				//print("here");
				//System.print();
			}
			else
			{
				
			}
			// if length = 2 then normal move
			// if length = 3 then enpassant
			// if length = 4 then castle
			if (col <= 7) {
				if (rank < 7) {
					rank++;
				} else {
					col++;
					
					rank = 0;
				}
			}
		}
		//print("countertest is: " + countertest);
		//printArray(prevcb);
		//print("____________________________");
		//print("============================");
		//printArray(newcb);
		if(storedcols.Count == 1 || storedcols.Count == 2)
		{
			
			destcol = storedcols[0];
			destrank = storedranks[0];
			
			
		}
		else if(storedcols.Count == 3 ||storedcols.Count == 4)
		{
			print("supposed to castle");
		}
		if(destcol != -1 && destrank != -1 && prevcb[destcol,destrank] != 6 & prevcb[destcol,destrank] != 12 && prevcb[destcol, destrank] != 0 && prevcb[destcol,destrank] != 99 && newcb[destcol,destrank] != 99 && newcb[destcol,destrank] != 98 && prevcb[destcol,destrank] != 98)
		{
			dx = destcol;
			dy = destrank;
			//print("capture!");
		}
		if(usepgn == false && destrank == 7 && prevcb[selcol,selrank] == 1)
		{
			//print("im in here for white");
			mwqueen = true;
		}
		else if(usepgn == false && destrank == 0 && prevcb[selcol,selrank] == 7)
		{
			//print("im in here for black");
			mbqueen = true;
		}
		else
		{
			if (usepgn == false)
			{
				mwqueen = false;
				mbqueen = false;
			}
		}
		if(usepgn == false && (prevcb[destcol,destrank] == 6 || prevcb[destcol,destrank] == 12))
		{
			print("Trying to capture king stop it, d is : "+ dx + ", " + dy + ", sel is: " + selcol + ", " + selrank);
			Debug.Break();
		}
		if(usepgn == false)
		{
			ArtificialMove(selcol,selrank, destcol, destrank,dx, dy, epx, epy);
		}
		else
		{
			if (prevcb[selcol, selrank] == 1 || prevcb[selcol, selrank] == 7)
			{
				movedpiece = "";
			}
			else if (prevcb[selcol, selrank] == 2 || prevcb[selcol, selrank] == 8)
			{
				movedpiece = "N";
			}
			else if (prevcb[selcol, selrank] == 3 || prevcb[selcol, selrank] == 9)
			{
				movedpiece = "B";
			}
			else if (prevcb[selcol, selrank] == 4 || prevcb[selcol, selrank] == 10)
			{
				movedpiece = "R";
			}
			else if (prevcb[selcol, selrank] == 5 || prevcb[selcol, selrank] == 11)
			{
				movedpiece = "Q";
			}
			else if (prevcb[selcol, selrank] == 6 || prevcb[selcol, selrank] == 12)
			{
				movedpiece = "K";
			}
			//if(whitesTurn == true && lastpgnline == "")
			//{
				//lastpgnline = lastpgnline + ((moveCount-1)/2 + 1) + ". ";
			//}
			lastpgnline = lastpgnline + movedpiece + (char)(destcol + 97) + (destrank + 1);
			if(whitesTurn == true)
			{
				lastpgnline = lastpgnline + " ";
			}
			else if(whitesTurn == false)
			{
				lastpgnline = lastpgnline + "\n" + ((moveCount/2) + 1) + ". ";
			}
		}
		
	}

	public void ArtificialMove (int x1, int y1, int x2, int y2,int dx, int dy, int epx, int epy)
	{
		seletedobjected = null;
		GameObject tempgo;
		float nx1 = ((x1*2)-7) * (1.12875f);
		float nx2 = ((x2*2)-7) * (1.12875f);
		float ny1 = ((y1*2)-7) * (1.12875f);
		float ny2 = ((y2*2)-7) * (1.12875f);
		float ndx = ((dx*2)-7) * (1.12875f);
		float ndy = ((dy*2)-7) * (1.12875f);
	//	originalLoc = new Vector2 (((x1*2)-7) * (1.12875f), ((y1*2)-7) * (1.12875f));
	//	print(nx1 + ",x1 is: "+ x1 + ", " + ny1 + ", " + ", y1 is: " + y1 + ", " + nx2 + ", x2 is: " + x2 + ", " + ny2);
		Vector3 p = new Vector3 (nx2,ny2,-1);

		if(dx != -1)
		{
			RaycastHit2D hitdel = Physics2D.Raycast (new Vector2 (ndx,ndy), -Vector2.up);
			if(hitdel == null || hitdel.transform == null)
			{
				print("raycast hit nothing dx" + "dx = " + dx + ", dy = " + dy);
				Debug.Break();
			}
			else{
				hitdel.transform.gameObject.SetActive(false);
				//print("Devated");
				
			}
		}
	//	RaycastHit2D hit;
	//	Ray ray = Camera.main.ScreenPointToRay(new Vector2 (nx1,ny1));
		RaycastHit2D hit = Physics2D.Raycast (new Vector2 (nx1,ny1), -Vector2.up) ;
		if(hit == null || hit.transform == null)
		{
			print("raycast hit nothing temp");
			Debug.Break();
		}
		tempgo = hit.transform.gameObject;
		//print(tempgo.name);
		tempgo.transform.position = p;
		if(mwqueen == true && tempgo.GetComponent<SpriteRenderer>().sprite == wpawnname)
		{
			tempgo.GetComponent<SpriteRenderer>().sprite = wqueenname;
			print("White Dest rank is: " + y2);
		}
		else if (mbqueen == true && tempgo.GetComponent<SpriteRenderer>().sprite == bpawnname)
		{
			tempgo.GetComponent<SpriteRenderer>().sprite = bqueenname;
			print("Black Dest rank is: " + y2);
		}
		isLeegal = true;
		//seletedobjected = tempgo.transform;
		//if(destcol == 7 && seletedobjected.GetComponent<)
		//seletedobjected = tempgo.transform;
		if(epx != -1)
		{
			float nex = ((epx*2)-7) * (1.12875f);
			float ney = ((epy*2)-7) * (1.12875f);
		//	print(nx1 + ",x1 is: "+ x1 + ", " + ny1 + ", " + ", y1 is: " + y1 + ", " + nx2 + ", x2 is: " + x2 + ", " + ny2);
			p = new Vector3 (nex,ney,-1);
		//	RaycastHit2D hit;
		//	Ray ray = Camera.main.ScreenPointToRay(new Vector2 (nx1,ny1));
			RaycastHit2D hitite = Physics2D.Raycast (new Vector2 (nex,ney), -Vector2.up) ;
			if(hitite == null || hitite.transform == null)
			{
				print("raycast hit nothing ep");
				Debug.Break();
			}
			tempgo = hitite.transform.gameObject;
			//print(tempgo.name);
			tempgo.SetActive(false);
			//seletedobjected = tempgo.transform;
		}
		moveCount ++;
		checkupcall = false;
		seletedobjected = null;
	}

	public void SetWPlayerType(int nnum)
	{
		_whitePlayerType = (PlayerTypes)nnum;
	}
	public void SetBPlayerType(int nnum)
	{
		_blackPlayerType = (PlayerTypes)nnum;
	}

	 IEnumerator DelayProcess(float delay)
    {
		ffcheck = true;
        //This is a coroutine
		fcheckupcall = false;
         yield return new WaitForSeconds(delay);    //Wait one frame
		fcheckupcall = true;
    }

	
	void UpdatePGN (int[,] prevcb, int [,] newcb)
	{
		string path = Application.dataPath +"/PGNDATA/PGN.txt";
		//Create File if it doesn't exist
//		if (!File.Exists(path))
//		{
		if(moveCount == 1)
		{
			File.WriteAllText(path, "Login log \n" + System.DateTime.Now + "\n");
		}
//		}
//		else
//		{
//
//		//}
		//Context of the file
		Interpreter(prevcb,newcb,true);
		//string content = "Login date: " + System.DateTime.Now + "\n";
		string content = lastpgnline;
		//Add some to text to it
		if(moveCount == 1)
		{
			File.AppendAllText(path, "1.");
		}
		
		File.AppendAllText(path, content);
	}
}