﻿using System.Collections.Generic;
using Entitas;
using Ent = GenEntitas.MainEntity;

namespace GenEntitas
{
	public class DestroySystem : IExecuteSystem
	{
		public				DestroySystem			( Contexts contexts )
		{
			_group				= contexts.main.GetGroup( MainMatcher.Destroy );
		}

		private				IGroup<Ent>				_group;
		private				List<Ent>				_buffer					= new List<Ent>(  );

		public				void					Execute					(  )
		{
			var ents = _group.GetEntities( _buffer );
			for ( var i = ents.Count - 1; i >= 0; i-- )
			{
				var ent = ents[i];
				ent.Destroy(  );
			}
		}
	}
}