using UnityEngine;
using UnityEngine.InputSystem;

public class Throwable : MonoBehaviour
{
    Vector3 throwVector;
    Rigidbody2D rb2d;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    
    public void ThrowRandomly()
    {
        throwVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        rb2d.AddForce(throwVector * 10, ForceMode2D.Impulse);
    }


    // Instead of using physics we can use a collection of valid areas and pick from those to move 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Stop the movement
        rb2d.angularVelocity = 0;
        rb2d.linearVelocity = Vector2.zero;
    }


    private void Update()
    {

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Call the ThrowRandomly method when the "E" key is pressed
            ThrowRandomly();
        }


    }
}
