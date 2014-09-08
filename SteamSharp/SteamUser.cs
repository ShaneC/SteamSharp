using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SteamSharp {

	/// <summary>
	/// Object representing a Steam User. This class includes authentication data for use in authorizing a user.
	/// </summary>
	public class SteamUser : IComparable<SteamUser> {

		/// <summary>
		/// User's SteamID.
		/// </summary>
		public SteamID SteamID { get; set; }

		/// <summary>
		/// Obtained via <see cref="UserAuthenticator"/>. Access Token to be passed on the Steam API for requests requiring first part authentication.
		/// </summary>
		public string OAuthAccessToken { get; set; }

		/// <summary>
		/// Obtained via <see cref="AuthCookieAuthenticator"/>. Transfer Token to be passed on Steam Store transactions.
		/// </summary>
		public string TransferToken { get; set; }

		/// <summary>
		/// Obtained via <see cref="AuthCookieAuthenticator"/>. Cookie which contains the authentication token for the user.
		/// </summary>
		public Cookie AuthCookie { get; set; }

		/// <summary>
		/// Obtained via <see cref="AuthCookieAuthenticator"/>. Login Key obtained from the Authentication Cookie.
		/// </summary>
		public string AuthCookieLoginKey { get; set; }

		/// <summary>
		/// Community and profile information for the given Steam User.
		/// </summary>
		public SteamCommunity.PlayerInfo PlayerInfo { get; set; }

		/// <summary>
		/// If the user object was generated via a GetFriendsList call, this field will contain the DateTime the friendship was established. Null otherwise.
		/// </summary>
		public DateTime FriendSince { get; set; }

		/// <summary>
		/// Only available in FriendsList property of a Connected SteamChatClient. Messages between the authenticated user and this SteamUser.
		/// </summary>
		public List<SteamChatMessage> MessagesWithUser { get; set; }

		/// <summary>
		/// Returns true if the user object is capable of UserAuthentication to OAuth APIs. False otherwise.
		/// </summary>
		/// <returns></returns>
		public bool IsAuthenticated() {
			return ( !String.IsNullOrEmpty( this.OAuthAccessToken ) );
		}

		/// <summary>
		/// (Async) If the user object is User Authenticated (IsAuthenticated() == true), creates a SteamClient and pulls the latest PlayerInfo data for the current user.
		/// Throws <see cref="SteamSharp.SteamAuthenticationException"/> if the user is not authenticated or is no longer able to authenticate.
		/// </summary>
		public async Task GetProfileDataAsync() {
			if( !this.IsAuthenticated() )
				throw new SteamAuthenticationException( "User object is not capable of authenticating with the UserAuthenticator object." );
			SteamClient client = new SteamClient();
			client.Authenticator = Authenticators.UserAuthenticator.ForProtectedResource( this.OAuthAccessToken );
			await GetProfileDataAsync( client );
		}

		/// <summary>
		/// (Async) (Requires client with <see cref="SteamSharp.Authenticators.APIKeyAuthenticator"/> or <see cref="SteamSharp.Authenticators.UserAuthenticator"/>)
		/// Pulls the latest PlayerInfo data for the current user from the Steam service.
		/// </summary>
		/// <param name="client">SteamClient for use in executing the request.</param>
		public async Task GetProfileDataAsync( SteamClient client ) {
			this.PlayerInfo = ( await SteamCommunity.GetUserAsync( client, this.SteamID ) ).PlayerInfo;
		}

		/// <summary>
		/// Provides a comparer, assisting to sort a list of <see cref="SteamUser"/> into alphabetical order based on their Persona Name (display name).
		/// </summary>
		/// <param name="target">Target for comparison</param>
		/// <returns>Comparator which allows alphabetical order sorted by PersonaName.</returns>
		public int CompareTo( SteamUser target ) {
			if( target == null )
				return 1;
			return this.PlayerInfo.PersonaName.CompareTo( target.PlayerInfo.PersonaName );
		}

		/// <summary>
		/// Operator comparator for comparing the <see cref="SteamID"/>s of two <see cref="SteamUser"/> objects.
		/// </summary>
		/// <param name="obj">Object to compare against this SteamUser.</param>
		/// <returns>True if both SteamUsers have the same SteamID. False otherwise.</returns>
		public override bool Equals( object obj ) {
			if( obj is SteamUser && this.SteamID != null && ((SteamUser)obj).SteamID != null )
				return ( this.SteamID.Equals( ((SteamUser)obj).SteamID ) );
			return false;
		}

		/// <summary>
		/// Operator comparator for comparing the <see cref="SteamID"/>s of two <see cref="SteamUser"/> objects.
		/// </summary>
		/// <param name="a">Input A</param>
		/// <param name="b">Input B</param>
		/// <returns>True if both SteamUsers have the same SteamID. False otherwise.</returns>
		public static bool operator ==( SteamUser a, SteamUser b ) {
			return a.Equals( b );
		}

		/// <summary>
		/// Returns the HashCode of the user's SteamID. If null, generates random HashCode.
		/// </summary>
		/// <returns>HashCode of the SteamUser object.</returns>
		public override int GetHashCode() {
			if( this.SteamID != null )
				return this.SteamID.GetHashCode();
			else
				return Guid.NewGuid().GetHashCode();
		}

		/// <summary>
		/// Operator comparator for comparing the <see cref="SteamID"/>s of two <see cref="SteamUser"/> objects.
		/// </summary>
		/// <param name="a">Input A</param>
		/// <param name="b">Input B</param>
		/// <returns>True if both SteamUsers have different SteamIDs. True otherwise.</returns>
		public static bool operator !=( SteamUser a, SteamUser b ) {
			return !a.Equals( b );
		}

		/// <summary>
		/// Method for use in IList.Sort() operations to sort a list of SteamUsers by their PersonaState.
		/// Users will be sorted by Available, Busy, Away, Offline. If the PersonaState is the same, users are sorted by PersonaName alphabetical.
		/// </summary>
		public static int PersonaSortComparer( SteamUser left, SteamUser right ) {

			Func<PersonaState, PersonaState> NormalizePersona = delegate( PersonaState state ) {
				if( state == PersonaState.LookingToPlay || state == PersonaState.LookingToTrade )
					return PersonaState.Online;
				if( state == PersonaState.Snooze )
					return PersonaState.Away;
				return state;
			};

			var leftState = NormalizePersona( left.PlayerInfo.PersonaState );
			var rightState = NormalizePersona( right.PlayerInfo.PersonaState );

			// If the PersonaState is identical, sort by most recent message (if available), then alphabetically
			if( leftState == rightState ) {

				if( left.MessagesWithUser != null && left.MessagesWithUser != null ) {

					// Recent Messages are available
					var leftRecentMessage = left.MessagesWithUser[left.MessagesWithUser.Count - 1];
					var rightRecentMessage = right.MessagesWithUser[left.MessagesWithUser.Count - 1];

					if( leftRecentMessage != null && rightRecentMessage == null )
						return -1;
					else if( leftRecentMessage == null && rightRecentMessage != null )
						return 1;

					var timeCompare = leftRecentMessage.UTCMessageDateTime.CompareTo( rightRecentMessage.UTCMessageDateTime );
					if( timeCompare != 0 )
						return timeCompare;

				}

				return left.PlayerInfo.PersonaName.CompareTo( right.PlayerInfo.PersonaName );

			}

			if( leftState == PersonaState.Online )
				return -1;
			else if( rightState == PersonaState.Online )
				return 1;

			if( leftState == PersonaState.Busy )
				return -1;
			else if( rightState == PersonaState.Busy )
				return 1;

			if( leftState == PersonaState.Away )
				return -1;
			else if( rightState == PersonaState.Away )
				return 1;

			return 1;

		}

	}

}
