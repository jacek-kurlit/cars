using UnityEngine;
using System.Collections.Generic;

public class AlphaCalculator{
	private  TendencyCalculator tendencyCalculator = new TendencyCalculator();

    public Position calculateAlpha(List<Krill> herd, Krill krill, HerdParameters parameters) {        
        Position localAlpha = calculateAlphaLocal(herd, krill, parameters);
        Position targetAlpha = calculateAlphaTarget(parameters,krill);

        return localAlpha + targetAlpha;
    }

    private Position calculateAlphaLocal(List<Krill> neighbours, Krill krill, HerdParameters parameters) {
        Position alphaLocal = new Position();

        foreach (Krill neighbour in neighbours) {
            Position relatedPosition = tendencyCalculator.calculateRelatedPosition(neighbour.getPosition(), krill.getPosition());
            float relatedFitness = tendencyCalculator.calculateRelatedFitness(krill.getFitnessValue(),neighbour.getFitnessValue(), parameters);
			
			relatedPosition = relatedPosition * relatedFitness;
            alphaLocal = alphaLocal + relatedPosition;            
        }
        return alphaLocal;
    }

    private Position calculateAlphaTarget(HerdParameters parameters, Krill krill) {
        Position relatedPosition = tendencyCalculator.calculateRelatedPosition(parameters.getBestFitnessKrill(), krill.getPosition());
        float relatedFitness = tendencyCalculator.calculateRelatedFitness(krill.getFitnessValue(),parameters.getBestFitnessValue(), parameters);

       	relatedPosition = relatedPosition * relatedFitness;
		relatedPosition = relatedPosition * calculateCoefficient(parameters);        

        return relatedPosition;
    }

    private float calculateCoefficient(HerdParameters parameters) {
        return 2.0f * (Random.Range(0.0f,1.0f) + parameters.getIterationRatio());
    }
}
