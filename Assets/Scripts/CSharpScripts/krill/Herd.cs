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

	public Herd(Transform carTransform,Transform[] krillViz, Transform[] initialTrace){
		this.carTransform = carTransform;
		this.krillViz = krillViz;	

		krillSetter = new TriangleKrillSetter(new HerdParameters());
		food = new Food(initialTrace,algorithmParameters);
	}

    public Vector3 simulate(){ 	        
		init();
		while(algorithmParameters.nextIteration()){
			fitnessEvaluation();
			motionCalculations();
        	updateKrillPositions();
		}
		return algorithmParameters.getBestFitnessKrill().toVector3();
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
			krill.updatePosition(carTransform.position);
		}
		food.updateFoodPosition(carTransform.position);	
    }
}
