#pragma strict

var target : Transform;
var gravity : float = 20;
var moveSpeed : float = 6;  // chase speed
var chargeSpeed : float = 12; //Charge speed for last part of attack
var rotSpeed : float = 180.0f;  // speed to turn to the player (degrees/second)
var attackDistance : float = 2;  // attack distance
var attackRange : float = 4; 
var chargeDistance : float = 10;
var detectRange : float = 200;  // detection distance
var anim : Animator;
//var enemyMask = 1 << 2; 
//enemyMask = ~enemyMask;

private var transf : Transform;
private var character: CharacterController; 

function Start () { 
    if (!target) target = GameObject.FindWithTag ("Player").transform; 
    transf = transform;
    character = GetComponent(CharacterController);
    anim = GetComponent("Animator");
}

function Update(){
	transform.position.z = 0;
    if (target){
        var tgtDir = target.position - transf.position;
        var tgtDist = tgtDir.magnitude; // get distance to the target
        var hit : RaycastHit;
        //Debug.Log(tgtDist);
        if ((Physics.Raycast((transf.position+Vector3(0.0f,0.5f,0.0f)), tgtDir, hit, detectRange))&&hit.collider.tag.Equals("Player")){
            //Debug.DrawLine(transf.position, hit.point, Color.red);
            var moveDir = target.position - transf.position;
            moveDir.y = 0;  // prevents enemy tilting
            var rot = Quaternion.FromToRotation(Vector3.forward, moveDir);
 //           anim.SetBool("Turning", true);
            transf.rotation = Quaternion.RotateTowards(transf.rotation, rot, rotSpeed * Time.deltaTime);
  //          anim.SetBool("Turning", false); 
            if (tgtDist <= attackDistance){  // if dist <= attackDistance: stop and attack
                anim.SetBool("Attacking", true); 
                anim.SetBool("Walking", false);
                anim.SetBool("Running", false); 
                // do your attack here
                print("Attack!");
            }
            else if((tgtDist <= chargeDistance) && (tgtDist > attackDistance) && (tgtDist <= attackRange))
            {
            	//anim.SetBool("RunningAttack", true);
            	anim.SetBool("Attacking", false);
            	anim.SetBool("Running", true);
            	anim.SetBool("Walking", false); 
            	MoveCharacter(moveDir, chargeSpeed);
            }
            else if ((tgtDist <= chargeDistance) && (tgtDist > attackDistance))
            {
            	anim.SetBool("Attacking", false); 
            	anim.SetBool("Running", true);
            	anim.SetBool("Walking", false); 
            	MoveCharacter(moveDir, chargeSpeed);
            }
            else {  // if attackDistance < dist < detectRange: chase the player
                anim.SetBool("Walking", true); 
                anim.SetBool("Attacking", false);
                anim.SetBool("Running", false); 
                MoveCharacter(moveDir, moveSpeed);
            }
        }
        else {
            // stays in idle mode if can't see target
            //Debug.Log("IDLING");
            Idle(); 
        }
    }
}

var walkSpeed : float = 3.0f; 
var travelTime : float = 4.068f; 
var idleTime : float = 2.834f;
var rndAngle = 180;  // enemy will turn +/- rndAngle

private var timeToNewDir = 0.0;
private var turningTime = 0.0;
private var turn: float;

function Idle () { 
    // Walk around and pause in random directions 
    if (Time.time > timeToNewDir) { // time to change direction?
        turn = (Random.value > 0.5)? rndAngle : -rndAngle; // choose new direction
        turningTime = Time.time + idleTime; // will stop and turn during idleTime...
        timeToNewDir = turningTime + travelTime;  // and travel during travelTime 
    }
    if (Time.time < turningTime){ // rotate during idleTime...
//    	anim.SetBool("Turning", true);
 //   	anim.SetBool("Walking", false); 
//      anim.SetBool("Running", false); 
//		anim.SetBool("Turning", true); 
        transform.Rotate(0, turn*Time.deltaTime/idleTime, 0);
    } else {  // and travel until timeToNewDir
 //   	anim.SetBool("Turning", false); 
    	anim.SetBool("Walking", true); 
//    	anim.SetBool("Turning", false); 
		anim.SetBool("Attacking", false);
		anim.SetBool("Running", false); 
        MoveCharacter(transform.forward, walkSpeed);
    }
}

function MoveCharacter(dir: Vector3, speed: float){
    var vel = dir.normalized * speed;  // vel = velocity to move 
    // clamp the current vertical velocity for gravity purpose
    vel.y = Mathf.Clamp(character.velocity.y, -200, 2); 
//    Debug.Log(vel.y);
    vel.y -= gravity;// * Time.deltaTime;  // apply gravity
//    Debug.Log(vel.y);
    character.Move(vel * Time.deltaTime);  // move
}

function die()
{
	anim.SetBool("Alive", false);
	target = null;
}