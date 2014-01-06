using UnityEngine;
using System.Collections;

// This class is repsonsible for controlling inputs to the car.
// Change this code to implement other input types, such as support for analogue input, or AI cars.
[RequireComponent (typeof (Drivetrain))]
public class CarController : MonoBehaviour {

	// Add all wheels of the car here, so brake and steering forces can be applied to them.
	public Wheel[] wheels;
	
	// A transform object which marks the car's center of gravity.
	// Cars with a higher CoG tend to tilt more in corners.
	// The further the CoG is towards the rear of the car, the more the car tends to oversteer. 
	// If this is not set, the center of mass is calculated from the colliders.
	public Transform centerOfMass;

	// A factor applied to the car's inertia tensor. 
	// Unity calculates the inertia tensor based on the car's collider shape.
	// This factor lets you scale the tensor, in order to make the car more or less dynamic.
	// A higher inertia makes the car change direction slower, which can make it easier to respond to.
	public float inertiaFactor = 1.5f;

	public string carId;

	public Transform visualFoodTransform;
	public Transform[] krillVis;
	// current input state
	float brake;
	float throttle;
	float throttleInput;
	float steering;
	float lastShiftTime = -1;
	float handbrake;
		
	// cached Drivetrain reference
	Drivetrain drivetrain;
	
	//Who Controller the car
	public Player player;
	public bool isHuman;
	// How long the car takes to shift gears
	public float shiftSpeed = 0.8f;
	

	// These values determine how fast throttle value is changed when the accelerate keys are pressed or released.
	// Getting these right is important to make the car controllable, as keyboard input does not allow analogue input.
	// There are different values for when the wheels have full traction and when there are spinning, to implement 
	// traction control schemes.
		
	// How long it takes to fully engage the throttle
	public float throttleTime = 1.0f;
	// How long it takes to fully engage the throttle 
	// when the wheels are spinning (and traction control is disabled)
	public float throttleTimeTraction = 10.0f;
	// How long it takes to fully release the throttle
	public float throttleReleaseTime = 0.5f;
	// How long it takes to fully release the throttle 
	// when the wheels are spinning.
	public float throttleReleaseTimeTraction = 0.1f;

	// Turn traction control on or off
	public bool tractionControl = true;

	private bool crashFlag = false;
	private float timer = 0.0f;
	private float respTime = 3.0f;
	// These values determine how fast steering value is changed when the steering keys are pressed or released.
	// Getting these right is important to make the car controllable, as keyboard input does not allow analogue input.
	
	// How long it takes to fully turn the steering wheel from center to full lock
	public float steerTime = 1.2f;
	// This is added to steerTime per m/s of velocity, so steering is slower when the car is moving faster.
	public float veloSteerTime = 0.1f;

	// How long it takes to fully turn the steering wheel from full lock to center
	public float steerReleaseTime = 0.6f;
	// This is added to steerReleaseTime per m/s of velocity, so steering is slower when the car is moving faster.
	public float veloSteerReleaseTime = 0f;
	// When detecting a situation where the player tries to counter steer to correct an oversteer situation,
	// steering speed will be multiplied by the difference between optimal and current steering times this 
	// factor, to make the correction easier.
	public float steerCorrectionFactor = 4.0f;

	// Used by SoundController to get average slip velo of all wheels for skid sounds.
	public float slipVelo {
		get {
			float val = 0.0f;
			foreach(Wheel w in wheels)
				val += w.slipVelo / wheels.Length;
			return val;
		}
	}
	
	// Initialize
	void Start () 
	{
		if(isHuman)
			player = new HumanPlayer();
		else{
			player = new EnemyPlayer(gameObject,visualFoodTransform,krillVis);
		}
		
		if (centerOfMass != null)
			rigidbody.centerOfMass = centerOfMass.localPosition;
		rigidbody.inertiaTensor *= inertiaFactor;
		drivetrain = GetComponent<Drivetrain>();
	}
	
	void Update () 
	{		
		//Steering
		adjustSteering();
		
		// Throttle/Brake
		thorttleAndBrake();
		
		// Gear shifting
		shiftGear();

		// Apply inputs
		applyPhysicToWheels();

		if(crashFlag){
			countDownCrash();
		}
	}
	
