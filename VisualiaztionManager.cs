using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualiaztionManager : MonoBehaviour {

	[System.Serializable]
	public class Stage
	{
		public string _stageName;
		public int _stageID;
		public GameObject[] _overridedisableGOs;
	}

	[System.Serializable]
	public class TraversalObject
	{
		public string _objectName;
		public int[] _stageIDs;
		public GameObject[] _traversalobjectGOs;
	}

	public int _currentStageID;

	public Stage[] _stages;

	public TraversalObject[] TraversalObjects;

	public int bplayertype;

	public int wplayertype;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Stage stage in _stages)
		{
			if(stage._stageID != _currentStageID)
			{
				foreach(GameObject _overridedisableGO in stage._overridedisableGOs)
				{
					_overridedisableGO.SetActive(false);
				}
			}
			else
			{
				foreach(GameObject _overridedisableGO in stage._overridedisableGOs)
				{
					_overridedisableGO.SetActive(true);
				}
			}
		}
		foreach (TraversalObject traveralobject in TraversalObjects)
		{
			if(Array.Exists(traveralobject._stageIDs, element => element == _currentStageID))
			{
				foreach(GameObject _traversalobjectGO in traveralobject._traversalobjectGOs)
				{
					_traversalobjectGO.SetActive(true);
				}
			}
			else
			{
				foreach(GameObject _traversalobjectGO in traveralobject._traversalobjectGOs)
				{
					_traversalobjectGO.SetActive(false);
				}
			}
		}
	}

	public void _goToStage (int _newStageID)
	{
		_currentStageID = _newStageID;
	}
}
