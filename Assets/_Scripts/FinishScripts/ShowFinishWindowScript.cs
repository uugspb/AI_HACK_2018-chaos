

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowFinishWindowScript : MonoBehaviour {

	public Canvas finishCanvas;
	//	public GameObject showEarnPoints;


	private static ShowFinishWindowScript instance;
	public static ShowFinishWindowScript Instance 
	{
		get {
			if (instance == null) 
			{
				instance = GameObject.FindObjectOfType<ShowFinishWindowScript> ();
			}
			return ShowFinishWindowScript.instance; }
	}

	private int startPoints;
	private int totalPoints;

	// для увеличения очков
	struct Record
	{
		public float Points;//Наши Points
		public float NextWaitTimer;//Ожидание перед следующим прибавлением Points
		public float InnerTimer;//Время "красивой анимации с циферками"
		public int infoIndex; // по этоту индексу определяется, какая информация из функции будет показана
	}


	public Text TestLog;
	float FullPoints;//Общее количество Points
	List <Record> records = new List<Record>(); // для Points

	private void SetStartParams()
	{
		startPoints = PlayerPrefs.GetInt ("StartPoints");
		totalPoints = PlayerPrefs.GetInt ("Points");

		//		winnerCanvas.transform.GetChild (0).GetChild (1).GetChild (0).GetComponent<Text> ().text = 
		//			startPoints.ToString(); 
		//		winnerCanvas.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Text> ().text = 
		//			totalPoints.ToString();

		finishCanvas.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Slider>().wholeNumbers = 
			true;
		finishCanvas.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Slider> ().minValue =
			startPoints;
		finishCanvas.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Slider>().maxValue = 
			totalPoints;
		finishCanvas.transform.GetChild (0).GetChild (0).GetChild (1).GetComponent<Text> ().text = 
			startPoints.ToString();
	}

	// выбрать, какую информацию показывать
	void ShowChangedInfo(int whatInfoNeed)
	{
		if (whatInfoNeed == 0) // оставить пустой
		{
			TestLog.text = "";
		}
		else if (whatInfoNeed == 1) // "уровень пройден. n-баллов"
		{
			ShowLevelPoints ();
		}
		else if (whatInfoNeed == 2) // "хард-версия уровеня пройдена. n-баллов"
		{
			ShowLevelPointsHardcore ();
		}
		else if (whatInfoNeed == 3) // "найдено в посылках. n-баллов"
		{
			ShowParcelPoints ();
		}

	}

	private void ShowLevelPoints()
	{
		TestLog.text = "Пройден уровень";

	}

	private void ShowLevelPointsHardcore()
	{
		TestLog.text = "Хард версия уровня";
	}

	private void ShowParcelPoints()
	{
		TestLog.text = "Найдено в посылках: " + (PlayerPrefs.GetInt ("TempParcelBonus")).ToString();
	}



	// Use this for initialization
	void Start () {

		SetStartParams ();
		TestLog.text = "";

		GetRecords();
		if(records.Count>0) StartCoroutine(RecordsInit());

	}


	void GetRecords()//каким то образом получаем все свои числа со временем ожидания для каждого
	{
		records.Add (new Record { Points = startPoints, InnerTimer = 2f, NextWaitTimer = 2f, infoIndex = 0 });
		records.Add (new Record { Points = PlayerPrefs.GetInt ("LevelAward"), InnerTimer = 2f, NextWaitTimer = 2f, infoIndex = 1 });


	}

	IEnumerator RecordsInit()
	{
		YieldInstruction yi = new WaitForEndOfFrame();
		Record LastRecord = records[0];
		FullPoints = LastRecord.Points;
		//		TestLog.text = ((int)FullPoints).ToString();

		finishCanvas.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Slider>().value = 
			(int)FullPoints;
		finishCanvas.transform.GetChild (0).GetChild (0).GetChild (1).GetComponent<Text> ().text = 
			((int)FullPoints).ToString();

		yield return new WaitForSeconds(LastRecord.NextWaitTimer);
		for (int i=1;i< records.Count;i++)
		{

			ShowChangedInfo (records[i].infoIndex);

			LastRecord = records[i];
			float cur = 0f;
			if (LastRecord.InnerTimer > 0f) {
				while (cur< LastRecord.Points)
				{
					cur += LastRecord.Points * (Time.deltaTime / LastRecord.InnerTimer);//Time.unscaledDeltaTime
					//					TestLog.text = ((int)(FullPoints + cur)).ToString();

					finishCanvas.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Slider>().value = 
						(int)(FullPoints + cur);
					finishCanvas.transform.GetChild (0).GetChild (0).GetChild (1).GetComponent<Text> ().text = 
						((int)(FullPoints + cur)).ToString();

					yield return yi;
				}
			}
			FullPoints += LastRecord.Points;
			//			TestLog.text = ((int)FullPoints).ToString();

			finishCanvas.transform.GetChild (0).GetChild (0).GetChild (0).GetComponent<Slider>().value = 
				(int)FullPoints;
			finishCanvas.transform.GetChild (0).GetChild (0).GetChild (1).GetComponent<Text> ().text = 
				((int)FullPoints).ToString();

			yield return new WaitForSeconds(LastRecord.NextWaitTimer);
		}
	}




	// Update is called once per frame
	void Update () {

	}
}

