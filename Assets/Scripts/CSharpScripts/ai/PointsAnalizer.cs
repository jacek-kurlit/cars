using UnityEngine;
using System.Collections.Generic;

public class PointsAnalizer : MonoBehaviour {

	private Transform carTransform;
	private SectorsManager sectorManager;
	private List<Vector3> newSectorPoints;
	private List<Vector3> allMapPoints;
	private int currentIndex;

	public int lap = 1;
	private float randomOffset = 4.0f;
	// Use this for initialization
	void Start () {
		sectorManager =  GameObject.FindGameObjectWithTag("Sector").GetComponent<SectorsManager>();
		carTransform = gameObject.transform;
		allMapPoints = sectorManager.getAllMapPoints();
		newSectorPoints = new List<Vector3>();
		currentIndex = 0;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 heading = allMapPoints[currentIndex] - carTransform.position;
		float angle = Vector3.Angle(heading,carTransform.forward);

		if(angle >= 90.0f){
			if(currentIndex < allMapPoints.Count)
				newSectorPoints.Add(randomizePosition(allMapPoints[currentIndex]));
			else
				newSectorPoints.Add(randomizePosition(carTransform.position));
			changeIndex();

		}
	}

	private Vector3 randomizePosition(Vector3 prevFoodPosition){
		float xOffset = Random.Range(-randomOffset,randomOffset);
		float zOffset = Random.Range(-randomOffset,randomOffset);
		return new Vector3(prevFoodPosition.x + xOffset,prevFoodPosition.y,prevFoodPosition.z + zOffset);
	}
	public void resetPoints(){
		newSectorPoints = new List<Vector3>();
	}

	public void setAllMapPoints(List<Vector3> allMapPoints){
		currentIndex = 0;
		this.allMapPoints = allMapPoints;
	}

	public List<Vector3> getNewSectorPoints(){
		return newSectorPoints;
	}

	private void changeIndex(){
		if(currentIndex + 1 < allMapPoints.Count)
			currentIndex++;
		else
			currentIndex = 0;
	}
}
