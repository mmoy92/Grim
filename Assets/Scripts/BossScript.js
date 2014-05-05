#pragma strict

var target : Transform;
var fireball : Transform;
var bat : Transform;

var health: int = 100;
var attackDistance = 8.0;
var attackTimer = Time.time;
var attackDif = 5f;
var attackMode: int = 0;
var stage1 : int = 0;
var stage2 : int = 0;

private var transf : Transform;
private var character: CharacterController;

function Start () {
	if (!target) target = GameObject.FindWithTag ("Player").transform; 
	transf = transform;
    character = GetComponent(CharacterController);
}

function getHurt(amt:int)
{
	health -= amt;	
}

function Update () {
	if(health <= 0)
	{
		Die();
	}
	else if (target){
		var tgtDir = target.position - transf.position;
        var tgtDist = tgtDir.magnitude;
        //Stage 1 - 1 bat, start fireballs
        if(tgtDist < attackDistance)
        {
        	if(attackMode == 0)
        	{
        		Instantiate (bat, transform.position, Quaternion.identity);
        	}
        	attackMode = 1;
        }
        if(attackMode == 1)
        {
        	
        	if(Time.time - attackTimer > attackDif)
        	{
        		Instantiate (fireball, transform.position, Quaternion.identity);
        		attackTimer = Time.time;
        	}
        	//Stage 2 - 2 bats, fast fireballs
        	if(health <= 7 && stage1 == 0)
        	{
        		Instantiate (bat, transform.position, Quaternion.identity);
        		Instantiate (bat, transform.position, Quaternion.identity);
        		stage1 = 1;
        		//Fire faster
        		attackDif = 4;
        	}
        	//Stage 3 - 3 bats, faster fireballs
        	if(health <= 4 && stage2 == 0)
        	{
        		Instantiate (bat, transform.position, Quaternion.identity);
        		Instantiate (bat, transform.position, Quaternion.identity);
        		Instantiate (bat, transform.position, Quaternion.identity);
        		stage2 = 1;
        		//More fireballs!
        		attackDif = 3.5;
        	}
        }
	}
}

function Die () {
	Destroy (gameObject);
}