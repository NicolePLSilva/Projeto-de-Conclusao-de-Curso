using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CodigoObjeto 
{
	private Ponte ponte ;

	private Player player ;

	public void Main(){
	ponte = GameObject.Find("Ponte").GetComponent<Ponte>();
	player = GameObject.Find("Player").GetComponent<Player>();
	

}
}