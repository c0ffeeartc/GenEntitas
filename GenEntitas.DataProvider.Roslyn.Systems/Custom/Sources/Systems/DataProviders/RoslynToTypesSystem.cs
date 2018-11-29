using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel.Composition;
using System.Text;
using DesperateDevs.Utils;
using Entitas;
using Entitas.CodeGeneration.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.MSBuild;
using Ent = GenEntitas.SettingsEntity;

namespace GenEntitas
{
	[Export(typeof(ISystem))]
	[Guid("0F5D3CA4-FC93-469B-8D73-9C570EFE04F8")]
	public class RoslynToTypesSystem : ReactiveSystem<Ent>
	{
		public				RoslynToTypesSystem	( Contexts contexts ) : base( contexts.settings )
		{
			_contexts			= contexts;
		}

		public				RoslynToTypesSystem	(  ) : this( Contexts.sharedInstance )
		{
		}

		public static	Dictionary<Type,List<String>> TypeToContextNames;
		private				Contexts				_contexts;

		protected override	ICollector<Ent>			GetTrigger				( IContext<Ent> context )
		{
			return context.CreateCollector( SettingsMatcher.RoslynPathToSolution );
		}

		protected override	Boolean					Filter					( Ent entity )
		{
			return entity.hasRoslynPathToSolution
				&& !String.IsNullOrEmpty( entity.roslynPathToSolution.Value );
		}

		protected override	void					Execute					( List<Ent> ents )
		{
			var pathToSolution		= ents[0].roslynPathToSolution.Value;
			var allTypes			= CollectAllInformation( pathToSolution );

			_contexts.main.ReplaceRoslynAllTypes( allTypes );

			var prefix = typeof( RoslynToTypesSystem ) + ": ";
			Console.WriteLine(prefix + Path.GetFileName( pathToSolution ) + ": All types: " + allTypes.Count() );

			var possibleComponents = allTypes.Where(t => t.ToDisplayString().EndsWith("Component"));
			Console.WriteLine( prefix + "TypeName.EndsWith(\"Component\"): " + possibleComponents.Count(  ) );

			var compTypes = new List<INamedTypeSymbol>(  );

            foreach (var type in allTypes
                .Where( type => type.Implements( typeof( IComponent ) ) )
                .Where(type => !type.IsAbstract)
                .Where(type => GetContextNames(type).Any()))  // force usage of contextAttribute for IComponent
            {
                try
                {
//                    Console.WriteLine(prefix + "Found component " + type );
					compTypes.Add( type );
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(prefix + "Fail to handle type: " + type.ToDisplayString());
                }
            }
            Console.WriteLine( prefix + "Components: " + compTypes.Count(  ) );

			var nonCompTypes = new List<INamedTypeSymbol>(  );

            foreach (var type in allTypes
               .Where( type => !type.Implements( typeof( IComponent ) ) )
               .Where(type => !type.IsGenericType)
               .Where(type => GetContextNames(type).Any()))
            {
                try
                {
//                    Console.WriteLine(prefix + "Found non-component " + type);
					nonCompTypes.Add( type );
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine( prefix + "Fail to handle type: " + type.ToDisplayString(  ) );
                }
            }
            Console.WriteLine( prefix + "Non-Components: " + nonCompTypes.Count(  ) );

			compTypes.AddRange( nonCompTypes );
			_contexts.main.ReplaceRoslynComponentTypes( compTypes.ToList(  ) );
		}

        private List<INamedTypeSymbol> CollectAllInformation(string pathToSolutionOrProjectFile)
        {
            using (var workspace = MSBuildWorkspace.Create())
            {
                if (pathToSolutionOrProjectFile.EndsWith(".sln"))
                {
                    var solution = workspace.OpenSolutionAsync(pathToSolutionOrProjectFile).Result;

					var sb = new StringBuilder(  );
					var typeSymbols = new List<INamedTypeSymbol>(  );
                    foreach (var project in solution.Projects)
                    {
                        var result = AnalyzeProject(project);
                        typeSymbols.AddRange(result);
						sb.Append( project.Name ) ;
						sb.Append( ", " ) ;
                    }
					if ( sb.Length > 0 )
					{
						sb.Remove( sb.Length - 2, 2 );
					}
					Console.WriteLine( typeof(RoslynToTypesSystem) + ": " + Path.GetFileName( solution.FilePath ) + ": " + solution.ProjectIds.Count + " projects: " + sb );
					return typeSymbols;
                }
                else
                {
                    var project = workspace.OpenProjectAsync(pathToSolutionOrProjectFile).Result;
                    return AnalyzeProject(project);
                }
            }
        }

        private List<INamedTypeSymbol> AnalyzeProject(Project project)
        {
            var compilation = project.GetCompilationAsync().Result;
            var allTypes = compilation.GetSymbolsWithName(x => true, SymbolFilter.Type).OfType<ITypeSymbol>().Where(t => t is INamedTypeSymbol).OfType<INamedTypeSymbol>();
			return allTypes.ToList(  );
		}

        private static List<string> GetContextNames(ITypeSymbol type)
        {
            List<string> contextAttributes = new List<string>();

            foreach (var attribute in type.GetAttributes())
            {
                if (attribute.AttributeClass.BaseType.ToDisplayString().Contains(typeof (ContextAttribute).ToCompilableString()))
                {
                    // we need to go deeper!
                    var declaration = attribute.AttributeConstructor.DeclaringSyntaxReferences.First().GetSyntax();
                    var baseConstructorInit = (ConstructorInitializerSyntax) declaration.DescendantNodes().First(x => x.IsKind(SyntaxKind.BaseConstructorInitializer));
                    var name = (LiteralExpressionSyntax) baseConstructorInit.ArgumentList.Arguments.First().Expression;                    
                    contextAttributes.Add(name.ToString().Replace("\"",""));
                }
                if (attribute.AttributeClass.ToDisplayString().Contains(typeof(ContextAttribute).ToCompilableString()))
                {
                    var name = (string)attribute.ConstructorArguments.First().Value;
                    contextAttributes.Add(name);
                }
            }
            return contextAttributes;
        }
	}
}