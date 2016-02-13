# SteamSharp Release Notes

## IN DEVELOPMENT

This project is still in development and may not yet ready for consumption. The code in `main` is not guaranteed (and in fact is most likely not) stable for a production application.

## v1.0.1.0 Change Log
* Fixed API issue preventing authentication
* Upgraded to latest Newtownsoft.Json
* Removed unnecessary references from projects which prevented build on new repo clones

### Feature Set

* Use pre-built methods to make Steam API calls and receive back deserialized objects -- all without ever having to deal with any aspect of the HTTP layer
* Easy and intuitive framework to send and receive REST calls to the Steam API
* Make calls to the Steam API using Steam specified semantics (no complex URI building)
* Add API key authorization to all calls by a client, simply by adding the relevant Authenticator
