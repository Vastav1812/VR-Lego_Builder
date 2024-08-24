using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public static float speed = 8;
    private Vector3 moveDirection;
    private CharacterController controller;
    public static bool lego_selected;
    public static Stack<Lego> placed_legos;
    public GameObject avatar;
    public GameObject cameraParent;

   

    

    void Start()
    {
        placed_legos = new Stack<Lego>();
        controller=GetComponent<CharacterController>();
        lego_selected = false;
       
    }

    void Update()
    {
        float y = Camera.main.transform.eulerAngles.y;
        avatar.transform.eulerAngles = new Vector3(avatar.transform.eulerAngles.x, y, avatar.transform.eulerAngles.z);

        if(MenuController.scale != 0)
        {
            //3rd person pov
            y = (y > 180) ? y - 360 : y;
            float cameraHeight = cameraParent.transform.position.y;
            cameraParent.transform.position = avatar.transform.position + new Vector3(Mathf.Sin(Mathf.PI * y / 180) * MenuController.scale * -4, cameraHeight, Mathf.Cos(Mathf.PI * y / 180) * MenuController.scale * -4) + new Vector3(0, 0, 1.03f);
            cameraParent.transform.position = new Vector3(cameraParent.transform.position.x, cameraHeight, cameraParent.transform.position.z);
        }
        else if (MenuController.scale == 0)
        {
            //1st person pov
            float avatary = avatar.transform.position.y;
            avatar.transform.position = cameraParent.transform.position + Camera.main.transform.forward * - 2f;
            avatar.transform.position = new Vector3(avatar.transform.position.x, avatary, avatar.transform.position.z);
        }
        
        float xDir = Input.GetAxisRaw("Horizontal");
        float yDir = Input.GetAxisRaw("Vertical");
        if(!MenuController.menuMode)
        {
            
            // controls player movement
            if(Input.GetButton("OK"))
            {
                moveDirection=new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0);
                moveDirection = Camera.main.transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                moveDirection.z = 0.0f;
                controller.Move(moveDirection * Time.deltaTime);
                
            }
            else
            {
                moveDirection=new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));
                moveDirection = Camera.main.transform.TransformDirection(moveDirection);
                moveDirection *= speed;
                moveDirection.y = 0.0f;
                controller.Move(moveDirection * Time.deltaTime);
            }
        }


    }
   
}
