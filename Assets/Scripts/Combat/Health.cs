using System.Collections;
using UnityEngine;

namespace RPG.Movement
{   
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;

        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            Debug.Log(gameObject.name + " recived damage, current HP is: " + health);
        }
    }
}