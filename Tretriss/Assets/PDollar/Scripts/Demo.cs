using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;
using UnityEngine.UI;
using Random = System.Random;

public class Demo : MonoBehaviour {

	public Transform gestureOnScreenPrefab;

	private List<Gesture> trainingSet = new List<Gesture>();

	private List<Point> points = new List<Point>();
	private int strokeId = -1;

	private Vector3 virtualKeyPosition = Vector2.zero;
	private Rect drawArea;

	private RuntimePlatform platform;
	private int vertexCount = 0;

	private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
	private LineRenderer currentGestureLineRenderer;

	//GUI
	private string message;
	private bool recognized;
	private string newGestureName = "";
	
	//GUI in game
	public Text timer;
	public Text formes;
	private float maxtime = 5;
	private float startTime;
	
	//Detection du geste

	private string[] formesADeviner = {"etoile","cercle","zigzag"};
	private string word;
	private bool activate = false;
	
	//lancement du scipt toutes les 30sec
	private float looptime = 30f;
	private float trueStartTime;
	
	void Start () {

		platform = Application.platform;
		drawArea = new Rect(0, 0, Screen.width, Screen.height);

		//timer = new Rect(Screen.width / 3, Screen.height / 5, Screen.width / 3, Screen.height / 10);
		
		/*
		//Load pre-made gestures
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
		foreach (TextAsset gestureXml in gesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
		*/
		TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GesturePerso/");
		foreach (TextAsset gestureXml in gesturesXml)
			trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
		
		/*
		//Load user custom gestures
		string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
		foreach (string filePath in filePaths)
			trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
		*/
		
		Initialise();

	}

	void Update ()
	{
		if (activate)
		{
			timer.text = string.Format("{0:N2}",(maxtime - (Time.time - startTime)));
			if ((maxtime - (Time.time - startTime)) <= 0)
			{
				recognized = true;
				startTime = Time.time;
				
				Gesture candidate = new Gesture(points.ToArray());
				Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
				
				//message = gestureResult.GestureClass + " " + gestureResult.Score;
				Debug.Log(gestureResult.GestureClass);
				if (gestureResult.GestureClass == word)
				{
					Score.fallTime = Score.fallTime*2;
					Debug.Log("Reussi : "+String.Format("{0:N3}",Score.fallTime));
				}
				else
				{
					Score.fallTime = Score.fallTime/4;
					Debug.Log("Echec : "+String.Format("{0:N3}",Score.fallTime));
				}

				Deactivate();
			}
			
			
			
			if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer) {
				if (Input.touchCount > 0) {
					virtualKeyPosition = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
				}
			} else {
				if (Input.GetMouseButton(0)) {
					virtualKeyPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
				}
			}

			if (drawArea.Contains(virtualKeyPosition)) {

				if (Input.GetMouseButtonDown(0)) {

					if (recognized) {

						recognized = false;
						strokeId = -1;

						points.Clear();

						foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

							lineRenderer.SetVertexCount(0);
							Destroy(lineRenderer.gameObject);
						}

						gestureLinesRenderer.Clear();
					}

					++strokeId;
					
					Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
					tmpGesture.transform.GetChild(0).gameObject.SetActive(false);
					currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
					
					gestureLinesRenderer.Add(currentGestureLineRenderer);
					
					vertexCount = 0;
				}
				
				if (Input.GetMouseButton(0)) {
					points.Add(new Point(virtualKeyPosition.x, -virtualKeyPosition.y, strokeId));

					currentGestureLineRenderer.SetVertexCount(++vertexCount);
					currentGestureLineRenderer.SetPosition(vertexCount - 1, Camera.main.ScreenToWorldPoint(new Vector3(virtualKeyPosition.x, virtualKeyPosition.y, 10)));
				}
			}
		}
		else
		{
			if (looptime - (Time.time - trueStartTime) <= 0)
			{
				Initialise();
			}
		}
	}

	void OnGUI() {

		GUI.Box(drawArea, "Draw Area");
		//GUI.Box(timer,"Temps restant :");
		//GUI.Label(new Rect(10, Screen.height - 40, 500, 50), message);

		/*
		if (GUI.Button(new Rect(Screen.width - 100, 10, 100, 30), "Recognize")) {

			recognized = true;

			Gesture candidate = new Gesture(points.ToArray());
			Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
			
			message = gestureResult.GestureClass + " " + gestureResult.Score;
		}

		GUI.Label(new Rect(Screen.width - 200, 150, 70, 30), "Add as: ");
		newGestureName = GUI.TextField(new Rect(Screen.width - 150, 150, 100, 30), newGestureName);

		if (GUI.Button(new Rect(Screen.width - 50, 150, 50, 30), "Add") && points.Count > 0 && newGestureName != "") {

			string fileName = String.Format("{0}/{1}-{2}.xml", "C:/Users/delec/unity_workspace/tretrissbis/Tretriss/Assets/PDollar/Resources/GesturePerso", newGestureName, DateTime.Now.ToFileTime());

			#if !UNITY_WEBPLAYER
				GestureIO.WriteGesture(points.ToArray(), newGestureName, fileName);
			#endif

			trainingSet.Add(new Gesture(points.ToArray(), newGestureName));
			
			newGestureName = "";
		}
		*/
	}

	public void Initialise()
	{
		startTime = Time.time;

		Random r = new Random();
		word = formesADeviner[r.Next(0, formesADeviner.Length)];
		formes.text = word;

		activate = true;
		Debug.Log(activate);
		gestureOnScreenPrefab.gameObject.SetActive(true);

		trueStartTime = Time.time;
	}

	public void Deactivate()
	{
		gestureOnScreenPrefab.gameObject.SetActive(false);
		activate = false;
				
		recognized = false;
		strokeId = -1;

		points.Clear();

		foreach (LineRenderer lineRenderer in gestureLinesRenderer) {

			lineRenderer.SetVertexCount(0);
			Destroy(lineRenderer.gameObject);
		}

		gestureLinesRenderer.Clear();
		trueStartTime = Time.time;
	}
}
