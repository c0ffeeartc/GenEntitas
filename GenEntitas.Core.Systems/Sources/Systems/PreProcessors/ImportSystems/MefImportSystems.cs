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
#pragma warning disable 649
		[ImportMany(typeof(ISystem))]
		private				IEnumerable<ISystem>	_imports;
#pragma warning restore 649

		public				List<ISystem>			Import					(  )
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