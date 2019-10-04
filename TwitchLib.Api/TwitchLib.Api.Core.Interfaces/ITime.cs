﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace TwitchLib.Api.Core.Interfaces
{
    public interface ITime
    {
        DateTime GetTimeNow();

        Task GetDelay(TimeSpan timespan, CancellationToken cancellationToken);
    }
}
