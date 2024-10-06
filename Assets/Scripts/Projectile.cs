using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Weapon weapon;

    public virtual void Init(Weapon weapon)
    {
        this.weapon = weapon;
    }
    public virtual void Launch()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
