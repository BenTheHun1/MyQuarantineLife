using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerBox : MonoBehaviour
{
	public Miner miner;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.TryGetComponent(out Interact interact))
		{
			if (interact.isGrafix)
			{
				Destroy(collision.gameObject);
				miner.secTrans++;
			}
		}
	}
}