	private void adjustSteering(){
		Vector3 carDir = transform.forward;
		float fVelo = rigidbody.velocity.magnitude;
		Vector3 veloDir = rigidbody.velocity * (1/fVelo);
		float angle = -Mathf.Asin(Mathf.Clamp( Vector3.Cross(veloDir, carDir).y, -1, 1));
		float optimalSteering = angle / (wheels[0].maxSteeringAngle * Mathf.Deg2Rad);
		if (fVelo < 1)
			optimalSteering = 0;
		
		int steerInput = player.getVerticalInput();
		
		if (steerInput < steering)
		{
			float steerSpeed = (steering>0)?(1/(steerReleaseTime+veloSteerReleaseTime*fVelo)) :(1/(steerTime+veloSteerTime*fVelo));
			if (steering > optimalSteering)
				steerSpeed *= 1 + (steering-optimalSteering) * steerCorrectionFactor;
			steering -= steerSpeed * Time.deltaTime;
			if (steerInput > steering)
				steering = steerInput;
		}
		else if (steerInput > steering)
		{
			float steerSpeed = (steering<0)?(1/(steerReleaseTime+veloSteerReleaseTime*fVelo)) :(1/(steerTime+veloSteerTime*fVelo));
			if (steering < optimalSteering)
				steerSpeed *= 1 + (optimalSteering-steering) * steerCorrectionFactor;
			steering += steerSpeed * Time.deltaTime;
			if (steerInput < steering)
				steering = steerInput;
		}
	}	
	
	private void thorttleAndBrake(){
		float horizontalInput = player.getHorizontalInput();
		bool accelKey = horizontalInput==1?true:false;
		bool brakeKey = horizontalInput==-1?true:false;
		
		if (drivetrain.automatic && drivetrain.gear == 0)
		{
			bool temp = accelKey;
			accelKey = brakeKey;
			brakeKey = temp;			
		}
		
		thorttleEngine(accelKey);		
		hitBrakes(brakeKey);		
		// Handbrake
		handbrake = Mathf.Clamp01 ( handbrake + (Input.GetKey (KeyCode.Space)? Time.deltaTime: -Time.deltaTime) );
	}
	
	private void thorttleEngine(bool accelKey){		
		if (accelKey)
		{
			if (drivetrain.slipRatio < 0.10f)
				throttle += Time.deltaTime / throttleTime;
			else if (!tractionControl)
				throttle += Time.deltaTime / throttleTimeTraction;
			else
				throttle -= Time.deltaTime / throttleReleaseTime;

			if (throttleInput < 0)
				throttleInput = 0;
			throttleInput += Time.deltaTime / throttleTime;
			brake = 0;
		}
		else 
		{
			if (drivetrain.slipRatio < 0.2f)
				throttle -= Time.deltaTime / throttleReleaseTime;
			else
				throttle -= Time.deltaTime / throttleReleaseTimeTraction;
		}
		throttle = Mathf.Clamp01 (throttle);
	}
	
	private void hitBrakes(bool brakeKey){
		if (brakeKey)
		{
			if (drivetrain.slipRatio < 0.2f)
				brake += Time.deltaTime / throttleTime;
			else
				brake += Time.deltaTime / throttleTimeTraction;
			throttle = 0;
			throttleInput -= Time.deltaTime / throttleTime;
		}
		else 
		{
			if (drivetrain.slipRatio < 0.2f)
				brake -= Time.deltaTime / throttleReleaseTime;
			else
				brake -= Time.deltaTime / throttleReleaseTimeTraction;
		}
		brake = Mathf.Clamp01 (brake);
		throttleInput = Mathf.Clamp (throttleInput, -1.0f, 1.0f);
	}
	
	private void shiftGear(){
		float shiftThrottleFactor = Mathf.Clamp01((Time.time - lastShiftTime)/shiftSpeed);
		drivetrain.throttle = throttle * shiftThrottleFactor;
		drivetrain.throttleInput = throttleInput;
		
		if(Input.GetKeyDown(KeyCode.A))
		{
			lastShiftTime = Time.time;
			drivetrain.ShiftUp ();
		}
		if(Input.GetKeyDown(KeyCode.Z))
		{
			lastShiftTime = Time.time;
			drivetrain.ShiftDown ();
		}
	}
	
	private void applyPhysicToWheels(){
		foreach(Wheel w in wheels)
		{
			w.brake = brake;
			w.handbrake = handbrake;
			w.steering = steering;
		}
	}
	
	public void OnCollisionEnter(Collision other){
		if(!(other.gameObject.tag == "Player") && !(other.gameObject.tag == "Enemy"))
			if(!crashFlag){
				crashFlag = true;
				timer = 0.0f;
			}
	}

	public void OnCollisionExit(Collision other){
		crashFlag = false;
		timer = 0.0f;
	}

	private void countDownCrash(){
		timer += Time.deltaTime;
		if(timer >= respTime){
			player.resetPosition();
			crashFlag = false;
			timer = 0.0f;
		}
	}
}
