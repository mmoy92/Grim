#pragma strict

var target : Transform;
var fireball : Transform;
var bat : Transform;

var health: int = 100;
var attackDistance = 10.0;
var attackTimer : float;
var attackDif = 1f;
var attackMode: int = 0;
var stage1 : int = 0;
var stage2 : int = 0;

private var transf : Transform;
private var character: CharacterController;
var anim : Animator;

function Start () {
	if (!target) target = GameObject.FindWithTag ("Player").transform; 
	transf = transform;
	attackTimer = Time.time;
    character = GetComponent(CharacterController);
    anim = GetComponent("Animator");
}

function getHurt(amt : int)
{
	print("HURT!!!!");
	health -= amt;	
}

function knockBack()
{
	//Empty, just receiver for message
}

function Update () {
	if(health <= 0)
	{
		Die();
	}
	else if (target){
		var tgtDir = target.position - transf.position;
		var spawnLoc = transf.position + new Vector3(-6, 5, 0);
        var tgtDist = tgtDir.magnitude;
        //Stage 1 - 1 bat, start fireballs
        if(tgtDist < attackDistance)
        {
        	if(attackMode == 0)
        	{
        		Instantiate (bat, spawnLoc, Quaternion.identity);
        	}
        	attackMode = 1;
        	anim.SetBool("Attacking", true); 
        }
        if(attackMode == 1)
        {
        	
        	if(Time.time - attackTimer > attackDif)
        	{
        		Instantiate (fireball, spawnLoc, Quaternion.identity);
        		attackTimer = Time.time;
        	}
        	//Stage 2 - 2 bats, fast fireballs
        	if(health <= 70 && stage1 == 0)
        	{
        		Instantiate (bat, spawnLoc, Quaternion.identity);
        		Instantiate (bat, spawnLoc, Quaternion.identity);
        		stage1 = 1;
        		//Fire faster
        		attackDif *= 0.9;
        	}
        	//Stage 3 - 3 bats, faster fireballs
        	if(health <= 40 && stage2 == 0)
        	{
        		Instantiate (bat, spawnLoc, Quaternion.identity);
        		Instantiate (bat, spawnLoc, Quaternion.identity);
        		Instantiate (bat, spawnLoc, Quaternion.identity);
        		stage2 = 1;
        		//More fireballs!
        		attackDif *= 0.9;
        	}
        }
	}
}

function Die () {
	anim.SetBool("Alive", false); 
}