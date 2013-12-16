using UnityEngine;
using System.Collections.Generic;


public class Food {	
	private const float distanceEffectivnes = 10.0f;
    private Position[] positions;
	private int currentIndex = 1;

	private TendencyCalculator tendencyCalculator = new TendencyCalculator();
	private HerdParameters herdParameters;

	public Food(Transform[] initialTrace, HerdParameters herdParameters){
		positions = new Position[initialTrace.Length];		
		this.herdParameters = herdParameters;
		for(int i = 0; i < initialTrace.Length; i++){
			positions[i] = new Position(initialTrace[i].position);
		}
	}	   

	public Position calculateFoodBeta(Krill krill){
		float coefficient  = coefficientForKrill(krill);
		float distantCoefficient = 1.0f - coefficient;
		float fitnessValue = 1.0f;

		Position closeFood = positions[currentIndex];
		Position farFood = farFoodPosition();

		//póki co fitness jest jeden ale powinnien byc rozny dla innej pozycji - nie oze to byc odleglosc bo zawsze wyjdzie 0
		float closeFitnes = tendencyCalculator.calculateRelatedFitness(krill.getFitnessValue(),fitnessValue,herdParameters);

		Position closeRelatedPosition = tendencyCalculator.calculateRelatedPosition(closeFood,krill.getPosition());
		Position farRelatedPosition = tendencyCalculator.calculateRelatedPosition(farFood,krill.getPosition());

		Position betaFood = closeRelatedPosition * closeFitnes * coefficient + farRelatedPosition * closeFitnes * distantCoefficient;

		return betaFood;
	}

	public Position getCurrentFoodPosition(){
		return positions[currentIndex];
	}
	//byc moze ta wartosc jest za mala bo w podstawowym jest max 2 a tutaj max to 1
	//tutaj mozna dodac przejcie do nastepnego food bo dystans by sie zgadzal
	private float coefficientForKrill(Krill krill){
		float distant = positions[currentIndex].distanceFrom(krill.getPosition());

		if(distant > 10.0f)
			return 1.0f;

		return distant/10.0f;
		
	}

	private Position farFoodPosition(){
		if(currentIndex + 1 < positions.Length){
			return positions[currentIndex + 1];
		}
		return positions[1];
	}	

	private void changeFoodIndex(){
		if(currentIndex + 1 < positions.Length){
			currentIndex++;
		}else{
			currentIndex = 1;
		}
	}

	public void switchToAnotherFood(Position bestKrill){
		float distant = positions[currentIndex].distanceFrom(bestKrill);
		//Debug.Log("Distant is " + distant);
		if(distant < 4.5f){
			//herdParameters.resetFitness();
			changeFoodIndex();
			Debug.Log("Switched to another food position[ " + currentIndex + "] " + positions[currentIndex]);
		}
	}
}
