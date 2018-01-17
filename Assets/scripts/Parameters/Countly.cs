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

	public static string ToCountryCode(this SystemLanguage language) {
		string result;
		if (COUTRY_CODES.TryGetValue (language, out result)) {
			return result;
		} else {
			return COUTRY_CODES[SystemLanguage.Unknown];
		}
	}


}
