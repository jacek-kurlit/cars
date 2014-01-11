using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class FileManager {

	public static void saveNewRekord(float time,int lap){
		StreamWriter sw = new StreamWriter (Application.dataPath + "/KrillData/rekords.txt", true);
		sw.WriteLine ("Nowy rekord toru wynosi: " + time + " dla okrążenia " + lap);
		sw.Close ();

		sw = new StreamWriter (Application.dataPath + "/KrillData/rekord.txt", false);
		sw.WriteLine ("" + time);
		sw.Close ();
	}

	public static void saveNewSectorTime(int sectorId,string text){
		StreamWriter sw = new StreamWriter (Application.dataPath + "/KrillData/sector" + sectorId + ".txt", true);
		sw.WriteLine (text);
		sw.Close();
	}

	public static string loadPointsFile(string fileName){
		StreamReader input = new StreamReader(fileName);
		string output =  input.ReadToEnd();
		input.Close();

		return output;
	}

	public static void saveNewSectorPoints(int sectorId, float newSectorTime, List<Vector3> points){
		StreamWriter sw = new StreamWriter (Application.dataPath + "/KrillData/wayPoints" + sectorId + ".txt", false);
		sw.WriteLine (newSectorTime.ToString());
		StringBuilder allPoints = new StringBuilder();

		foreach(Vector3 vector in points){
			allPoints.Append (vector.x + "," + vector.y + "," + vector.z + ";");
		}
		sw.WriteLine(allPoints.ToString());
		sw.Close();
	}

	public static void saveInitialTrace(){
		Transform[] vectors = GameObject.FindGameObjectWithTag("InitialTrace").GetComponentsInChildren<Transform>();
		StreamWriter sw = new StreamWriter (Application.dataPath + "/KrillData/wayPoints.txt", false);
		
		for(int i = 1; i < vectors.Length; i++){
			Vector3 vector = vectors[i].position;
			sw.WriteLine(vector.x + "," + vector.y + "," + vector.z + ";");
		}

		sw.Close();
	}

	public static float loadBestLapTime(){
		StreamReader sr = new StreamReader (Application.dataPath + "/KrillData/rekord.txt");
		float bestTime = float.Parse(sr.ReadLine());
		sr.Close();

		return bestTime;
	}

	public static void saveLap(string carId,int lap, float time){
		StreamWriter sw = new StreamWriter (Application.dataPath + "/KrillData/" + carId + ".txt", true);
		sw.WriteLine(lap + "," + time);
		sw.Close();
	}
}
