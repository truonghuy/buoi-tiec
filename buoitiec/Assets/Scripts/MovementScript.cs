using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {

	
//	private bool isTou = false; //checks touch for our texture
//	private int isFingTou = -1; //which finger contact with screen
//

	private int speed = 5;
	private int gravity = 5;
	private CharacterController cc;



	// Use this for initialization
	void Start () {
		cc = (CharacterController)GetComponent("CharacterController");
	}
	
	// Update is called once per frame
	void Update () {
		cc.Move(new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 
		                -gravity * Time.deltaTime, 
		                Input.GetAxis("Vertical") * speed * Time.deltaTime));
	}
//
//
//
//	void Update() {
//		if(Input.touchCount > 0) { //count touches
//			for (int i = 0; i < Input.touchCount; i++) { //see every touch
//				//TouchPhase.Began - Works only once in case of a screen contact 
//				if (Input.GetTouch(i).phase == TouchPhase.Began && !isTou) { // finger touch screen
//					if (this.guiTexture.HitTest(Input.GetTouch(i).position)) { //Touch texture
//						isTou = true;
//						isFingTou = Input.GetTouch(i).fingerId;
//					}
//				}
//				if (isTou) {
//					myMovePlayer();
//				}
//				//End of touch
//				if (Input.GetTouch(i).phase == TouchPhase.Ended && Input.GetTouch(i).fingerId == isFingTou) {
//					isTou = false;
//					isFingTou = -1;
//				}
//			}
//		}
//	}
//	
//	public void myMovePlayer() {
//		myPlayer.Translate(Vector2.right * myCharacterSpeed * Time.deltaTime);
//	}
}
