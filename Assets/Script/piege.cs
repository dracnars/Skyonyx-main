using UnityEngine;

public class piege : MonoBehaviour
{
    public int degats = 1;

    private void OnTriggerEnter2D(Collider2D truc) {
        if(truc.tag == "Player") {
            truc.SendMessage("takeDamage", degats);
        }
    }
}
