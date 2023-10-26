using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 5f; //velocidade m�xima
    public float jumpSpeed = 7f; //velocidade de pulo
    public float jumpVelocityReduction = 0.5f; //redu��o da velocidade de pulo
   
    Animator animator; //anima��es do personagem
    SpriteRenderer spriteRenderer; //sprite do personagem

    bool facingRight = true; //verifica se o personagem est� virado para a direita
	private void Awake()
	{
	    animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected override void ComputeVelocity()
	{
		Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal"); //pega o valor do eixo horizontal
        if(Input.GetButtonDown("Jump") && grounded) //se o bot�o de pulo for pressionado e o objeto estiver no ch�o
        {
            velocity.y = jumpSpeed;
        }
        else if(Input.GetButtonUp("Jump")) //se o bot�o de pulo for solto
        {
			if(velocity.y > 0) //se a velocidade na vertical for maior que 0
            {
				velocity.y *= jumpVelocityReduction; //reduz a velocidade na vertical
			}
		}

        if(facingRight && move.x < 0)
        {
            spriteRenderer.flipX = true; //vira o sprite
			facingRight = false; //o personagem n�o est� virado para a direita
		}
		else if(!facingRight && move.x > 0)
        {
			spriteRenderer.flipX = false; //vira o sprite
			facingRight = true; //o personagem est� virado para a direita
        }

        animator.SetBool("onGround", grounded); //seta a anima��o de pulo
        animator.SetBool("run", move.x != 0); //seta a anima��o de andar

        targetVelocity = move * maxSpeed; //a velocidade alvo � igual a velocidade m�xima multiplicada pelo movimento
    }
}
