using System;
using System.Collections.Generic;

namespace GenEntitas
{
	public interface ISettingsService
	{
		Dictionary<String,List<String>> Parse		( String str );
		Boolean				BoolFromStr				( String str );
	}
}