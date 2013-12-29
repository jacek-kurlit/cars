using UnityEngine;
using System.Collections;

public class Position{
    private float x;
	private float z;	
	private float y;

    public Position() {
        x = 0.0f;
		z = 0.0f;
		y = 0.0f;
    }   
	
	public Position(Vector3 vector){
		x = vector.x;
		z = vector.z;
		y = vector.y;
	}

	public Position(float x,float z) {
		this.x = x;
		this.z = z;
	}

	public Position(float x,float y,float z) {
        this.x = x;
		this.y = y;
		this.z = z;
    }

    public float getX() {
        return x;
    }

	public float getY(){
		return y;
	}

	public float getZ(){
		return z;
	}

    public void setX(float x) {
        this.x = x;
    }
	
	public void setZ(float z){
		this.z = z;
	}

	
	public float distanceFrom(Position other){
		float poz_x = this.x - other.x;
		float poz_z = this.z - other.z;
		
		poz_x *= poz_x;
		poz_z *= poz_z;
		
		return Mathf.Sqrt(poz_x + poz_z);
	}
	
	public static Position operator+(Position pos1, Position pos2){
		float x = pos1.x + pos2.x;
		float y = pos1.y + pos2.y;
		float z = pos1.z + pos2.z;


		return new Position(x,y,z);
	}
	
	public static Position operator-(Position pos1, Position pos2){
		float x = pos1.x - pos2.x;
		float y = pos1.y - pos2.y;
		float z = pos1.z - pos2.z;


		return new Position(x,y,z);
	}
	
	public static Position operator*(Position pos1, float scalar){
		float x = pos1.x * scalar;
		float y = pos1.y * scalar;
		float z = pos1.z * scalar;
		
		return new Position(x,y,z);		
	}
	
	public static Position operator/(Position pos1, float scalar){
		float x = pos1.x / scalar;
		float y = pos1.y / scalar;
		float z = pos1.z / scalar;
		
		return new Position(x,y,z);		
	}
	
	public Position getClone(){
		return new Position(x,y,z);
	}

	public override string ToString()
	{
		return "Position(" + x + "," +z + ")";
	}

	public Vector3 toVector3(){
		return new Vector3(x,y,z);
	}
}
