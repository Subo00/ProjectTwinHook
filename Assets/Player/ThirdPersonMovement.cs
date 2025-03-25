using UnityEngine;
using UnityEngine.Playables;

public class ThirdPersonMovement : MonoBehaviour{

    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [SerializeField] private string horizontal = "Horizontal";
    [SerializeField] private string verticals = "Vertical";

    //protected bool isUIActive = false;
    //protected PlayerAnimationController animationController;
    protected Vector3 direction;

    private CharacterController controller;
    private float turnSmoothVelocity;

    protected virtual void Start(){
        //animationController = GetComponent<PlayerAnimationController>();
        controller = GetComponent<CharacterController>();
    }

    protected virtual void Update(){

        //if (isUIActive) return;

        //takes input from axis 
        float horiznotalAxis = Input.GetAxisRaw(horizontal);
        float verticalAxis = Input.GetAxisRaw(verticals);

        //normalize directions so that when its moving diagonaly the speed is not dubled
        direction = new Vector3(horiznotalAxis, 0f, verticalAxis).normalized;

        if (direction.magnitude >= 0.1f){
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.SimpleMove(direction * speed /* Time.deltaTime*/);
            //animationController.PlayAllLayers(Animations.RUNNING);
        }
        else{
            controller.SimpleMove(direction * speed /* Time.deltaTime*/);
            //animationController.PlayAllLayers(Animations.IDLE);
        }

    }
    /*
    public void ToggleUI(bool isActive){
        isUIActive = isActive;
    }
    
    public void LoadData(GameData data)
    {
        //this.transform.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        //data.playerPosition = this.transform.position;
    }
    */

}