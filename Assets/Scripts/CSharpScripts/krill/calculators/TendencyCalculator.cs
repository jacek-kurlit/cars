using UnityEngine;
using System.Collections;

public class TendencyCalculator {
    private static float e = 0.001f;

    public Position calculateRelatedPosition(Position neighbour, Position krill) {
        Position related = neighbour - krill; 
		related = related/(neighbour.distanceFrom(krill) + e); 	
        return related;
    }

    public float calculateRelatedFitness(float krillFitness,float neighbourFitness, HerdParameters parameters) {
        return  (krillFitness - neighbourFitness) / parameters.getRelatedFitnessValue();
    }
}
