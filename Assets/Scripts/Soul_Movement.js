#pragma strict


var target : Transform;    
var lookAtDistance = 15.0;
var attackRange = 10.0;
var moveSpeed = 6.0;
var damping = 6.0;
var health = 10;
var good = true;

private var isItAttacking = false;
function Start()
{
	target = GameObject.Find("Player").transform;
}
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
		
		if (good){
			//transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.World);
			if(target.transform.position.x - transform.position.x < 0)
         	{
          			transform.Translate(moveSpeed*Time.deltaTime,0,0, Space.World);
         	}
        	else if(target.transform.position.x - transform.position.x > 0)
         	{
         	 		transform.Translate(-moveSpeed*Time.deltaTime,0,0, Space.World);
         	}
			/*var targetRelative = transform.InverseTransformPoint(target.transform.position);
		
			if (targetRelative.x > 0)
			{
				transform.Translate(moveSpeed * Time.deltaTime, 0, 0, Space.World);
			}
			else if (targetRelative.x < 0)
			{
				transform.Translate(-1 * moveSpeed * Time.deltaTime, 0, 0, Space.World);
			}*/
		}
	}
}

function getHurt(amt:int)
{
	health -= amt;
	//rigidbody.AddForce(Vector3(10.0f,0,0));
}