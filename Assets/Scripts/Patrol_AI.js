#pragma strict

var target : Transform;
var gravity : float = 20;
var moveSpeed : float = 1;  // chase speed
var shuffleSpeed : float = 0.01; //Forward moving while stationary attack
var rotSpeed : float = 45;  // speed to turn to the player (degrees/second)
var attackDistance : float = 1.5;  // attack distance
var AttackRange : float = 3;
var detectRange : float = 20;  // detection distance
//var animatedPlayerModel : ;
var anim : Animator;
var hp : int; 

private var transf : Transform;
private var character: CharacterController; 

function Start () { 
	hp = 1; 
    if (!target) target = GameObject.FindWithTag ("Player").transform; 
    transf = transform;
    character = GetComponent(CharacterController);
    anim = GetComponent("Animator");
}

function Update(){
	//animatedPlayerModel.transform.localPosition = Vector3.zero;
	transform.position.z = 0;
	if (hp <= 0)
	{
		anim.SetBool("Dead", true); 
		//Spawn soul. Only once, this is update method so don't spawn 100 during destruction time. 
		Destroy(gameObject, 2.0f);
	}
    else if (target){
        var tgtDir = target.position - transf.position;
        var tgtDist = tgtDir.magnitude; // get distance to the target
        var hit : RaycastHit;
        if ((Physics.Raycast(transf.position, tgtDir, hit, detectRange))&&hit.collider.tag.Equals("Player")){
            var moveDir = target.position - transf.position;
            moveDir.y = 0;  // prevents enemy tilting
            var rot = Quaternion.FromToRotation(Vector3.forward, moveDir);
            transf.rotation = Quaternion.RotateTowards(transf.rotation, rot, rotSpeed * Time.deltaTime);
//            Debug.Log(tgtDist);
            if (tgtDist <= attackDistance){  // if dist <= attackDistance: attack
                // do your attack here
                anim.SetBool("Attacking", true);
                anim.SetBool("Walking", false);
                anim.SetBool("WalkingAttack", false);
                MoveCharacter(moveDir, shuffleSpeed); //Continue moving and pushing for hitbox ease
                //anim.Play("Attack");
                print("Attack!");
            }
            else if((tgtDist <= AttackRange) && (tgtDist > attackDistance))
            {//Replacing charge logic with walk & attack area
            	//anim.SetBool("Attacking", true);
            	anim.SetBool("Attacking", false);
            	anim.SetBool("Walking", true);
            	anim.SetBool("WalkingAttack", true);
            	MoveCharacter(moveDir, moveSpeed);
            	print("Attack Region");
            }
            else {  // if attackDistance < dist < detectRange: chase the player
                // Move towards target
                anim.SetBool("Attacking", false);
                anim.SetBool("Walking", true);
                anim.SetBool("WalkingAttack", false);
                MoveCharacter(moveDir, moveSpeed);
            }
        }
        else {
            // stays in idle mode if can't see target
            anim.SetBool("Attacking", false);
            anim.SetBool("WalkingAttack", false);
            //anim.SetBool("Walking", false);
            Idle(); 
        }
    }
}

var walkSpeed : float = 1.0f; //None of these values can be changed. 
var travelTime : float = 8.0f; //Animator doesn't seem to like cutting animation midway.
var idleTime : float = 4.0f; //So you get gliding enemies unless times match up in multiples. 
var rndAngle = 180;  // enemy will turn +/- rndAngle
var delayTime : float = 4.167f;

private var timeToNewDir = 0.0; //For calcs, don't change
private var turningTime = 0.0; //For calcs, don't change
private var turn: float;

function Idle () { 
	//anim.Play("Idle");
    // Walk around and pause in random directions 
    if (Time.time > timeToNewDir) { // time to change direction?
        turn = (Random.value > 0.5)? rndAngle : -rndAngle; // choose new direction
        turningTime = Time.time + idleTime; // will stop and turn during idleTime...
        timeToNewDir = turningTime + travelTime + delayTime;  // and travel during travelTime 
    }
    if (Time.time < turningTime){ // rotate during idleTime...
        transform.Rotate(0, turn*Time.deltaTime/idleTime, 0);
        anim.SetBool("Walking", true);
    } else if (Time.time > turningTime && Time.time < (timeToNewDir-delayTime)){  // and travel until timeToNewDir
    	anim.SetBool("Walking", true);
        MoveCharacter(transform.forward, walkSpeed);
    }
    else{
    	anim.SetBool("Walking", false);
    	anim.SetBool("Attacking", false);
    }
}

function MoveCharacter(dir: Vector3, speed: float){
	anim.SetBool("Walking", true);
    var vel = dir.normalized * speed;  // vel = velocity to move 
    // clamp the current vertical velocity for gravity purpose
    vel.y = Mathf.Clamp(character.velocity.y, -30, 2); 
    vel.y -= gravity * Time.deltaTime;  // apply gravity
    character.Move(vel * Time.deltaTime);  // move
}
function die()
{
	anim.SetBool("Dead", true);
	hp = 0; 
}