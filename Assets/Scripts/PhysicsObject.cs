using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float gravityModifier = 1f; //modificador de gravidade
    public float minGroundNormalY = 0.65f; //valor minimo para considerar que o objeto est� no ch�o

    protected Rigidbody2D rb2d;
    protected Vector2 velocity; //velocidade atual do objeto
    protected Vector2 targetVelocity; //velocidade alvo do objeto

    protected ContactFilter2D contactFilter; //filtro de colis�o
    protected RaycastHit2D[] raycastHit2Ds = new RaycastHit2D[16]; //vetor de colis�es
    
    protected bool grounded; //verifica se o objeto est� no ch�o
    protected Vector2 groundNormal; //normal do ch�o

    protected const float minMoveDistance = 0.001f; //distancia minima para movimento
    protected const float shellRadius = 0.01f; //raio do colisor


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        contactFilter.useTriggers = false; //n�o usa triggers
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); //seta a m�scara de colis�o
        contactFilter.useLayerMask = true; //usa a m�scara de colis�o
    }

    // Update is called once per frame
    void Update()
    {
        targetVelocity = Vector2.zero; //zera a velocidade alvo
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

	}

	private void FixedUpdate()
	{
        //a todo momento a velocidade � alterada pela gravidade
		velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
		velocity.x = targetVelocity.x; //a velocidade na horizontal � igual a velocidade alvo

		grounded = false; //o objeto n�o est� no ch�o
        

        //a posi��o � alterada de acordo com a velocidade
        Vector2 deltaPosition = velocity * Time.deltaTime;
		Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x); //vetor de movimento na horizontal
      
        Vector2 move = moveAlongGround * deltaPosition.x; //vetor de movimento na horizontal
        Movement(move, false); //movimenta o objeto na horizontal
		move = Vector2.up * deltaPosition.y;
        Movement(move, true);
	}

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude; //calcula a distancia do movimento
        if (distance > minMoveDistance) //s� movimenta se a dist�ncia for maior que a minima
        {
			//projeta o colisor do objeto na dire��o do movimento para verificar colis�es
			int count = rb2d.Cast(move, contactFilter, raycastHit2Ds, distance + shellRadius); 
            
            for (int i = 0; i < count; i++)
            {
                Vector2 currentNormal = raycastHit2Ds[i].normal;
                
                if(currentNormal.y > minGroundNormalY) //verifica se o objeto est� no ch�o
                {
                    grounded = true;
                    if (yMovement) //se o movimento for na vertical
                    {
						groundNormal = currentNormal; //seta a normal do ch�o
						currentNormal.x = 0; //zera a normal na horizontal
					}
				}

                float projection = Vector2.Dot(velocity, currentNormal); //calcula a proje��o da velocidade na normal
                if(projection < 0) //se a proje��o for menor que zero
                {
					velocity -= projection * currentNormal; //remove a proje��o da velocidade
				}

                float modifiedDistance = raycastHit2Ds[i].distance - shellRadius; //calcula a distancia modificada
                distance = modifiedDistance < distance ? modifiedDistance : distance; //se a distancia modificada for menor que a distancia, a distancia recebe o valor da distancia modificada
			}

		}
        
        //Movimenta o objeto com base no vetor de movimento recebido
		rb2d.position += move.normalized * distance;
	}
}
