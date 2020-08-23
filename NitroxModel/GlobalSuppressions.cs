// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Bug", "S1656:Variables should not be self-assigned", Justification = "NLog magic", Scope = "member", Target = "~M:NitroxModel.Logger.Log.AddSensitiveFilter(NLog.Config.LoggingConfiguration,System.Func{NLog.Targets.Target,System.Boolean})")]
[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "WIP IP-Getting", Scope = "member", Target = "~M:NitroxModel.Helper.WebHelper.GetPublicIP~System.String")]
[assembly: SuppressMessage("Critical Code Smell", "S2346:Flags enumerations zero-value members should be named \"None\"", Justification = "MultiplayerSessionReservationStates-Context", Scope = "type", Target = "~T:NitroxModel.MultiplayerSession.MultiplayerSessionReservationStates")]
[assembly: SuppressMessage("Major Code Smell", "S4016:Enumeration members should not be named \"Reserved\"", Justification = "MultiplayerSessionReservationStates-Context", Scope = "type", Target = "~T:NitroxModel.MultiplayerSession.MultiplayerSessionReservationStates")]
