using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : AItem
{
    public override Vector3 m_holdingPosOffset()
    {
        return new Vector3(-0.058f,0.051f,0.112f);
    }

    public override Vector3 m_holdingRotOffset()
    {
        return new Vector3(27.481f,166.88f,0);
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
