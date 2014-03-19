using UnityEngine;

public class HeroKeymap : MonoBehaviour
{
	// Movement
    public KeyCode forwardKey = KeyCode.W;
    public KeyCode backwardKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
	
	// Jump
    public KeyCode jumpKey = KeyCode.Space;
	
	// Walk,Sneak,Sprint toggle
	public KeyCode walkToggleKey = KeyCode.X;
	public KeyCode sneakToggleKey = KeyCode.V;
    public KeyCode sprintToggleKey = KeyCode.LeftShift;
	
	// Climb or stand up
    public KeyCode climbOrStandKey = KeyCode.JoystickButton2;
	
    // Next,Ready,Reload Weapon
	public KeyCode nextWeaponKey = KeyCode.Q;
    public KeyCode readyWeaponKey = KeyCode.C;
    public KeyCode reloadKey = KeyCode.R;
	
	// Camera modes
    public KeyCode firstPersonCameraKey = KeyCode.Alpha1;
    public KeyCode orbitCameraKey = KeyCode.Alpha2;
    public KeyCode thirdPersonCameraKey = KeyCode.Alpha3;
	
	// Trow,Pull,Push,Dance
    public KeyCode throwKey = KeyCode.T;
    public KeyCode pullLeverKey = KeyCode.B;
    public KeyCode pushButtonKey = KeyCode.N;
    public KeyCode dance1Key = KeyCode.G;
    public KeyCode dance2Key = KeyCode.H;
    public KeyCode dance3Key = KeyCode.J;
}
