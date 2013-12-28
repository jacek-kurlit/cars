using UnityEngine;
using System.Collections.Generic;


public class Food {	
    private Position[] positions;
	private Position currentFoodPosition;
	private int currentIndex = 1;

	private float foodCarOffset = 10.0f;
	private float maxFoodCoefficient = 10.0f;

	private const float closeDistance = 10.0f;
	private const float farDistance = 50.0f;

	private TendencyCalculator tendencyCalculator = new TendencyCalculator();
	private HerdParameters herdParameters;

	private Transform visualFoodPosition;

	public Food(Transform[] initialTrace, HerdParameters herdParameters){
		positions = new Position[initialTrace.Length];		
		visualFoodPosition = GameObject.FindGameObjectWithTag("Food").transform;
		this.herdParameters = herdParameters;
		currentFoodPosition = new Position(visualFoodPosition.position);
		for(int i = 0; i < initialTrace.Length; i++){
			positions[i] = new Position(initialTrace[i].position);
		}
	}	   

	public void updateFoodPosition(Vector3 carPosVecotr){
		Position carPosition = new Position(carPosVecotr);
		Position newPosition = calculateVirtualFoodPosition(carPosition);

		currentFoodPosition = newPosition;
		clapToCar(carPosVecotr);
	}

	private Position calculateVirtualFoodPosition(Position carPosition){
			float distance = carPosition.distanceFrom(positions[currentIndex]);
			if(distance > farDistance){
				Debug.Log("Position of " + positions[currentIndex]);
				return positions[currentIndex];
			}else{
				return calculateRelatedFoodPosition(carPosition);
			}
	}

	private Position calculateRelatedFoodPosition(Position carPosition){
		Position relatedClosePosition =  tendencyCalculator.calculateRelatedPosition(positions[currentIndex],carPosition);
		Position relatedFarPosition = tendencyCalculator.calculateRelatedPosition(farFoodPosition(),carPosition);
		
		float closeFoodCoefficient = calculateCloseFoodCoefficient(carPosition);
		float farFoodCoefficient = maxFoodCoefficient - closeFoodCoefficient;
		
		Position newPosition = positions[currentIndex] +  relatedClosePosition * closeFoodCoefficient + relatedFarPosition * farFoodCoefficient;
		
		return newPosition;
	}
	private float calculateCloseFoodCoefficient(Position carPosition){
		float distance = carPosition.distanceFrom(positions[currentIndex]);
		if(distance >= closeDistance && distance <= farDistance){
			return ((distance - closeDistance)/(farDistance - closeDistance)) * maxFoodCoefficient;
		}
		changeFoodIndex();
		return 1.0f;
	}

	private void clapToCar(Vector3 carVector){
		//currentFoodPosition.setX(Mathf.Clamp(currentFoodPosition.getX(),carVector.x - foodCarOffset,carVector.x + foodCarOffset));
		//currentFoodPosition.setZ(Mathf.Clamp(currentFoodPosition.getZ(),carVector.z - foodCarOffset,carVector.z + foodCarOffset));
		Vector3 newVisualFoodVector = new Vector3(currentFoodPosition.getX(),carVector.y + 1.0f,currentFoodPosition.getZ());
		visualFoodPosition.position = newVisualFoodVector;
	}
	
	public Position getCurrentFoodPosition(){
		return currentFoodPosition;
	}
	
	private Position farFoodPosition(){
		if(currentIndex + 1 < positions.Length){
			return positions[currentIndex + 1];
		}
		return positions[1];
	}	


	private void switchToAnotherFood(Position bestKrill){
		float distant = positions[currentIndex].distanceFrom(bestKrill);
		if(distant < closeDistance){
			changeFoodIndex();
		}
	}

	private void changeFoodIndex(){
		if(currentIndex + 1 < positions.Length){
			currentIndex++;
		}else{
			currentIndex = 1;
		}
	}
}
