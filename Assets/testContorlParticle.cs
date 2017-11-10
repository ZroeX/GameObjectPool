using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZroeX.ObjectPool;

public class testContorlParticle : MonoBehaviour {

	private Queue< GameObject> objs= new Queue<GameObject>();
	// Update is called once per frame
	void Update () {
		if ( Input.GetKeyDown( KeyCode.P ) ) {
			GameObject obj = EffectPool.Instance.Get( "testParticle" );
			obj.transform.position = new Vector3( Random.Range( 0f, 5f ), Random.Range( 0f, 5f ), Random.Range( 0f, 5f ) );
			objs.Enqueue( obj );
		}

		if ( Input.GetKeyDown( KeyCode.O ) ) {
			EffectPool.Instance.Return( objs.Dequeue() );
		}



	}
}
