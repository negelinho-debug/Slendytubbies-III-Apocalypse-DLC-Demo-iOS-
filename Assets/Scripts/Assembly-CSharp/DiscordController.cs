using System;
using UnityEngine;
using UnityEngine.Events;

public class DiscordController : MonoBehaviour
{
	public DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();

	public string applicationId;

	public string optionalSteamId;

	public int callbackCalls;

	public int clickCounter;

	public DiscordRpc.DiscordUser joinRequest;

	public UnityEvent onConnect;

	public UnityEvent onDisconnect;

	public UnityEvent hasResponded;

	public DiscordJoinEvent onJoin;

	public DiscordJoinEvent onSpectate;

	public DiscordJoinRequestEvent onJoinRequest;

	public string currentPresence;

	public string state;

	public string largeImage;

	public string stateES;

	private DiscordRpc.EventHandlers handlers;

	public void OnClick()
	{
		Debug.Log("Discord: on click!");
		clickCounter++;
		presence.details = string.Format(currentPresence);
		presence.largeImageKey = string.Format(largeImage);
		if (PlayerPrefs.GetInt("Language") == 1)
		{
			presence.state = string.Format(stateES);
		}
		else
		{
			presence.state = string.Format(state);
		}
		DiscordRpc.UpdatePresence(presence);
	}

	public void RequestRespondYes()
	{
		Debug.Log("Discord: responding yes to Ask to Join request");
		DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.Yes);
		hasResponded.Invoke();
	}

	public void RequestRespondNo()
	{
		Debug.Log("Discord: responding no to Ask to Join request");
		DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.No);
		hasResponded.Invoke();
	}

	public void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser)
	{
		callbackCalls++;
		Debug.Log($"Discord: connected to {connectedUser.username}#{connectedUser.discriminator}: {connectedUser.userId}");
		onConnect.Invoke();
	}

	public void DisconnectedCallback(int errorCode, string message)
	{
		callbackCalls++;
		Debug.Log($"Discord: disconnect {errorCode}: {message}");
		onDisconnect.Invoke();
	}

	public void ErrorCallback(int errorCode, string message)
	{
		callbackCalls++;
		Debug.Log($"Discord: error {errorCode}: {message}");
	}

	public void JoinCallback(string secret)
	{
		callbackCalls++;
		Debug.Log($"Discord: join ({secret})");
		onJoin.Invoke(secret);
	}

	public void SpectateCallback(string secret)
	{
		callbackCalls++;
		Debug.Log($"Discord: spectate ({secret})");
		onSpectate.Invoke(secret);
	}

	public void RequestCallback(ref DiscordRpc.DiscordUser request)
	{
		callbackCalls++;
		Debug.Log($"Discord: join request {request.username}#{request.discriminator}: {request.userId}");
		joinRequest = request;
		onJoinRequest.Invoke(request);
	}

	private void Start()
	{
	}

	private void Update()
	{
		DiscordRpc.RunCallbacks();
	}

	private void OnEnable()
	{
		Debug.Log("Discord: init");
		callbackCalls = 0;
		handlers = default(DiscordRpc.EventHandlers);
		handlers.readyCallback = ReadyCallback;
		handlers.disconnectedCallback = (DiscordRpc.DisconnectedCallback)Delegate.Combine(handlers.disconnectedCallback, new DiscordRpc.DisconnectedCallback(DisconnectedCallback));
		handlers.errorCallback = (DiscordRpc.ErrorCallback)Delegate.Combine(handlers.errorCallback, new DiscordRpc.ErrorCallback(ErrorCallback));
		handlers.joinCallback = (DiscordRpc.JoinCallback)Delegate.Combine(handlers.joinCallback, new DiscordRpc.JoinCallback(JoinCallback));
		handlers.spectateCallback = (DiscordRpc.SpectateCallback)Delegate.Combine(handlers.spectateCallback, new DiscordRpc.SpectateCallback(SpectateCallback));
		handlers.requestCallback = (DiscordRpc.RequestCallback)Delegate.Combine(handlers.requestCallback, new DiscordRpc.RequestCallback(RequestCallback));
		DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
	}

	private void OnDisable()
	{
		Debug.Log("Discord: shutdown");
		DiscordRpc.Shutdown();
	}

	private void OnDestroy()
	{
	}
}
