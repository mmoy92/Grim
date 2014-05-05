var imageFolderName = "Videos/Test";
var MakeTexture = true;
var pictures = new Array();
var loop = false; 
var counter = 0;
var Film = true;
var PictureRateInSeconds:float = 1;
var delay = 1;

private var nextPic:float = 0;
 
function Start () {
	if(Film == true){
    	PictureRateInSeconds = 0.04166666666666666666;
	}
    var textures : Object[] = Resources.LoadAll(imageFolderName);
    Debug.Log("Number found: " + textures.Length);
    for(var i = 0; i < textures.Length; i++){
	    Debug.Log("found" + textures[i]);
	    pictures.Add(textures[i]);
    }
    nextPic = Time.time + delay;
}
 
function Update () {
    if(Time.time > nextPic){
     	if (!audio.isPlaying) {
 			audio.Play();
 		}
	    nextPic = Time.time + PictureRateInSeconds;
	    counter += 1;
	    if(counter >= pictures.length){
	    	audio.Stop();
		    //Debug.Log("fertig");
		    if(loop){
		    	counter = 0;
		    } else {
		    	enabled = false;
		    }
		   
    	}
    	
    	if(MakeTexture){ 
    		guiTexture.texture = pictures[counter];
    		//renderer.material.mainTexture = pictures[counter]; 
    	}
    }
}