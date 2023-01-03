using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainClass
{
	private Ponte ponte ;

	public void Main(){
	ponte = GameObject.Find("Ponte").GetComponent<Ponte>();
ponte.MoverPlayer("direita");
}
}