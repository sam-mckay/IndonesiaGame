using UnityEngine;
using System.Collections;

public class PowerUp
{
    protected Vector3 position;
    protected float abilityStrength; //amount it affects the stat by
    protected PowerUp_Controller controller;

    public PowerUp(Vector3 _position, float _abilityStrength, PowerUp_Controller _controller)
    {
        position = _position;
        abilityStrength = _abilityStrength;
        controller = _controller;
    }

    public virtual void OnTriggerEnter(Collider other)
    {

    }

    public void Destroy()
    {
        controller.removePowerUp();
    }
}
