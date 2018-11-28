using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using Entitas;

namespace GenEntitas
{
	public class MefImportSystems
	{
		[ImportMany(typeof(IExecuteSystem))]
		private			IEnumerable<IExecuteSystem>	_imports;

		public				List<IExecuteSystem>	Import					(  )
		{
			var catalog				= new DirectoryCatalog( ".", "GenEntitas.*.dll" );
			var composition			= new CompositionContainer( catalog );
			composition.ComposeParts( this );
			var systems				= _imports.ToList(  );
			systems.Sort( (a, b) => String.Compare( a.GetType(  ).Name, b.GetType(  ).Name, StringComparison.Ordinal ) );
			return systems;
		}
	}
}