//-------------------------------------------------
// Copyright (C) 0000-2018, Yegor c0ffee
// Email: c0ffeeartc@gmail.com
//-------------------------------------------------

namespace GenEntitas
{
	internal class Program
	{
		public static void Main( string[] args )
		{
			var runner = new Runner();
			runner.Init(  );
			runner.Systems.Initialize(  );
			runner.Systems.Execute(  );
			runner.Systems.Cleanup(  );
			runner.Systems.TearDown(  );
		}
	}
}