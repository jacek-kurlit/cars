using UnityEngine;
using System.Collections.Generic;


public class Food {	
    private Position[] positions;
	private Position centerPosition;
	private Position nextCenterPosition;
	private Position currentFoodPosition;
	private int currentIndex = 1;

	private float foodCarOffset = 10.0f;
	private float maxFoodCoefficient = 0.05f;

	private const float closeDistance = 10.0f;
	private const float farDistance = 50.0f;

	private TendencyCalculator tendencyCalculator = new TendencyCalculator();
	private HerdParameters herdParameters;

	private Transform visualFoodPosition;

	public Food(Transform[] initialTrace, HerdParameters herdParameters){
		positions = new Position[initialTrace.Length];		
		visualFoodPosition = GameObject.FindGameObjectWithTag("Food").transform;
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
		centerPosition = calculateCenterPosition(1);
		nextCenterPosition = calculateNextCenterPosition();
		currentFoodPosition = centerPosition;
	}		

	private Position calculateCenterPosition(int startIndex){
		Position closePosition = positions[startIndex];
		Position farPosition = farFoodPosition(startIndex);
		return (closePosition + farPosition)/2.0f;
	}
	
	private Position farFoodPosition(int startIndex){
		if(startIndex + 1 < positions.Length){
			return positions[startIndex + 1];
		}
		return positions[1];
	}

	private Position calculateNextCenterPosition(){
		int next = nextIndex(currentIndex);
		Position nextCenterPosition = calculateCenterPosition(next);
			
		return nextCenterPosition;
	}
	private int  nextIndex(int index){
		if(index + 1 < positions.Length){
			return index + 1;
		}else{
			return 1;
		}
	}
	
	private Position calculateVirtualFoodPosition(Position carPosition){
		float distance = carPosition.distanceFrom(centerPosition);
		
		if(distance > farDistance){				
			return centerPosition;
		}else{
			return calculateRelatedFoodPosition(carPosition);
		}
	}
	
	private Position calculateRelatedFoodPosition(Position carPosition){
		Position relatedClosePosition =  tendencyCalculator.calculateRelatedPosition(currentFoodPosition,carPosition);
		Position relatedFarPosition = tendencyCalculator.calculateRelatedPosition(nextCenterPosition,carPosition);
		
		float closeFoodCoefficient = calculateCloseFoodCoefficient(carPosition);
		float farFoodCoefficient = maxFoodCoefficient - closeFoodCoefficient;
		
		Position newPosition = currentFoodPosition +  relatedClosePosition * closeFoodCoefficient + relatedFarPosition * farFoodCoefficient;
		return newPosition;
	}

	private float calculateCloseFoodCoefficient(Position carPosition){
		float distance = carPosition.distanceFrom(centerPosition);
		if(distance >= closeDistance && distance <= farDistance){
			return ((distance - closeDistance)/(farDistance - closeDistance)) * maxFoodCoefficient;
		}
		changeFoodIndex();
		return 1.0f;
	}

	private void changeFoodIndex(){
		Debug.Log("Leaving point " + currentIndex + " next target at point " + (currentIndex + 1));
		currentIndex = nextIndex(currentIndex);
		nextPoint();
	}

	private void nextPoint(){
		centerPosition = nextCenterPosition;
		nextCenterPosition = calculateNextCenterPosition();
		currentFoodPosition = centerPosition;
	}

	private void visualizeFood(Vector3 carVector){
		Vector3 newVisualFoodVector = new Vector3(currentFoodPosition.getX(),currentFoodPosition.getY() + 0.5f,currentFoodPosition.getZ());
		visualFoodPosition.position = newVisualFoodVector;
	}
			
}
