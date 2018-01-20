using UnityEngine;
using System.Collections;
using System;

namespace PSGameUtils{
	public static class GameUtils {
		
		public static float ConvertToFloat(float val){
			if (float.IsPositiveInfinity(val))
			{
				Debug.LogWarning("Max Value");
				val = float.MaxValue;
			} else if (float.IsNegativeInfinity(val))
			{
				val = float.MinValue;
			}

			return val;

		}

		public static string uniqueID(){
			DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			int currentEpochTime = (int)(DateTime.UtcNow - epochStart).TotalSeconds;
			int z1 = UnityEngine.Random.Range (0, 1000000);
			int z2 = UnityEngine.Random.Range (0, 1000000);
			string uid = currentEpochTime + ":" + z1 + ":" + z2;
			return uid;
		}
	}
}
