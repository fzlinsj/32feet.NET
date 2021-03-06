#region Copyright (c) 2002-2003, James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole, Philip A. Craig
/************************************************************************************
'
' Copyright � 2002-2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' Copyright � 2000-2003 Philip A. Craig
'
' This software is provided 'as-is', without any express or implied warranty. In no 
' event will the authors be held liable for any damages arising from the use of this 
' software.
' 
' Permission is granted to anyone to use this software for any purpose, including 
' commercial applications, and to alter it and redistribute it freely, subject to the 
' following restrictions:
'
' 1. The origin of this software must not be misrepresented; you must not claim that 
' you wrote the original software. If you use this software in a product, an 
' acknowledgment (see the following) in the product documentation is required.
'
' Portions Copyright � 2003 James W. Newkirk, Michael C. Two, Alexei A. Vorontsov, Charlie Poole
' or Copyright � 2000-2003 Philip A. Craig
'
' 2. Altered source versions must be plainly marked as such, and must not be 
' misrepresented as being the original software.
'
' 3. This notice may not be removed or altered from any source distribution.
'
'***********************************************************************************/
#endregion

namespace NUnit.Framework 
{
	using System;
#if ! NETCF //andyh
	using System.Runtime.Serialization;
#endif
	
	/// <summary>
	/// Thrown when an assertion failed.
	/// </summary>
	/// 
#if ! (NETCF && FX1_1) //andyh
	[Serializable]
#endif
	public class IgnoreException : System.Exception
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public IgnoreException (string message) : base(message) 
		{}

		/// <summary>
		/// Standard constructor
		/// </summary>
		/// <param name="message">The error message that explains 
		/// the reason for the exception</param>
		/// <param name="inner">The exception that caused the 
		/// current exception</param>
		public IgnoreException(string message, Exception inner) :
			base(message, inner) 
		{}

#if ! NETCF //andyh
		/// <summary>
		/// Serialization Constructor
		/// </summary>
		protected IgnoreException(SerializationInfo info, 
			StreamingContext context) : base(info,context)
		{}
#endif

	}
}
