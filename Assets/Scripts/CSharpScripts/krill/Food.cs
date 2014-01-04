using UnityEngine;
using System.Collections.Generic;


public class Food {	
    private Position[] positions;
	private Position firstPosition;
	private Position secondPosition;
	private Position currentFoodPosition;
	private int currentIndex = 1;
	
	private float maxFoodCoefficient = 0.03f;

	private const float closeDistance = 20.0f;
	private const float farDistance = 50.0f;

	private TendencyCalculator tendencyCalculator = new TendencyCalculator();
	private HerdParameters herdParameters;

	private Transform visualFoodPosition;

	public Food(Transform[] initialTrace, HerdParameters herdParameters, Transform visualFoodTransform){
		positions = new Position[initialTrace.Length];		
		visualFoodPosition = visualFoodTransform;
		this.herdParameters = herdParameters;

		for(int i = 0; i < initialTrace.Length; i++){
			positions[i] = new Position(initialTrace[i].position);
		}

		initCenterPositions();
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
		firstPosition = positions[1];
		secondPosition = positions[2];
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
			currentIndex = 1;
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
		return positions[1];
	}

	private void visualizeFood(Vector3 carVector){
		Vector3 newVisualFoodVector = new Vector3(currentFoodPosition.getX(),currentFoodPosition.getY() + 0.5f,currentFoodPosition.getZ());
		visualFoodPosition.position = newVisualFoodVector;
	}

	public Vector3 getFirstFoodPosition(){
		return positions[currentIndex].toVector3();
	}
			
}
