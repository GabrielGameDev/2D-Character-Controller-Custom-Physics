using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    public float gravityModifier = 1f; //modificador de gravidade
    public float minGroundNormalY = 0.65f; //valor minimo para considerar que o objeto está no chão

    protected Rigidbody2D rb2d;
    protected Vector2 velocity; //velocidade atual do objeto

    protected ContactFilter2D contactFilter; //filtro de colisão
    protected RaycastHit2D[] raycastHit2Ds = new RaycastHit2D[16]; //vetor de colisões
    protected List<RaycastHit2D> rayCastHitList = new List<RaycastHit2D>(16); //lista de colisões
    protected bool grounded; //verifica se o objeto está no chão
    protected Vector2 groundNormal; //normal do chão

    protected const float minMoveDistance = 0.001f; //distancia minima para movimento
    protected const float shellRadius = 0.01f; //raio do colisor


    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        contactFilter.useTriggers = false; //não usa triggers
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); //seta a máscara de colisão
        contactFilter.useLayerMask = true; //usa a máscara de colisão
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void FixedUpdate()
	{
        //a todo momento a velocidade é alterada pela gravidade
		velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;

        grounded = false; //o objeto não está no chão

        //a posição é alterada de acordo com a velocidade
        Vector2 deltaPosition = velocity * Time.deltaTime;
        Vector2 move = Vector2.up * deltaPosition.y;
        Movement(move, true);
	}

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude; //calcula a distancia do movimento
        if (distance > minMoveDistance) //só movimenta se a distância for maior que a minima
        {
			//projeta o colisor do objeto na direção do movimento para verificar colisões
			int count = rb2d.Cast(move, contactFilter, raycastHit2Ds, distance + shellRadius); 
            //rayCastHitList.Clear(); //limpa a lista de colisões
            //for (int i = 0; i < count; i++)
            //{
            //    rayCastHitList.Add(raycastHit2Ds[i]);
            //}
            for (int i = 0; i < count; i++)
            {
                Vector2 currentNormal = raycastHit2Ds[i].normal;
                if(currentNormal.y > minGroundNormalY) //verifica se o objeto está no chão
                {
                    grounded = true;
                    if (yMovement) //se o movimento for na vertical
                    {
						groundNormal = currentNormal; //seta a normal do chão
						currentNormal.x = 0; //zera a normal na horizontal
					}
				}

                float projection = Vector2.Dot(velocity, currentNormal); //calcula a projeção da velocidade na normal
                if(projection < 0) //se a projeção for menor que zero
                {
					velocity -= projection * currentNormal; //remove a projeção da velocidade
				}

                float modifiedDistance = raycastHit2Ds[i].distance - shellRadius; //calcula a distancia modificada
                distance = modifiedDistance < distance ? modifiedDistance : distance; //se a distancia modificada for menor que a distancia, a distancia recebe o valor da distancia modificada
			}

		}
        
        //Movimenta o objeto com base no vetor de movimento recebido
		rb2d.position += move.normalized * distance;
	}
}
