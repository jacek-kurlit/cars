using UnityEngine;
using System.Collections.Generic;

public class Herd {
    private FitnessCalculator fitnessCalculator = new FitnessCalculator();

    private List<Krill> herd;
    private Food food;
    private HerdParameters algorithmParameters;
	private KrillSetter krillSetter;

	private Transform carTransform;

	public Herd(Transform carTransform,Transform[] krillViz, Transform[] initialTrace){
		this.carTransform = carTransform;
		
		algorithmParameters = new HerdParameters();
		krillSetter = new TriangleKrillSetter(algorithmParameters);
		herd = krillSetter.initHerd(carTransform,krillViz);
		food = new Food(initialTrace,algorithmParameters);
	}

    public Vector3 simulate(){ 	        
		fitnessEvaluation();
        updateKrillPositions();
		return algorithmParameters.getBestFitnessKrill().toVector3(carTransform.position.y);
    }

    private void fitnessEvaluation() {
       foreach(Krill krill in herd){
           Position krillPosition = krill.getPosition();
           float newFitness = fitnessCalculator.calculateFitness(krillPosition,food);

           krill.setFitnessValue(newFitness);          
       }
    }

    private void updateKrillPositions(){
		krillSetter.placeKrills(herd, carTransform);
		food.updateFoodPosition(carTransform.position);
		algorithmParameters.updateBestWorstKrill(herd);
    }
}
