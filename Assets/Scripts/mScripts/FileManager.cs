using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class FileManager {

	public static void WriteTimeToFile(int krillID, string text) {
		//StreamWriter sw = System.IO.File.AppendText (Application.dataPath + "/KrillData/krill.txt", true);
		StreamWriter sw = new StreamWriter (Application.dataPath + "/KrillData/krill" + krillID + ".txt", true);
		sw.WriteLine (text);
		sw.Close ();
	}

	public static void WriteParametersToFile(int krillID, int sectorID, float[] parameters) {
		StreamWriter sw = new StreamWriter (Application.dataPath + "/KrillData/krillParams" + krillID + ".txt", true);
		sw.WriteLine ("SECTOR " + sectorID);
		sw.WriteLine ("steerTime = " + parameters [0]);
		sw.WriteLine ("maxSpeed = " + parameters [1]);
		sw.Close ();
	}

	public static void saveNewRekord(string text){
		StreamWriter sw = new StreamWriter (Application.dataPath + "/KrillData/rekords.txt", true);
		sw.WriteLine (text);
		sw.Close ();
	}

}
