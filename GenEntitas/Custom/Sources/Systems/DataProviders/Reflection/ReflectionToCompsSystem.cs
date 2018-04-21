using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using Ent = MainEntity;

namespace GenEntitas.Sources
{
	public class ReflectionToCompsSystem : ReactiveSystem<Ent>
	{
		public				ReflectionToCompsSystem	( Contexts contexts ) : base( contexts.main )
		{
			_contexts			= contexts;
		}

		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( MainMatcher.ReflectionComponentTypes );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasReflectionComponentTypes;
		}

		protected override	void					Execute					( List<Ent> entities )
		{
			var types			= _contexts.main.reflectionComponentTypes.Values;

			foreach ( var t in types )
			{
				var ent = _contexts.main.CreateEntity();
				ent.AddTypeComp( t );
				ent.AddComp( t.Name, t.FullName );

				ProvideContextNamesComp( ent, t );
			}
		}

		private				void					ProvideContextNamesComp	( MainEntity ent, Type t )
		{
			var attributes = Attribute.GetCustomAttributes( t, typeof(Entitas.CodeGeneration.Attributes.ContextAttribute) );
			if ( attributes.Length == 0 )
			{
				ent.AddContextNamesComp( new List<string>{ "Main" } );
				return;
			}

			var contextNames = Attribute
				.GetCustomAttributes(t)
				.OfType<Entitas.CodeGeneration.Attributes.ContextAttribute>()
				.Select(attr => attr.contextName)
				.ToList();

			ent.AddContextNamesComp( contextNames );
		}
	}
}