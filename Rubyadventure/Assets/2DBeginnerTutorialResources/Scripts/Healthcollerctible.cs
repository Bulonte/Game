using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthcollerctible : MonoBehaviour
{
    public AudioClip audioClip;

    public GameObject effectParticle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("与我们碰撞的物体是： " + collision);
        RubyController rubyController = collision.GetComponent<RubyController>();
        if(rubyController!=null)
        {
            if (rubyController.currentHealth<rubyController.maxHealth)
            {
                rubyController.changeHealth(1);
                rubyController.PlaySound(audioClip);
                Instantiate(effectParticle,transform.position,Quaternion.identity);
                Destroy(gameObject);
            }
            
        }
    }
}
