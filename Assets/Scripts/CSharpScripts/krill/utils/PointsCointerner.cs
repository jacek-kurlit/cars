using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PointsCointerner{
	private Dictionary<int,Sector> points = new Dictionary<int, Sector>();

	public PointsCointerner(){}

	public PointsCointerner(PointsCointerner other){
		Dictionary<int,Sector> clone = new Dictionary<int, Sector>();
		for(int i = 1; i <= other.points.Count; i++){		
			clone.Add(i,other.points[i].clone());
		}
		points = clone;
	}

	public void loadPointsFromFile(string fileName, int sectorId){
		string fileContent = FileManager.loadPointsFile(fileName);
		createPointsFromString(fileContent,sectorId);
	}

	public float getBestLapTime(){
		float totalTime = 0.0f;
		foreach(Sector sector in points.Values){
			totalTime += sector.BestSectorTime;
		}
		return totalTime;
	}

	public float getBestSectorTime(int sectorId){
		return points[sectorId].BestSectorTime;
	}

	public void replaceSectorPoints(int sectorId,float newTime, List<Vector3> vectors){
		Sector sector = points[sectorId];
		sector.Points = vectors;
		sector.BestSectorTime = newTime;
	}

	public List<Vector3> getSectorVectors(int sectorId){
		return points[sectorId].Points;
	}

	private void createPointsFromString(string content, int sectorId){	
		StringReader input = new StringReader(content);
		string line = input.ReadLine();
		float bestTime = float.Parse(line);
		line = input.ReadLine();
		Sector sector = new Sector(bestTime,line);
		points.Add(sectorId,sector);
		input.Close();
	}

	public List<Vector3> getAllMapPoints(){
		List<Vector3> allPoints = new List<Vector3>();

		for(int i = 1; i <= points.Count; i++){
			allPoints.AddRange(points[i].Points);
		}

		return allPoints;
	}

	public void setSectorVectors(List<Vector3> vectors,int sectorId){
		points[sectorId].Points = vectors;
	}

}
