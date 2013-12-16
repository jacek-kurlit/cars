using UnityEngine;
using System.Collections;

public class FitnessCalculator {
	 public float calculateFitness(Position krill,Food food){
		float distance = food.getCurrentFoodPosition().distanceFrom(krill);

		return distance;
    }
}
