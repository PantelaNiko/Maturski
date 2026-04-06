using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    public abstract void Cast(Vector3 mouseClickPos);
}