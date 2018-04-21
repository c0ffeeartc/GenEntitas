using System;
using System.Collections.Generic;
using Entitas;
using GenEntitas.Sources;
using NSpec;

namespace Tests.Tests
{
	[Main]
	internal class TestComp1 : IComponent
	{
	}
	[Settings]
	internal class TestComp2 : IComponent
	{
	}
	public class describe_ReflectionToCompsSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					given_empty_context		(  )
		{
			context["add contexts, system and ents"] = (  ) =>
			{
				_contexts				= new Contexts(  );
				var system				= new ReflectionToCompsSystem( _contexts );

				_contexts.main.SetReflectionComponentTypes( new List<Type>
					{
						typeof( TestComp1 ),
						typeof( TestComp2 ),
					} );

				var group_				= _contexts.main.GetGroup( MainMatcher.Comp );

				it["has 0 comp ents"] = (  ) =>
				{
					group_.count.should_be( 0 );
				};

				it["has N comp ents"] = (  ) =>
				{
					system.Execute(  );
					group_.count.should_be( _contexts.main.reflectionComponentTypes.Values.Count );
				};

				it["context names should match"] = (  ) =>
				{
					{
						var ent = group_.GetEntities(  )[0];
						ent.contextNamesComp.Values.Count.should_be( 1 );
						ent.contextNamesComp.Values[0].should_be( "Main" );
					}
					{
						var ent = group_.GetEntities(  )[1];
						ent.contextNamesComp.Values.Count.should_be( 1 );
						ent.contextNamesComp.Values[0].should_be( "Settings" );
					}
				};
			};
		}
	}
}