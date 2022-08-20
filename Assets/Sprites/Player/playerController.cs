using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    public float collisionOffset = 0.05f;
    public float moveSpeed = 5f;
    public ContactFilter2D movementFilter;
    Vector2 movementInput;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero)
        {
            bool success = TryMove(movementInput);

            if (!success)
            {
                success = TryMove(new Vector2(movementInput.x, 0));
                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.y, 0));
                }
            }

            if (movementInput.x != 0)
            {
                animator.SetBool("isMovingSides", success);
            }
            else { animator.SetBool("isMovingSides", false); }

            if (movementInput.y > 0)
            {
                animator.SetBool("isMovingUp", success);;
            }
            else { animator.SetBool("isMovingUp", false); }
            if (movementInput.y < 0)
            {
                animator.SetBool("isMovingDown", success);
            } else { animator.SetBool("isMovingDown",false); }
        }
        if(movementInput.x==0 && movementInput.y == 0)
        {
            animator.SetBool("isMovingSides",false);
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingDown", false);
        }

        if (movementInput.x < 0) {
            spriteRenderer.flipX = true;
        } else if(movementInput.x>0){
            spriteRenderer.flipX = false;
        }
    }


    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else { return false; }
        }
        else return false;
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}