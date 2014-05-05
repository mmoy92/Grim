#pragma strict

var levelName : int;

function Start () {

}

function Update () {

}

function OnTriggerEnter(other : Collider) {
	Application.LoadLevel(levelName);
}