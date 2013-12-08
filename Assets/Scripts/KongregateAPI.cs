using UnityEngine;
using System;
using System.Collections;

/*
//////////////////////////////////////////////////////////////////////////////
//                                                                          //
//                  SUBMIT STATISTICS WITH                                  //
//                                                                          //
//  Application.ExternalCall("kongregate.stats.submit","MatchesMade",1);    //
//                                                                          //
//////////////////////////////////////////////////////////////////////////////
*/

public class KongregateAPI : MonoBehaviour {
	
	public bool isKongregate = false;
	public string username = "Guest";
	public int userID = 0;
	public string gameAuthToken = "";
	
	void Awake() {
		DontDestroyOnLoad(gameObject);
		
		Application.ExternalEval(
			"if(typeof(kongregateUnitySupport) != 'undefined'){" +
			" kongregateUnitySupport.initAPI('KongregateAPI', 'OnKongregateAPILoaded');" +
			"}"
		);
	}
	
	void OnKongregateAPILoaded(string userInfoString){
		isKongregate = true;
		
		string[] kongStuff = userInfoString.Split("|"[0]);
		userID = int.Parse(kongStuff[0]);
		username = kongStuff[1];
		gameAuthToken = kongStuff[2];
	}
	
	public void SubmitStats(string stat, int score){
		Application.ExternalCall("kongregate.stats.submit", stat, score);
	}
}

