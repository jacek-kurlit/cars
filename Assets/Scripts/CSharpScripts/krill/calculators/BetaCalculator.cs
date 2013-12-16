using UnityEngine;
using System.Collections;

public class BetaCalculator {
 	private TendencyCalculator tendencyCalculator = new TendencyCalculator();

    public Position calculateBeta(Krill krill, Food food, HerdParameters parameters) {
        Position betaFood = calculateBetaFood(food,krill);
        Position betaBest = calculateBetaBest(krill,parameters);

        return betaFood + betaBest;
    }

    private Position calculateBetaFood(Food food, Krill krill ) {        
        return food.calculateFoodBeta(krill);
    }

    private Position calculateBetaBest(Krill krill, HerdParameters parameters) {
        Position bestRelatedPosition = tendencyCalculator.calculateRelatedPosition(krill.getBestPosition(),krill.getPosition());
        float bestFitness = tendencyCalculator.calculateRelatedFitness(krill.getFitnessValue(),krill.getBestPositionFitness(),parameters);		
		bestRelatedPosition = bestRelatedPosition * bestFitness;        
        return bestRelatedPosition;
    }
}
