using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Player_Object_Controller : MonoBehaviour
{
    Rigidbody2D rigidbody2D_player;
    Vector2 lookDirection = new Vector2(1,0);
    public float moveSpeed;
    public float moveLimit;
    public float hypeMoveLimit;
    public float hypeSpeed;
    public Camera view_camera;
    public GameObject Cooltime_icon_hype;
    public GameObject Cooltime_icon_gravity;
    public GameObject Map_gravity;
    public float coolTime_hype;
    public float coolTime_gravity;
    float delay_hype = 0;
    float delay_gravity = 0;
    public GameObject hypeFX;
    Image icon_hype;
    Image icon_gravity;
    bool isCooldown_hype;
    bool isHype;
    bool isCooldown_gravity;
    int angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D_player = GetComponent<Rigidbody2D>();
        icon_hype = Cooltime_icon_hype.GetComponent<Image>();
        icon_gravity = Cooltime_icon_gravity.GetComponent<Image>();

        isCooldown_hype = false;
        isHype = false;
        icon_hype.fillAmount = 1;

        isCooldown_gravity = false;
        icon_gravity.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(horizontal, vertical);

        rigidbody2D_player.AddForce(new Vector2(horizontal * moveSpeed, 0), ForceMode2D.Force);

        //속도 제한 스크립트
        if(!(isHype))
        {
            if(rigidbody2D_player.velocity.x > moveLimit)
                rigidbody2D_player.velocity = new Vector2(moveLimit, rigidbody2D_player.velocity.y);
            
            else if(rigidbody2D_player.velocity.x < moveLimit*(-1)) 
                rigidbody2D_player.velocity = new Vector2(moveLimit*(-1), rigidbody2D_player.velocity.y);

            if(rigidbody2D_player.velocity.y > moveLimit)
                rigidbody2D_player.velocity = new Vector2(rigidbody2D_player.velocity.x, moveLimit);
            
            else if(rigidbody2D_player.velocity.y < moveLimit*(-1)) 
                rigidbody2D_player.velocity = new Vector2(rigidbody2D_player.velocity.x*(-1), moveLimit*(-1));
        
        }
        else
        {
            if(rigidbody2D_player.velocity.x > hypeMoveLimit)
                rigidbody2D_player.velocity = new Vector2(hypeMoveLimit, rigidbody2D_player.velocity.y);
            
            else if(rigidbody2D_player.velocity.x < hypeMoveLimit*(-1)) 
                rigidbody2D_player.velocity = new Vector2(hypeMoveLimit*(-1), rigidbody2D_player.velocity.y);

            if(rigidbody2D_player.velocity.y > hypeMoveLimit)
                rigidbody2D_player.velocity = new Vector2(rigidbody2D_player.velocity.x, hypeMoveLimit);
            
            else if(rigidbody2D_player.velocity.y < hypeMoveLimit*(-1)) 
                rigidbody2D_player.velocity = new Vector2(rigidbody2D_player.velocity.x*(-1), hypeMoveLimit*(-1));
        }


        //능력 쿨타임 처리
        if(isCooldown_hype)
        {
            delay_hype += Time.fixedDeltaTime;
            icon_hype.fillAmount = delay_hype / (coolTime_hype);
            Debug.Log(delay_hype);
            if(delay_hype > coolTime_hype)
            {
                isCooldown_hype = false;
                delay_hype = 0;
            }

        }

        if(isCooldown_gravity)
        {
            delay_gravity += Time.fixedDeltaTime;
            icon_gravity.fillAmount = delay_gravity / (coolTime_gravity);
            Debug.Log(delay_gravity);
            if(delay_gravity > coolTime_gravity)
            {
                isCooldown_gravity = false;
                delay_gravity = 0;
            }

        }
        

        //특수 능력 관련 처리 스크립트
        if(Input.GetKeyDown(KeyCode.Z) && !(isHype) && !(isCooldown_hype))
        {
            isHype = true;
            hypeFX.SetActive(true);
            icon_hype.fillAmount = 0; 
            rigidbody2D_player.AddForce(move * hypeSpeed, ForceMode2D.Impulse);
        }
        

        Debug.DrawRay(rigidbody2D_player.position, lookDirection * 1.5f, new Color(1,0,0));

        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        //중력반전
        if(Input.GetKeyDown(KeyCode.X) && !(isCooldown_gravity))
        {
            angle += 180;
            rigidbody2D_player.simulated = false;
            icon_gravity.fillAmount = 0;
            Map_gravity.transform.DORotate(new Vector3(0,0,angle), 5f);
            Invoke("OffGravity", 5f);

        }       
        
    }
    
    void OffHype()
    {
        isHype = false;
        hypeFX.SetActive(false);
        isCooldown_hype = true;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Wall" && isHype)
        {
            OffHype();
        }
            
    }
    
    void OffGravity()
    {
        rigidbody2D_player.simulated = true;
        isCooldown_gravity = true;
    }
}
