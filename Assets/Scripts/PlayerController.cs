using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject
{
    public float maxSpeed = 5f; //velocidade m�xima
    public float jumpSpeed = 7f; //velocidade de pulo
    public float jumpVelocityReduction = 0.5f; //redu��o da velocidade de pulo
   

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

        targetVelocity = move * maxSpeed; //a velocidade alvo � igual a velocidade m�xima multiplicada pelo movimento
    }
}
