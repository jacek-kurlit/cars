using UnityEngine;
using System.Collections.Generic;
using System;

public class Food {	
    private Position[] positions;
	private Position firstPosition;
	private Position secondPosition;
	private Position currentFoodPosition;
	private int currentIndex = 0;
	
	private float maxFoodCoefficient = 0.03f;

	private const float closeDistance = 20.0f;
	private const float farDistance = 50.0f;

	private TendencyCalculator tendencyCalculator = new TendencyCalculator();
	private HerdParameters herdParameters;

	private Transform visualFoodPosition;
	private SectorsManager sectorManager;
	private PointsCointerner pointsCointainer;

	public Food(HerdParameters herdParameters, Transform visualFoodTransform){
		sectorManager =  GameObject.FindGameObjectWithTag("Sector").GetComponent<SectorsManager>();
		pointsCointainer = sectorManager.getContainerClone();

		List<Vector3> initialTrace = pointsCointainer.getAllMapPoints();

		positions = new Position[initialTrace.Count];		
		visualFoodPosition = visualFoodTransform;
		this.herdParameters = herdParameters;

		for(int i = 0; i < initialTrace.Count; i++){
			positions[i] = new Position(initialTrace[i]);
		}

		initCenterPositions();
	}	   

	public void resetFoodPoints(){
		List<Vector3> newPositions = pointsCointainer.getAllMapPoints();
		try{		
		for(int i = 0; i < newPositions.Count; i++){
			positions[i] = new Position(newPositions[i]);
		}
		}catch(IndexOutOfRangeException e){
			Debug.Log("wtf");
		}
		currentIndex = 0;
	}

	public void updateFoodPosition(Vector3 carPosVecotr){
		Position carPosition = new Position(carPosVecotr);
		Position newPosition = calculateVirtualFoodPosition(carPosition);
		
		currentFoodPosition = newPosition;
		visualizeFood(carPosVecotr);
	}

	public Position getCurrentFoodPosition(){
		return currentFoodPosition;
	}

	private void initCenterPositions(){
		firstPosition = positions[0];
		secondPosition = positions[1];
		currentFoodPosition = firstPosition;
	}		
	
	private Position calculateVirtualFoodPosition(Position carPosition){
		float distance = carPosition.distanceFrom(firstPosition);
		
		if(distance > farDistance){				
			return firstPosition;
		}else{
			return calculateRelatedFoodPosition(carPosition);
		}
	}
	
	private Position calculateRelatedFoodPosition(Position carPosition){
		Position relatedClosePosition =  tendencyCalculator.calculateRelatedPosition(currentFoodPosition,carPosition);
		Position relatedFarPosition = tendencyCalculator.calculateRelatedPosition(secondPosition,currentFoodPosition);
		
		float closeFoodCoefficient = calculateCloseFoodCoefficient(carPosition);
		float farFoodCoefficient = maxFoodCoefficient - closeFoodCoefficient;
		
		Position newPosition = currentFoodPosition +  relatedClosePosition * closeFoodCoefficient + relatedFarPosition * farFoodCoefficient;
		return newPosition;
	}

	private float calculateCloseFoodCoefficient(Position carPosition){
		float distance = carPosition.distanceFrom(firstPosition);
		if(distance >= closeDistance && distance <= farDistance){
			return ((distance - closeDistance)/(farDistance - closeDistance)) * maxFoodCoefficient;
		}
		changeFoodIndex();
		return 0.01f;
	}

	private void changeFoodIndex(){
		if(currentIndex + 1 < positions.Length)
			currentIndex++;
		else
			currentIndex = 0;
		nextPoint();
	}
	
	private void nextPoint(){
		firstPosition = secondPosition;
		secondPosition = farFoodPosition();
		currentFoodPosition = firstPosition;
	}

	private Position farFoodPosition(){
		if(currentIndex + 1 < positions.Length){
			return positions[currentIndex + 1];
		}
		return positions[0];
	}

	private void visualizeFood(Vector3 carVector){
		Vector3 newVisualFoodVector = new Vector3(currentFoodPosition.getX(),currentFoodPosition.getY() + 0.5f,currentFoodPosition.getZ());
		visualFoodPosition.position = newVisualFoodVector;
	}

	public Vector3 getPreviousFoodPosition(){
		if(currentIndex - 1 >= 0)
			return positions[currentIndex -1].toVector3();
		return positions[positions.Length - 1].toVector3();
	}

	public List<Vector3> getSectorVectors(int sectorID){
		return pointsCointainer.getSectorVectors(sectorID);
	}

	public void setSectorVectors(List<Vector3> vectors,int sectorId){
		pointsCointainer.setSectorVectors(vectors,sectorId);
	}

	public List<Vector3> getAllMapPoints(){
		resetFoodPoints();
		return pointsCointainer.getAllMapPoints();
	}
}
