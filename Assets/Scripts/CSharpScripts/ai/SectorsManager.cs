using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SectorsManager : MonoBehaviour {
	public int sectorsCount;
	private PointsCointerner pointsCointerner = new PointsCointerner();
	// Use this for initialization
	void Awake() {
		for(int i = 1; i <= sectorsCount; i++){
			pointsCointerner.loadPointsFromFile(Application.dataPath + "/KrillData/wayPoints" + i + ".txt",i);
		}
	}

	public float getBestLapTime(){
		return FileManager.loadBestLapTime();
	}

	public float getBestSectorTime(int sectorId){
		return pointsCointerner.getBestSectorTime(sectorId);
	}

	public void saveNewSectorVectors(int sectorId,float newSectorTime,List<Vector3> points){
		pointsCointerner.replaceSectorPoints(sectorId,newSectorTime,points);
		FileManager.saveNewSectorPoints(sectorId,newSectorTime,points);
		FileManager.saveNewSectorTime(sectorId,"Nowy rekord sektoru " + newSectorTime);
	}

	public List<Vector3> getAllMapPoints(){
		return pointsCointerner.getAllMapPoints();
	}

	public PointsCointerner getContainerClone(){
		return new PointsCointerner(pointsCointerner);
	}

	public List<Vector3> getSectorVectors(int sectorId){
		return pointsCointerner.getSectorVectors(sectorId);
	}
}
