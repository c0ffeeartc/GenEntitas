using System;
using System.Collections.Generic;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using GenEntitas;
using NSpec;

namespace Tests.Tests
{
	public class describe_ReflectionToCompsSystem : nspec
	{
		private				Contexts				_contexts;

		private				void					when_testing_context_attribute		(  )
		{
			before = ()=>
			{
				_contexts				= new Contexts(  );
			};
			it["given system and ents"] = (  ) =>
			{
				var system				= new ReflectionToCompsSystem( _contexts );

				_contexts.main.SetReflectionComponentTypes( new List<Type>
					{
						typeof( TestCompContextMain ),
						typeof( TestCompContextSettings ),
						typeof( TestCompContextMainAndSettings ),
						typeof( TestCompContextNone ),
					} );

				var group_				= _contexts.main.GetGroup( MainMatcher.Comp );

				group_.count.should_be( 0 );

				// when
				system.Execute(  );

				// then
				group_.count.should_be( _contexts.main.reflectionComponentTypes.Values.Count );

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

			it["creates event listener comp entity"] = (  ) =>
			{
				var system				= new ReflectionToCompsSystem( _contexts );

				_contexts.main.SetReflectionComponentTypes( new List<Type>
					{
						typeof( TestFlagEventTrue ),
					} );

				var group_				= _contexts.main.GetGroup( MainMatcher.EventListenerComp );

				// when
				system.Execute(  );

				{
					var ent = group_.GetSingleEntity(  );
					ent.contextNamesComp.Values.should_contain( "Test" );
					ent.hasPublicFieldsComp.should_be_true(  );
				}
			};

			it["creates event listener comp entity 2"] = (  ) =>
			{
				var system				= new ReflectionToCompsSystem( _contexts );

				_contexts.main.SetReflectionComponentTypes( new List<Type>
					{
						typeof( TestFlagEventTrueAddAndRemove ),
					} );

				var group_				= _contexts.main.GetGroup( MainMatcher.EventListenerComp );

				// when
				system.Execute(  );

				group_.count.should_be( 2 );
				{
					var ent = group_.GetEntities(  )[0];
					ent.contextNamesComp.Values.should_contain( "Test" );
					ent.hasPublicFieldsComp.should_be_true(  );
				}
			};
		}

		private class TestCompContextNone : IComponent
		{
		}

		[Context("Main")]
		private class TestCompContextMain : IComponent
		{
		}

		[Context("Settings")]
		private class TestCompContextSettings : IComponent
		{
		}

		[Context("Main")]
		[Context("Settings")]
		private class TestCompContextMainAndSettings : IComponent
		{
		}

		[Context("Test")]
		[Event(true)]
		private class TestFlagEventTrue : IComponent
		{
		}

		[Context("Test")]
		[Event(true)]
		[Event(true, EventType.Removed)]
		private class TestFlagEventTrueAddAndRemove : IComponent
		{
		}
	}
}