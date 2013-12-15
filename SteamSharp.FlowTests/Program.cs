using SteamSharp.FlowTests.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SteamSharp.FlowTests {

	/// <summary>
	/// Super basic test console application designed to test scenarios which require multiple input from the user (and thus are not well suited for standard test cases).
	/// </summary>
	public class Program {

		private static readonly Version _version = new AssemblyName( typeof( Program ).GetTypeInfo().Assembly.FullName ).Version;

		static void Main( string[] args ) {

			WriteConsole.Write( "SteamSharp Flow Test Client v" + _version.ToString(), ConsoleColor.Yellow );
			WriteConsole.Information( "Usage: SteamSharp.FlowTests.exe {ClassName}\n" );
			WriteConsole.Information( "Initializing test client..." );

			if( args.Length < 1 ) {
				WriteConsole.Information( "Usage: SteamSharp.FlowTests.exe {ClassName}" );
				FatalError( "Please specify a Test Class to invoke." );
			}

			Type type = null;
			try {
				type = Type.GetType( "SteamSharp.FlowTests.Tests." + args[0] );
			} catch( Exception ) {
				FatalError( String.Format( "Unable to find Test Class for \"{0}.\"", args[0] ) );
			}

			ITestClass testClass = null;
			try { 
				testClass = (ITestClass)Activator.CreateInstance( type );
			} catch( Exception ) {
				FatalError( String.Format( "Cannot cast class \"{0}\" to the ITestClass interface. Is your target Test Class properly inheriting ITestClass?", args[0] ) );
			}

			WriteConsole.Information( "Test console loaded successfully." );

			try {
				MethodInfo methodInfo = type.GetMethod( "Invoke" );
				if( methodInfo == null )
					throw new Exception( "Unable to locate method." );
			} catch( Exception ) {
				FatalError( String.Format( "Cannot find Invoke method on Test Class. Has ITestClass been modified to not inlude the Invoke method?", args[0] ) );
			}

			if( testClass.Invoke() )
				WriteConsole.Success( "TEST PASSED - " + args[0] );
			else
				WriteConsole.Error( "TEST FAILED - " + args[0] );

			Console.WriteLine();
			WriteConsole.Information( "Flows concluded, client exiting." );
			WriteConsole.Pause();
			Environment.Exit( 0 );

		}

		public static void FatalError( string message ){
			WriteConsole.Error( message );
			WriteConsole.Pause();
			Environment.Exit( -1 );
		}

	}

}
