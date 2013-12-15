
namespace SteamSharp.FlowTests.Tests {

	/// <summary>
	/// Interface intended to be inherited by all Test Classes, which provides the mandatory methods to override.
	/// </summary>
	public interface ITestClass {
		
		/// <summary>
		/// Contains all execution logic for the class
		/// </summary>
		/// <returns>Boolean value representing the success or failure state of the class.</returns>
		bool Invoke();

	}

}
