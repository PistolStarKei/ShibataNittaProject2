using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Countly {

	private static readonly Dictionary<SystemLanguage,string> COUTRY_CODES = new Dictionary<SystemLanguage, string>()
	{
		{ SystemLanguage.Afrikaans, "ZA" },
		{ SystemLanguage.Arabic    , "SA" },
		{ SystemLanguage.Basque    , "US" },
		{ SystemLanguage.Belarusian    , "BY" },
		{ SystemLanguage.Bulgarian    , "BJ" },
		{ SystemLanguage.Catalan    , "ES" },
		{ SystemLanguage.Chinese    , "CN" },
		{ SystemLanguage.Czech    , "HK" },
		{ SystemLanguage.Danish    , "DK" },
		{ SystemLanguage.Dutch    , "BE" },
		{ SystemLanguage.English    , "US" },
		{ SystemLanguage.Estonian    , "EE" },
		{ SystemLanguage.Faroese    , "FU" },
		{ SystemLanguage.Finnish    , "FI" },
		{ SystemLanguage.French    , "FR" },
		{ SystemLanguage.German    , "DE" },
		{ SystemLanguage.Greek    , "JR" },
		{ SystemLanguage.Hebrew    , "IL" },
		{ SystemLanguage.Icelandic    , "IS" },
		{ SystemLanguage.Indonesian    , "ID" },
		{ SystemLanguage.Italian    , "IT" },
		{ SystemLanguage.Japanese    , "JP" },
		{ SystemLanguage.Korean    , "KR" },
		{ SystemLanguage.Latvian    , "LV" },
		{ SystemLanguage.Lithuanian    , "LT" },
		{ SystemLanguage.Norwegian    , "NO" },
		{ SystemLanguage.Polish    , "PL" },
		{ SystemLanguage.Portuguese    , "PT" },
		{ SystemLanguage.Romanian    , "RO" },
		{ SystemLanguage.Russian    , "RU" },
		{ SystemLanguage.SerboCroatian    , "SP" },
		{ SystemLanguage.Slovak    , "SK" },
		{ SystemLanguage.Slovenian    , "SI" },
		{ SystemLanguage.Spanish    , "ES" },
		{ SystemLanguage.Swedish    , "SE" },
		{ SystemLanguage.Thai    , "TH" },
		{ SystemLanguage.Turkish    , "TR" },
		{ SystemLanguage.Ukrainian    , "UA" },
		{ SystemLanguage.Vietnamese    , "VN" },
		{ SystemLanguage.ChineseSimplified    , "CN" },
		{ SystemLanguage.ChineseTraditional    , "CN" },
		{ SystemLanguage.Unknown    , "UN" },
		{ SystemLanguage.Hungarian    , "HU" },
	};

	private static readonly Dictionary<string,string> COUTRY_CODES_ON_GUI = new Dictionary<string,string>()
	{
		{ "ZA","za" },
		{ "SA","sa" },
		{ "UN","gv_mini" },
		{ "BJ","bj" },
		{ "ES","es" },
		{ "CN","cn" },
		{ "HK","cz" },
		{ "DK","dk" },
		{ "BE","be" },
		{ "US","us" },
		{ "EE","ee" },
		{ "FU","gv_mini" },
		{ "FI","fi" },
		{ "FR","fr" },
		{ "DE","de" },
		{ "JR","gr" },
		{ "IL", "il"},
		{ "IS","is" },
		{ "ID","id" },
		{ "IT","it" },
		{ "JP","jp" },
		{ "KR","kr" },
		{ "LV","lv" },
		{ "LT","lt" },
		{ "NO","no" },
		{ "PL","pl" },
		{ "PT","pt" },
		{ "RO","ro" },
		{ "RU","ru" },
		{ "SP","gv_mini" },
		{ "SK","sk" },
		{ "SI","si" },
		{ "SE","se" },
		{ "TH","th" },
		{ "TR","tr" },
		{ "UA","ua" },
		{ "VN","vn" },
		{ "HU","hu" },
	};

	public static string ToCountrySPrite(string countly) {
		string result;
		if (COUTRY_CODES_ON_GUI.TryGetValue (countly, out result)) {
			return result;
		} else {
			return COUTRY_CODES_ON_GUI["UN"];
		}
	}

	public static string ToCountryCode(this SystemLanguage language) {
		string result;
		if (COUTRY_CODES.TryGetValue (language, out result)) {
			return result;
		} else {
			return COUTRY_CODES[SystemLanguage.Unknown];
		}
	}


}
