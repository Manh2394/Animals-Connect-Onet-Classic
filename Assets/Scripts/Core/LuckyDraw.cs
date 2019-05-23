using UnityEngine;
using System.Collections;

public class LuckyDraw : MonoBehaviour {
	public HingeJoint2D joint;
	private int reduceSpeed = 5;
	JointMotor2D motor;
	public int drawedCell;
	// Use this for initialization
	void Start () {
		motor = joint.motor;
		StartCoroutine (UpdateMotorSpeed ());
	}
	IEnumerator UpdateMotorSpeed ()
	{
		yield return new WaitForSeconds (2f);
		while (motor.motorSpeed > 0)
		{
			if (motor.motorSpeed > 1000)
			{
				motor.motorSpeed -= reduceSpeed * Random.Range(1, 100);			
			} else
			{
				motor.motorSpeed -= reduceSpeed * Random.Range(1, 4);			
			}
			if (motor.motorSpeed <= 0) {
				motor.motorSpeed = 0;
				Debug.LogError (transform.localEulerAngles.z);
				drawedCell = (int) (transform.localEulerAngles.z / 60f) + 1;
			}
			joint.motor = motor;
			yield return new WaitForSeconds (0.01f);
		}
	}
	
	// Update is called once per frame
	// void Update () {
	// 	if (motor.motorSpeed > 1000)
	// 	{
	// 		motor.motorSpeed -= reduceSpeed * Random.Range(1, 5);			
	// 	} else
	// 	{
	// 		motor.motorSpeed -= reduceSpeed * Random.Range(1, 3);			
	// 	}
	// 	if (motor.motorSpeed < 0)
	// 		motor.motorSpeed = 0;
	// 	joint.motor = motor;
	// }
}
