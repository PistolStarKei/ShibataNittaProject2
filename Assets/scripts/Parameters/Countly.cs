using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Countly {

	private static readonly Dictionary<SystemLanguage,string> COUTRY_CODES = new Dictionary<SystemLanguage, string>()
	{
		{ SystemLanguage.Afrikaans, "za" },
		{ SystemLanguage.Arabic    , "sa" },
		{ SystemLanguage.Basque    , "us" },
		{ SystemLanguage.Belarusian    , "by" },
		{ SystemLanguage.Bulgarian    , "bj" },
		{ SystemLanguage.Catalan    , "es" },
		{ SystemLanguage.Chinese    , "cn" },
		{ SystemLanguage.Czech    , "hk" },
		{ SystemLanguage.Danish    , "dk" },
		{ SystemLanguage.Dutch    , "be" },
		{ SystemLanguage.English    , "us" },
		{ SystemLanguage.Estonian    , "ee" },
		{ SystemLanguage.Faroese    , "fu" },
		{ SystemLanguage.Finnish    , "fi" },
		{ SystemLanguage.French    , "fr" },
		{ SystemLanguage.German    , "de" },
		{ SystemLanguage.Greek    , "jr" },
		{ SystemLanguage.Hebrew    , "il" },
		{ SystemLanguage.Icelandic    , "is" },
		{ SystemLanguage.Indonesian    , "id" },
		{ SystemLanguage.Italian    , "it" },
		{ SystemLanguage.Japanese    , "jp" },
		{ SystemLanguage.Korean    , "kr" },
		{ SystemLanguage.Latvian    , "lv" },
		{ SystemLanguage.Lithuanian    , "lt" },
		{ SystemLanguage.Norwegian    , "no" },
		{ SystemLanguage.Polish    , "pl" },
		{ SystemLanguage.Portuguese    , "pt" },
		{ SystemLanguage.Romanian    , "ro" },
		{ SystemLanguage.Russian    , "ru" },
		{ SystemLanguage.SerboCroatian    , "sp" },
		{ SystemLanguage.Slovak    , "sk" },
		{ SystemLanguage.Slovenian    , "si" },
		{ SystemLanguage.Spanish    , "es" },
		{ SystemLanguage.Swedish    , "se" },
		{ SystemLanguage.Thai    , "th" },
		{ SystemLanguage.Turkish    , "tr" },
		{ SystemLanguage.Ukrainian    , "ua" },
		{ SystemLanguage.Vietnamese    , "vn" },
		{ SystemLanguage.ChineseSimplified    , "cn" },
		{ SystemLanguage.ChineseTraditional    , "cn" },
		{ SystemLanguage.Unknown    , "jp" },
		{ SystemLanguage.Hungarian    , "hu" },
	};

	public static readonly string[] ServerRegions=new string[]{
		"ASIA","JAPAN","KOREA","AUST","CANNADA","EULO","INDIA","RUSSIA","eRUSSIA","USA","wUSA"
	};

	public static readonly Dictionary<string,CloudRegionCode> ServerRegion=new Dictionary<string, CloudRegionCode>(){
		{"ASIA",CloudRegionCode.asia},
		{"JAPAN",CloudRegionCode.jp},
		{"KOREA",CloudRegionCode.kr},
		{"AUST",CloudRegionCode.au},
		{"CANNADA",CloudRegionCode.cae},
		{"EULO",CloudRegionCode.eu},
		{"INDIA",CloudRegionCode.@in},
		{"RUSSIA",CloudRegionCode.ru},
		{"eRUSSIA",CloudRegionCode.ru},
		{"USA",CloudRegionCode.us},
		{"wUSA",CloudRegionCode.usw}

	};
	public static readonly Dictionary<string,bool> ServerRegionAvaillable=new Dictionary<string, bool>(){
		{"ASIA",false},
		{"JAPAN",true},
		{"KOREA",false},
		{"AUST",false},
		{"CANNADA",false},
		{"EULO",false},
		{"INDIA",false},
		{"RUSSIA",false},
		{"eRUSSIA",false},
		{"USA",false},
		{"wUSA",false}

	};

	public static string GetRegion(this SystemLanguage language){
		string result="asia";
		if(language==SystemLanguage.Japanese)result="JAPAN";
		if(language==SystemLanguage.Korean && ServerRegionAvaillable["KOREA"])result="KOREA";
		if(language==SystemLanguage.Russian && ServerRegionAvaillable["RUSSIA"])result="RUSSIA";
		if(language==SystemLanguage.English && ServerRegionAvaillable["USA"])result="USA";
		return result;
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
