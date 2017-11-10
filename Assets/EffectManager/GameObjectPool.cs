using System.Collections.Generic;
using UnityEngine;

namespace ONIK.ObjectPool.Design {
	public class GameObjectPool {
		private readonly GameObject[] prefabs;
		private readonly Transform poolParent;
		private readonly string prefabName;

		private readonly Queue<GameObject> objQueu;
		private readonly int maxNum;
		private int currentMax;
		public bool isNeedReMaxNum {
			get {
				return maxNum < currentMax;
			}
		}

		public GameObjectPool( GameObject prefab, int number, Transform root, string name )
			: this( new GameObject[ 1 ] { prefab }, number, root, name ) {

		}

		public GameObjectPool( GameObject[] prefabs, int number, Transform root, string name ) {
			this.prefabs = prefabs;
			poolParent = root;
			prefabName = name;

			objQueu = new Queue<GameObject>( number );
			maxNum = number;
			currentMax = 0;
			for ( int i = 0; i < number; ++i ) {
				CreateNewInstance();
			}
		}

		private void CreateNewInstance() {
			GameObject clone = Object.Instantiate( prefabs[ Random.Range( 0, prefabs.Length ) ] );
			clone.name = prefabName;
			clone.transform.SetParent( poolParent );
			clone.SetActive( false );
			++currentMax;

			objQueu.Enqueue( clone );
		}

		public GameObject Get() {

			if ( objQueu.Count == 0 ) {
				CreateNewInstance();
			}

			GameObject obj = objQueu.Dequeue();
			obj.SetActive( true );
			return obj;
		}

		public void Return( GameObject obj ) {
			obj.SetActive( false );
			objQueu.Enqueue( obj );
			ReMaxNum();
		}

		public void ReMaxNum() {
			while ( objQueu.Count == currentMax && objQueu.Count > maxNum ) {
				GameObject.Destroy( objQueu.Dequeue() );
				currentMax = objQueu.Count;
			}			
		}
	}
}