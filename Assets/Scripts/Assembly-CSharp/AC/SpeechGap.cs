namespace AC
{
	public struct SpeechGap
	{
		public int characterIndex;

		public int waitTime;

		public SpeechGap(int _characterIndex, int _waitTime)
		{
			characterIndex = _characterIndex;
			waitTime = _waitTime;
		}
	}
}
