﻿using UnityEngine;
using System.Collections;

namespace PSGameUtils{
	public static class GameUtils {
		
		public static float ConvertToFloat(float val){
			if (float.IsPositiveInfinity(val))
			{
				val = float.MaxValue;
			} else if (float.IsNegativeInfinity(val))
			{
				val = float.MinValue;
			}

			return val;

		}


	}
}