#pragma strict


var target : Transform;    
var lookAtDistance = 15.0;
var attackRange = 10.0;
var moveSpeed = 5.0;
var damping = 6.0;
var health = 10;
public var Soul:GameObject;

private var isItAttacking = false;

function Update() 
{
	//transform.LookAt(target);
	
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
    	//transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
	}
}

function OnCollisionEnter (other : Collision)
{
    if(other.gameObject.name == "Player")
    {
       health = health - 10;  // Reworked line
    }
	if (health < 0){
       Destroy (gameObject);
       var new_Soul : GameObject;
       new_Soul = Instantiate (Soul, Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), 
       			Quaternion.identity);
    }
}

function getHurt(amt:int)
{
	health -= amt;
	//rigidbody.AddForce(Vector3(10.0f,0,0));
}