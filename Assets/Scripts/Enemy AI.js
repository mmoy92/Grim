#pragma strict

var deathEffect:Transform;
var Soul:GameObject;
var target : Transform;    
var lookAtDistance = 15.0;
var attackRange = 10.0;
var moveSpeed = 15.0;
var damping = 6.0;
var health = 50;

private var isItAttacking = false;



function Update() 
{
	transform.LookAt(target);
	
	var distance = Vector3.Distance(target.position, transform.position);
	
	if(distance < lookAtDistance && distance > attackRange)
	{
		isItAttacking = false;
		renderer.material.color = Color.yellow;
	}   
	else if (distance > lookAtDistance)
	{
		renderer.material.color = Color.green; 
	}
	
	if(distance < attackRange)
	{
		isItAttacking = true;
    	renderer.material.color = Color.red;
    	transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    	//rigidbody.velocity = (target.transform.position - transform.position).normalized    * (moveSpeed );
	}
}


function getHurt(amt:int)
{
	health -= amt;
	if(health <=0)
	{
		die();
	}
	
}
function knockBack()
{
	transform.Translate(Vector3.back * 0.5f);

}

function die()
{
	GameObject.FindGameObjectWithTag("MainCamera").GetComponent("FollowCam2D").SendMessage("SlowMoShake");

	Instantiate(deathEffect, transform.position, Quaternion.Euler(new Vector3(0,0,0)));
	Instantiate (Soul, transform.position, Quaternion.identity);
	Destroy (gameObject);
}