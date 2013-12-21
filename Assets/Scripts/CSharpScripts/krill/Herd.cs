using UnityEngine;
using System.Collections.Generic;

public class Herd {
private DiffusionCalculator diffusionCalculator = new DiffusionCalculator();
    private ForagingCalculator foragingCalculator = new ForagingCalculator();
    private MotionCalculator motionCalculator = new MotionCalculator();
    private FitnessCalculator fitnessCalculator = new FitnessCalculator();

    private Crossover crossover = new Crossover();
    private Mutation mutation = new Mutation();

    private List<Krill> herd;
    private Food food;
    private HerdParameters algorithmParameters;
	
	private Transform carTransform;

	public void init(Transform carTransform,Transform[] krillViz, Transform[] initialTrace){
		this.carTransform = carTransform;
		
		algorithmParameters = new HerdParameters();
		Randomizer randomizer = new Randomizer();
		herd = randomizer.randomizeHerd(algorithmParameters,carTransform.position,krillViz);
		food = new Food(initialTrace,algorithmParameters);
	}

    public Vector3 simulate(){ 	
        fitnessEvaluation();
        motionCalculation();
        geneticOperators();
        updateKrillPositions();
		return algorithmParameters.getBestFitnessKrill().toVector3(carTransform.position.y);
    }

    private void fitnessEvaluation() {
       foreach(Krill krill in herd){
           Position krillPosition = krill.getPosition();
           float newFitness = fitnessCalculator.calculateFitness(krillPosition,food);

           krill.setFitnessValue(newFitness);          
       }

		algorithmParameters.updateBestWorstKrill(herd);
    }
    private void motionCalculation() {
        motionCalculator.calculateMotion(herd, algorithmParameters);
		//most VIP funkction now
        foragingCalculator.calculateForagingMotion(herd, algorithmParameters,food);
        diffusionCalculator.calculateDiffusionMotion(herd,algorithmParameters);
    }

    private void geneticOperators() {
        //crossover.crossoverHerd(herd,algorithmParameters);
       // mutation.mutateHerd(herd,algorithmParameters);
    }

    private void updateKrillPositions(){
		//Debug.Log("Start update");
      foreach (Krill krill in herd){
          krill.updatePosition(carTransform.position);
      } 
		food.switchToAnotherFood(algorithmParameters.getBestFitnessKrill(),herd);
		//Debug.Log("Stop update");
    }
}
