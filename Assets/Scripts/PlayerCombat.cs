using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour {
	public Transform melee;
	public Transform lightSpear;
	public Transform evilSplode;
	bool mAttacking						= false;	//Are we currently attacking
	bool mCanAttack						= true;		//Can the player click the button to do the next attack?
	float mAttackTimer					= 0;		//Current time until the current attack is done
	public float ultimateTimer		    = 0;		//Current time until next use of ultimate.
	public int mCurrentAttack			= 0;		//Current attack type being performed (0,1, or 2)
	public float AttackALength		 	= 0.23f;	//The duration of attack A
	public float AttackA_velY			= 5;		//The vertical thrust of attack A
	
	public float AttackBLength 			= 0.29f;	//The duration of attack B
	public float AttackB_velY			= 5;		//The vertical thrust of attack B
	
	public float AttackCLength 			= 0.21f;	//The duration of attack C
	public float AttackC_velY			= 5;		//The vertical thrust of attack C

	public AudioClip slashClip;						// Audio clip to play when player attacks

	private bool isRight;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		ultimateTimer += Time.deltaTime;

		if (mAttacking) 
		{
			mAttackTimer -= Time.deltaTime;

			if(mAttackTimer <= 0.15) //Allows the player to click near the end of an attack animation to do the next attack
			{
				mCanAttack = true;
				
				if(mAttackTimer<= 0) //Attack animation is done, time to revert back to normal state
				{
					mAttacking = false;
					if(mCurrentAttack == 1) //Reset the attack combo
					{
						mCurrentAttack = 0;
					}

					SendAnimMessage("FinishedAttack");

				}
			}
		}
	}
	public void Attack(Vector3 vel, bool mGoingRight)
	{
		if (mCanAttack)
		{
			mCanAttack = false;
			mAttacking = true;

			if(mCurrentAttack > 3){
				mCurrentAttack = 0;
			}

			mCurrentAttack += 1;
			
			if(mCurrentAttack == 1)
			{
				mAttackTimer = AttackALength + 0.2f;
				SendAnimMessage("StartAttackA");
				vel.y = AttackA_velY;
			} else if(mCurrentAttack == 2)
			{
				mAttackTimer = AttackBLength + 0.2f;
				SendAnimMessage("StartAttackB");
				vel.y = AttackB_velY;
			} else if(mCurrentAttack == 3)
			{
				mAttackTimer = AttackCLength + 0.2f;
				SendAnimMessage("StartAttackC");
				vel.y = AttackC_velY;
				
				mCurrentAttack = 0;
			} 
			rigidbody.velocity = vel; //Set the vertical thrust
			
			if(mGoingRight)			//Set the horizontal thrust
			{
				rigidbody.velocity += 4 * Vector3.right;
			} else {
				rigidbody.velocity += 4 * Vector3.left;
			}

			isRight = mGoingRight;
			
			
		}

	}
	public void EvilAttack(Vector3 vel, bool mGoingRight)
	{
		if (mCanAttack && ultimateTimer >= 5f)
		{
			ultimateTimer = 0.0f;
			mCanAttack = false;
			mAttacking = true;
			
			mCurrentAttack = 4;
			
			mAttackTimer = 0.5f;

			vel.y = -20;
			rigidbody.velocity = vel; 

			SendAnimMessage("StartEvilSplode");
			
			isRight = mGoingRight;
			
		}
		
	}
	public void GoodAttack(Vector3 vel, bool mGoingRight)
	{
		if (mCanAttack && ultimateTimer >= 5f)
		{
			ultimateTimer = 0.0f;
			mCanAttack = false;
			mAttacking = true;
			
			mCurrentAttack = 5;

			mAttackTimer = 0.5f;
			SendAnimMessage("StartGoodSpear");
			
			isRight = mGoingRight;
			
		}
		
	}
	public void DealAttack()
	{
		Vector3 swipeSpawn = transform.position;

		if (mCurrentAttack == 4) {//Evil splode


			Instantiate (evilSplode, swipeSpawn, Quaternion.Euler (new Vector3 (0, 0, 0)));

			
		} else if (mCurrentAttack == 5) {//Light spear
			swipeSpawn.y += 1.2f;
			if (isRight) {
				swipeSpawn.x += 5.0f;
				Instantiate (lightSpear, swipeSpawn, Quaternion.Euler (new Vector3 (0, 0, 0)));
			} else {
				swipeSpawn.x -= 5.0f;
				Instantiate (lightSpear, swipeSpawn, Quaternion.Euler (new Vector3 (0, 0, 180f)));
			}

		} else {//Other attacks
			swipeSpawn.y += 1.0f;
				if (isRight) {
						swipeSpawn.x += 2.5f;
						MeleeSwipe swipeInstance = Instantiate (melee, swipeSpawn, Quaternion.Euler (new Vector3 (0, 0, 0))) as MeleeSwipe;
				} else {
						swipeSpawn.x -= 2.5f;
						MeleeSwipe swipeInstance = Instantiate (melee, swipeSpawn, Quaternion.Euler (new Vector3 (0, 0, 180f))) as MeleeSwipe;
				}
		}
	}
	//send a message to all other scripts to trigger for example the animations
	void SendAnimMessage(string message)
	{
		SendMessage(message, SendMessageOptions.DontRequireReceiver);
	}
	public bool IsAttacking() {return mAttacking;}
	public bool CanAttack() {return mCanAttack;}
}
