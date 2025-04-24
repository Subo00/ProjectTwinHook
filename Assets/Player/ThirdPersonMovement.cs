using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour, IDataPersistence{

    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [SerializeField] protected string horizontal = "Horizontal";
    [SerializeField] protected string vertical = "Vertical";

    protected bool isUIActive = false;
    //protected PlayerAnimationController animationController;
    protected Vector3 direction;

    private CharacterController controller;
    private float turnSmoothVelocity;

    protected virtual void Awake(){
        controller = GetComponent<CharacterController>();
    }

    protected virtual void Start(){
        //animationController = GetComponent<PlayerAnimationController>();
    }

    protected virtual void Update(){

        if (isUIActive) return;

        //takes input from axis 
        float horiznotalAxis = Input.GetAxisRaw(horizontal);
        float verticalAxis = Input.GetAxisRaw(vertical);

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
    
    public void ToggleUI(bool isActive){
        isUIActive = isActive;
    }
    
    public void LoadData(GameData data)
    {

        controller.enabled = false;
        this.transform.position = data.playerPosition;
        //this.transform.rotation = data.playerRotation;
        controller.enabled = true;

    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
        //data.playerRotation = this.transform.rotation;
    }
    

}