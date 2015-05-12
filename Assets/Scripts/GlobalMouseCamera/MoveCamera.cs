using UnityEngine;

// this class manages the movement of the camera on the level editor

public class MoveCamera : MonoBehaviour
{
	public GUISkin currentSkin;
	//speed of the camera movement and boolean to store the result of the check if its 2d or 3d mode
    public float speed = 5;
	private bool was2d = false;
	private bool forward = false;
	private bool backward = false;
	private bool left = false;
	private bool right = false;
	private bool zoomIn = false;
	private bool zoomOut = false;
	
    void Update()
    {
		forward = Input.GetKey(KeyCode.W);
		backward = Input.GetKey(KeyCode.S);
		right = Input.GetKey(KeyCode.D);
		left = Input.GetKey(KeyCode.A);
		//zoomIn = Input.GetKey(KeyCode.Q);
		//zoomOut = Input.GetKey(KeyCode.E);
		
		// reset the camera position to 2d mode default position when entering 2d mode
		if(GameManager.is2DMode && !was2d)
		{
			Vector3 tempPos = transform.position;
			tempPos.z = -10f;
			transform.position = tempPos;		
		}
		
		//checks if the load and save dialog are not active
        if (!GameManager.isLoadDialogActive && !GameManager.isSaveDialogActive)
        {
			//2d mode camera controls
            if(GameManager.is2DMode)
			{				
            	if (forward)
	            {
	                transform.Translate(Vector3.up * (speed * Time.deltaTime));
	            }
	            
	            if (backward)
	            {
	                transform.Translate(Vector3.down * (speed * Time.deltaTime));
	            }
	            
	            if (left)
	            {
	                transform.Translate(Vector3.left * (speed * Time.deltaTime));
	            }
	            
	            if (right)
	            {
	                transform.Translate(Vector3.right * (speed * Time.deltaTime));
	            }
	            
	            if (zoomIn)
	            {
	                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
	            }
	            
	            if (zoomOut)
	            {
	                transform.Translate(Vector3.back * (speed * Time.deltaTime));
				}
            } 
			
			//3d mode camera controls
			else
			{
				if (forward)
	            {
	                transform.Translate(Vector3.forward * (speed * Time.deltaTime));
	            }
	            
	           if (backward)
	            {
	                transform.Translate(Vector3.back * (speed * Time.deltaTime));
	            }
	            
	            if (left)
	            {
	                transform.Translate(Vector3.left * (speed * Time.deltaTime));
	            }
	            
	            if (right)
	            {
	                transform.Translate(Vector3.right * (speed * Time.deltaTime));
	            }
			}
        }
		
		//store the current level editor mode state for later use
		was2d = GameManager.is2DMode;
    }
}