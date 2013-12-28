using UnityEngine;
using System.Collections;

public class Krill {
    private Position position;
    private float fitnessValue;
    private Position motionInduced = new Position();
    private Position foragingMotion = new Position();
    private Position diffusionMotion = new Position();
    private Position bestPosition = new Position();
    private float bestPositionFitness = 10000.0f;
	private Transform krillVizualPosition;

    public Krill(Position position, Transform kvp) {
        this.position = position;
		krillVizualPosition = kvp;
    }

    public void updatePosition(Vector3 carPosition,Position newPosition){
		position = newPosition;
			
		clampToCar(carPosition);
    }

	private void clampToCar(Vector3 carPosition){
		//position.setX(Mathf.Clamp(position.getX(),carPosition.x - 5.0f,carPosition.x + 5.0f));
		//position.setZ(Mathf.Clamp(position.getZ(),carPosition.z - 5.0f,carPosition.z + 5.0f));
		krillVizualPosition.position = new Vector3(position.getX(),carPosition.y + 1.0f,position.getZ());
	}

    public Position getPosition() {
        return position;
    }

    public float getFitnessValue() {
        return fitnessValue;
    }

    public void setFitnessValue(float fitnessValue) {
        bestPositionFitness = fitnessValue;
        bestPosition = position.getClone();
        this.fitnessValue = fitnessValue;
    }
  
	public Position getMotionInduced(){
		return motionInduced;
	}

    public void setMotionInduced(Position motionInduced) {
        this.motionInduced = motionInduced;
    }

    public Position getForagingMotion() {
        return foragingMotion;
    }

    public void setForagingMotion(Position foragingMotion) {
        this.foragingMotion = foragingMotion;
    }

    public Position getBestPosition() {
        return bestPosition;
    }

    public float getBestPositionFitness() {
        return bestPositionFitness;
    }

    public void setDiffusionMotion(Position diffusionMotion) {
        this.diffusionMotion = diffusionMotion;
    }

	public void setPosition(Position position){
		this.position = position;
	}

	public void resetBestPosition(){
		bestPositionFitness = fitnessValue;
		bestPosition = position.getClone();
		//Debug.Log("new best fitness " + fitnessValue + " and best position " + bestPosition);
	}
}
