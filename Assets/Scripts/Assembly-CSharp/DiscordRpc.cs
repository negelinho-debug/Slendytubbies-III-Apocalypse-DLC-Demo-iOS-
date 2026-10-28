using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public class DiscordRpc
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ReadyCallback(ref DiscordUser connectedUser);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void DisconnectedCallback(int errorCode, string message);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ErrorCallback(int errorCode, string message);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void JoinCallback(string secret);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SpectateCallback(string secret);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void RequestCallback(ref DiscordUser request);

	public struct EventHandlers
	{
		public ReadyCallback readyCallback;

		public DisconnectedCallback disconnectedCallback;

		public ErrorCallback errorCallback;

		public JoinCallback joinCallback;

		public SpectateCallback spectateCallback;

		public RequestCallback requestCallback;
	}

	[Serializable]
	public struct RichPresenceStruct
	{
		public IntPtr state;

		public IntPtr details;

		public long startTimestamp;

		public long endTimestamp;

		public IntPtr largeImageKey;

		public IntPtr largeImageText;

		public IntPtr smallImageKey;

		public IntPtr smallImageText;

		public IntPtr partyId;

		public int partySize;

		public int partyMax;

		public IntPtr matchSecret;

		public IntPtr joinSecret;

		public IntPtr spectateSecret;

		public bool instance;
	}

	[Serializable]
	public struct DiscordUser
	{
		public string userId;

		public string username;

		public string discriminator;

		public string avatar;
	}

	public enum Reply
	{
		No,
		Yes,
		Ignore
	}

	public class RichPresence
	{
		private RichPresenceStruct _presence;

		private readonly List<IntPtr> _buffers = new List<IntPtr>(10);

		public string state;

		public string details;

		public long startTimestamp;

		public long endTimestamp;

		public string largeImageKey;

		public string largeImageText;

		public string smallImageKey;

		public string smallImageText;

		public string partyId;

		public int partySize;

		public int partyMax;

		public string matchSecret;

		public string joinSecret;

		public string spectateSecret;

		public bool instance;

		internal RichPresenceStruct GetStruct()
		{
			if (_buffers.Count > 0)
			{
				FreeMem();
			}
			_presence.state = StrToPtr(state);
			_presence.details = StrToPtr(details);
			_presence.startTimestamp = startTimestamp;
			_presence.endTimestamp = endTimestamp;
			_presence.largeImageKey = StrToPtr(largeImageKey);
			_presence.largeImageText = StrToPtr(largeImageText);
			_presence.smallImageKey = StrToPtr(smallImageKey);
			_presence.smallImageText = StrToPtr(smallImageText);
			_presence.partyId = StrToPtr(partyId);
			_presence.partySize = partySize;
			_presence.partyMax = partyMax;
			_presence.matchSecret = StrToPtr(matchSecret);
			_presence.joinSecret = StrToPtr(joinSecret);
			_presence.spectateSecret = StrToPtr(spectateSecret);
			_presence.instance = instance;
			return _presence;
		}

		private IntPtr StrToPtr(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return IntPtr.Zero;
			}
			int byteCount = Encoding.UTF8.GetByteCount(input);
			IntPtr intPtr = Marshal.AllocHGlobal(byteCount + 1);
			for (int i = 0; i < byteCount + 1; i++)
			{
				Marshal.WriteByte(intPtr, i, 0);
			}
			_buffers.Add(intPtr);
			Marshal.Copy(Encoding.UTF8.GetBytes(input), 0, intPtr, byteCount);
			return intPtr;
		}

		private static string StrToUtf8NullTerm(string toconv)
		{
			string text = toconv.Trim();
			byte[] bytes = Encoding.Default.GetBytes(text);
			if (bytes.Length > 0 && bytes[bytes.Length - 1] != 0)
			{
				text += "\0\0";
			}
			return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(text));
		}

		internal void FreeMem()
		{
			for (int num = _buffers.Count - 1; num >= 0; num--)
			{
				Marshal.FreeHGlobal(_buffers[num]);
				_buffers.RemoveAt(num);
			}
		}
	}

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Initialize")]
	public static extern void Initialize(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId);

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Shutdown")]
	public static extern void Shutdown();

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_RunCallbacks")]
	public static extern void RunCallbacks();

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_UpdatePresence")]
	private static extern void UpdatePresenceNative(ref RichPresenceStruct presence);

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_ClearPresence")]
	public static extern void ClearPresence();

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Respond")]
	public static extern void Respond(string userId, Reply reply);

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_UpdateHandlers")]
	public static extern void UpdateHandlers(ref EventHandlers handlers);

	public static void UpdatePresence(RichPresence presence)
	{
		RichPresenceStruct presence2 = presence.GetStruct();
		UpdatePresenceNative(ref presence2);
		presence.FreeMem();
	}
}
