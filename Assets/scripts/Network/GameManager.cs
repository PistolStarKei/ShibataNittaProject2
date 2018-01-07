using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon;

public enum GameState {
	PRE_START, COUNTDOWN, FIGHTING, FINISHED
}
namespace PSPhoton {
	public class GameManager : PunBehaviour {

		public static GameManager instance;
		public int loadedPlayers=0;

		public float gameTime = 0;
		public double startTimestamp = 0;

		private GameState state = GameState.PRE_START;


		// Use this for initialization
		void Start () {
			instance=this;
			CreatePlayer();
			photonView.RPC ("ConfirmLoad", PhotonTargets.All);
		}
		
		private void CreatePlayer() {
			/*
			int pos = (int) PhotonNetwork.player.CustomProperties["spawn"];
			int carNumber = (int) PhotonNetwork.player.CustomProperties["car"];
			Transform spawn = spawnPoints[pos];

			GameObject car = PhotonNetwork.Instantiate("Car" + carNumber, spawn.position, spawn.rotation, 0);
			*/
		}

		[PunRPC]
		public void ConfirmLoad () {
			loadedPlayers++;
		}

		void Update () {
			gameTime += Time.deltaTime;

			switch (state) {
			case GameState.PRE_START:
				if (PhotonNetwork.isMasterClient) {
					CheckCountdown();
				}
				break;
			case GameState.COUNTDOWN:
				//messagesGUI.text = "" + (1 + (int) (startTimestamp - PhotonNetwork.time));
				if (PhotonNetwork.time >= startTimestamp) {
					StartGame();
				}
				break;
			case GameState.FIGHTING:
				if (gameTime > 3) {
					//messagesGUI.text = "";
				} else {
					//messagesGUI.text = "GO!";
				}
				break;
			case GameState.FINISHED:
				//Show Result
				break;
			}

			/*if (localCar.state == RaceState.FINISHED) {
				state = RaceState.FINISHED;
				// enable panel
				endRacePanel.gameObject.SetActive(true);
			}*/

		}

		private void CheckCountdown() {
			bool takingTooLong = gameTime >= 5;
			bool finishedLoading = loadedPlayers == PhotonNetwork.playerList.Length;
			if (takingTooLong || finishedLoading) {
				photonView.RPC ("StartCountdown", PhotonTargets.All, PhotonNetwork.time + 4);
			}
		}

		[PunRPC]
		public void StartCountdown(double startTimestamp) {
			Debug.Log ("Countdown");
			state = GameState.COUNTDOWN;
			// sets local timestamp to the desired server timestamp (will be checked every frame)
			this.startTimestamp = startTimestamp;
		}


		public void StartGame() {
			
			state = GameState.FIGHTING;
			/*
			GameObject[] cars = GameObject.FindGameObjectsWithTag ("Player");
			foreach (GameObject go in cars) {
				carControllers.Add(go.GetComponent<CarRaceControl>());
				go.GetComponent<CarInput>().enabled = true;
				go.GetComponent<CarRaceControl>().currentWaypoint = GameObject.Find("Checkpoint1").GetComponent<Checkpoint>();
				go.GetComponent<CarRaceControl> ().state = RaceState.RACING;
			}
			*/
			//localCar.GetComponent<CarInput>().controlable = true;
			//localCar.GetComponent<WeaponManager>().enabled = true;

			gameTime = 0;
		}


		public override void OnPhotonPlayerDisconnected(PhotonPlayer disconnetedPlayer) {
			Debug.Log (disconnetedPlayer.NickName + " disconnected...");

			/*CarRaceControl toRemove = null;
			foreach (CarRaceControl rc in carControllers) {
				//Debug.Log (rc.photonView.owner);
				if (rc.photonView.owner == null) {
					toRemove = rc;
				}
			}
			// remove car controller of disconnected player from the list
			carControllers.Remove (toRemove);
			*/
		}

		// Use this to go back to the menu, without leaving the lobby
		public void ResetToMenu () {
			PhotonNetwork.LeaveRoom ();
			//PhotonNetwork.LoadLevel ("Menu");
		}

	}
}
