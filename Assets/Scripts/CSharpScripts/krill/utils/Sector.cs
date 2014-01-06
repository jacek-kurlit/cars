using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

public class Sector{
	private float bestSectorTime;
	private List<Vector3> points;

	private Sector(float bestTime, List<Vector3> pointsToClone){
		this.bestSectorTime = bestTime;
		List<Vector3> clone = new List<Vector3>();

		foreach(Vector3 vector in pointsToClone){
			clone.Add(new Vector3(vector.x,vector.y,vector.z));
		}

		this.points = clone;
	}

	public Sector(float time, string pointsString){
		bestSectorTime = time;
		points = createListFromString(pointsString);	
	}

	public Sector clone(){
		return new Sector(bestSectorTime, points);
	}

	private List<Vector3> createListFromString(string points){
		string[] vectors = points.Split(';');
		List<Vector3> pointsList = new List<Vector3>();

		for(int i =0; i < vectors.Length - 1;i++){
			Vector3 vec = createVectorFromString(vectors[i]);
			pointsList.Add(vec);
		}

		return pointsList;
	}

	private Vector3 createVectorFromString(string vector){
		string[] fields = vector.Split(',');

		float x = float.Parse(fields[0],CultureInfo.InvariantCulture);
		float y = float.Parse(fields[1],CultureInfo.InvariantCulture);
		float z = float.Parse(fields[2],CultureInfo.InvariantCulture);

		return new Vector3(x,y,z);
	}

	public float BestSectorTime {
		get {
			return this.bestSectorTime;
		}
		set {
			bestSectorTime = value;
		}
	}

	public List<Vector3> Points {
		get {
			return this.points;
		}
		set {
			points = value;
		}
	}
}
