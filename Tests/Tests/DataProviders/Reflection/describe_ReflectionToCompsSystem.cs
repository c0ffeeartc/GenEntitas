using System;
using System.Collections.Generic;
using Entitas;
using GenEntitas;
using NSpec;

namespace Tests.Tests
{
	internal class TestCompContextNone : IComponent
	{
	}
	[Main]
	internal class TestCompContextMain : IComponent
	{
	}
	[Settings]
	internal class TestCompContextSettings : IComponent
	{
	}
	[Main][Settings]
	internal class TestCompContextMainAndSettings : IComponent
	{
	}
	public class describe_ReflectionToCompsSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					when_testing_context_attribute		(  )
		{
			context["add contexts, system and ents"] = (  ) =>
			{
				_contexts				= new Contexts(  );
				var system				= new ReflectionToCompsSystem( _contexts );

				_contexts.main.SetReflectionComponentTypes( new List<Type>
					{
						typeof( TestCompContextMain ),
						typeof( TestCompContextSettings ),
						typeof( TestCompContextMainAndSettings ),
						typeof( TestCompContextNone ),
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

				it["context names and amount should match"] = (  ) =>
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
					{
						var ent = group_.GetEntities(  )[2];
						ent.contextNamesComp.Values.Count.should_be( 2 );
					}
					{
						var ent = group_.GetEntities(  )[3];
						ent.contextNamesComp.Values.Count.should_be( 1 );
					}
				};
			};
		}
	}
}