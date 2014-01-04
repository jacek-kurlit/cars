using UnityEngine;
using System.Collections.Generic;

public class HerdParameters{
 	public float N_MAX = 0.01f;
    public float Vf = 0.02f;
    public float inertiaWeight = 1.0f; // from range [0,1]
    public float inertiaForagingWeight = 0.9f; // from range [0,1]
	//nie zmieniać!
    public int herdSize = 5;

	private int currentIteration = 0;
	private int maxIteration = 5;

	private Krill bestKrill;
    private Position bestFitnessPosition;
    private float bestFitnessValue = 1000000.0f;
    private float worstFitnessValue = -100000.0f;

    public float getRelatedFitnessValue(){
        return worstFitnessValue - bestFitnessValue;
    }

    public Position getBestFitnessPosition() {
        return bestFitnessPosition;
    }

	public Krill getBestFitnessKrill(){
		return bestKrill;
	}
	public bool nextIteration(){
		if(currentIteration <= maxIteration){
			currentIteration++;
			return true;
		}

		return false;
	}
    public float getIterationRatio(){
		return (float)currentIteration/maxIteration;
    }

    public float randomizeDMAX(){
        float ran =  Random.Range(2.0f,8.0f);
        return ran/1000.0f;
    }

    public void updateBestWorstKrill(Krill krill){


		if(krill.getFitnessValue() < bestFitnessValue){
			bestKrill = krill;
			bestFitnessValue = krill.getFitnessValue();
			bestFitnessPosition = krill.getPosition().getClone();
		}
		if(krill.getFitnessValue() > worstFitnessValue){
			worstFitnessValue = krill.getFitnessValue();
		}		         
    }

    public float getBestFitnessValue() {
        return bestFitnessValue;
    }
	
}
