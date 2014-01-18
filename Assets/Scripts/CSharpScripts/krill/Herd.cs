using UnityEngine;
using System.Collections.Generic;

public class Herd {
    private FitnessCalculator fitnessCalculator = new FitnessCalculator();

    private List<Krill> herd;
    private Food food;
    private HerdParameters algorithmParameters;
	private KrillSetter krillSetter;

	private MotionCalculator motionCalculator = new MotionCalculator();
	private DiffusionCalculator diffusionCalculator = new DiffusionCalculator();

	private Transform carTransform;
	private Transform[] krillViz;

	public Herd(Transform carTransform,Transform[] krillViz,Transform visualFoodTransform){
		this.carTransform = carTransform;
		this.krillViz = krillViz;	
		krillSetter = new CircleKrillSetter(new HerdParameters());
		food = new Food(algorithmParameters,visualFoodTransform);
	}

    public Krill simulate(){ 	        
		init();
		while(algorithmParameters.nextIteration()){
			fitnessEvaluation();
			motionCalculations();
        	updateKrillPositions();
		}
		Krill bestKrill = algorithmParameters.getBestFitnessKrill();
		bestKrill.calculateSpeed();
		return bestKrill;
    }

	private void init(){
		algorithmParameters = new HerdParameters();
		herd = krillSetter.initHerd(carTransform,krillViz);
	}

    private void fitnessEvaluation() {
       foreach(Krill krill in herd){
           Position krillPosition = krill.getPosition();
           float newFitness = fitnessCalculator.calculateFitness(krillPosition,food);

           krill.setFitnessValue(newFitness);  
		   algorithmParameters.updateBestWorstKrill(krill);
       }
    }

	private void motionCalculations(){
		motionCalculator.calculateMotion(herd,algorithmParameters);
		diffusionCalculator.calculateDiffusionMotion(herd,algorithmParameters);
	}

    private void updateKrillPositions(){
		foreach(Krill krill in herd){
			krill.updatePosition(carTransform.position, algorithmParameters);
		}
		food.updateFoodPosition(carTransform.position);	
    }

	public Vector3 getCurrentFoodPosition(){
		return food.getPreviousFoodPosition();
	}

	public void resetFoodPoints(){
		food.resetFoodPoints();
	}

	public List<Vector3> getSectorVectors(int sectorId){
		return food.getSectorVectors(sectorId);
	}

	public void setSectorVectors(List<Vector3> vectors,int sectorId){
		food.setSectorVectors(vectors,sectorId);
	}

	public List<Vector3> getAllMapPoints(){
		return food.getAllMapPoints();
	}
}