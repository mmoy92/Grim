#pragma strict

var target : Transform;
var gravity : float = 100;
var moveSpeed : float = 6;  // chase speed
var chargeSpeed : float = 7; //Charge speed for last part of attack
var rotSpeed : float = 180;  // speed to turn to the player (degrees/second)
var attackDistance : float = 7;  // attack distance
var chargeDistance : float = 15;
var detectRange : float = 20;  // detection distance

private var transf : Transform;
private var character: CharacterController; 

function Start () { 
    if (!target) target = GameObject.FindWithTag ("Player").transform; 
    transf = transform;
    character = GetComponent(CharacterController);
}

private var switchRound: int = 60;
var attackTurnFrames: int = 60; 
function Update(){
	transform.position.z = 0;
    if (target){
        var tgtDir = target.position - transf.position;
        var tgtDist = tgtDir.magnitude; // get distance to the target
        var hit : RaycastHit;
        if ((Physics.Raycast(transf.position, tgtDir, hit, detectRange))&&hit.collider.tag.Equals("Player")){
            var moveDir = target.position - transf.position;
            moveDir.y = 0;  // prevents enemy tilting
            if (switchRound >= 0)//Random.value > .5 &&
            {
            	switchRound--; 
            	moveDir.x = -moveDir.x;
            }
            else if (switchRound == (attackTurnFrames-(2*attackTurnFrames)))
            {
            	switchRound = attackTurnFrames;
            }
            else
            {
            	switchRound--;
            }
            var rot = Quaternion.FromToRotation(Vector3.forward, moveDir);
            transf.rotation = Quaternion.RotateTowards(transf.rotation, rot, rotSpeed * Time.deltaTime);
            if (tgtDist <= attackDistance){  // if dist <= attackDistance: stop and attack
                // do your attack here
                //print("Attack!");
            }
            else if((tgtDist <= chargeDistance) && (tgtDist > attackDistance))
            {
            	MoveCharacter(moveDir, chargeSpeed);
            }
            else {  // if attackDistance < dist < detectRange: chase the player
                // Move towards target
                MoveCharacter(moveDir, moveSpeed);
            }
        }
        else {
            // stays in idle mode if can't see target
            Idle(); 
        }
    }
}

var dropTime = 3.0;
var thrustTime = 1.0;
var upThrust = 200.0; 
var walkSpeed = 3.0; 
var travelTime = 2.0; 
var idleTime = 0.2;
var rndAngle = 180;  // enemy will turn +/- rndAngle

private var timeToNewThrust = 0.0;
private var timeToNewDir = 0.0;
private var turningTime = 0.0;
private var turn: float;
private var thrusting : boolean = false;
var verticalHover = 2;
private var hoverDir = (Random.value > 0.5)? 1 : -1; // choose new direction

function Idle () { 
    // Walk around and pause in random directions 
    if (Time.time > timeToNewDir) { // time to change direction?
        turn = (Random.value > 0.5)? rndAngle : -rndAngle; // choose new direction
        hoverDir = (Random.value > 0.5)? 1 : -1; // choose new direction
        turningTime = Time.time + idleTime; // will stop and turn during idleTime...
        timeToNewDir = turningTime + travelTime;  // and travel during travelTime 
    }
    if (Time.time < turningTime){ // rotate during idleTime...
        transform.Rotate(0, turn*Time.deltaTime/idleTime, 0);
    } else {  // and travel until timeToNewDir
        MoveCharacter(transform.forward, walkSpeed);
    }
}

var minAltitude : float;
minAltitude = 2.0f;
function MoveCharacter(dir: Vector3, speed: float){
	var vel = dir.normalized * speed;  // vel = velocity to move 
	if(Time.time > timeToNewThrust) //If due for new thrust, set thrust and reset time
	{
		thrusting = true;
		timeToNewThrust = Time.time + dropTime + thrustTime;
	} 
	if (thrusting)//If thrusting, go up, check if into droptime
	{
		thrusting = true;
		vel.y += upThrust/thrustTime * Time.deltaTime;
		if ((timeToNewThrust - dropTime) < Time.time)
		{	
			thrusting = false; 
		}
	}
	else //Not thrusting-> droptime
	{
		vel.y -= gravity * Time.deltaTime;
	}
    //vel.y += hoverDir * verticalHover; 
    // clamp the current vertical velocity for gravity purpose
    //vel.y = Mathf.Clamp(character.velocity.y, -2, 2); 
    if (Physics.Raycast(transf.position, -Vector3.up, minAltitude))
    {
    	vel.y = upThrust/thrustTime * Time.deltaTime;
    }
    character.Move(vel * Time.deltaTime);  // move
}
var batDamage :int  = 1; 
function OnCollisionEnter(other : Collision)
{
	//Debug.Log("Hit");
	if(other.gameObject.tag.Equals("Player"))
	{
		//Debug.Log("Damage");
		other.gameObject.GetComponent("grimInfo").SendMessage("Damage", batDamage);
	}//This must be done with "" and sendmsg because griminfo is a .cs file. 
}