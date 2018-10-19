using System;

namespace SwissILKnife
{
	public sealed class UnsupportedArgumentTypeException : Exception
	{
		public UnsupportedArgumentTypeException(string msg) : base(msg)
		{
		}
	}
}