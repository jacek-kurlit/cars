using UnityEngine;
using System.Collections.Generic;

public class HerdParameters{
 	public float N_MAX = 0.01f;
    public float Vf = 0.02f;
    public float inertiaWeight = 0.5f; // from range [0,1]
    public float inertiaForagingWeight = 0.9f; // from range [0,1]
    public int herdSize = 5;

    private Position bestFitnessPosition;
    private float bestFitnessValue = 1000000.0f;
    private float worstFitnessValue = -100000.0f;

    public float getRelatedFitnessValue(){
        return worstFitnessValue - bestFitnessValue;
    }

    public Position getBestFitnessKrill() {
        return bestFitnessPosition;
    }

    public float getIterationRatio(){
		return 0.0f;
    }

    public float randomizeDMAX(){
        float ran =  Random.Range(2.0f,8.0f);
        return ran/1000.0f;
    }

	//tak naprawde to jest najlepszy i najgorszy krill w tej iteracji a nie w ogóle.
	//Trasa po ktorej trzeba jechac dynamicznie sie zmienia
	//Inna opcja to reset statystyk ale tylko przy mzianie food na nastepny
    public void updateBestWorstKrill(List<Krill> herd){
		float currentBestFitness = 100000.0f;
		float currentWorstFitness = -100000.0f;
		Position currentBestPosition = new Position();

		foreach(Krill krill in herd){
			if(krill.getFitnessValue() < currentBestFitness){
				currentBestFitness = krill.getFitnessValue();
				currentBestPosition = krill.getPosition().getClone();
			}
			if(krill.getFitnessValue() > currentWorstFitness){
				currentWorstFitness = krill.getFitnessValue();
			}
		}
      
    	bestFitnessValue = currentBestFitness;
		bestFitnessPosition = currentBestPosition;
   		worstFitnessValue = currentWorstFitness;      
    }

    public float getBestFitnessValue() {
        return bestFitnessValue;
    }

	public void resetFitness(){
		bestFitnessValue = 1000000.0f;
		worstFitnessValue = -100000.0f;
		bestFitnessPosition = new Position();
	}
}
