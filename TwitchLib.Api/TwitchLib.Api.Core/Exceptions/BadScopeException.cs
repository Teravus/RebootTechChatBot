﻿using System;

namespace TwitchLib.Api.Core.Exceptions
{
    /// <inheritdoc />
    /// <summary>Exception representing a provided scope was not permitted.</summary>
    public class BadScopeException : Exception
    {
        /// <inheritdoc />
        /// <summary>Exception constructor</summary>
        public BadScopeException(string data)
            : base(data)
        {
        }
    }
}