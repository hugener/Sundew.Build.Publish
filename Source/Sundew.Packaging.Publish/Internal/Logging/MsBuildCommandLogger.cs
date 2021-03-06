﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MsBuildCommandLogger.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Packaging.Publish.Internal.Logging
{
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Sundew.Packaging.Publish.Internal.Commands;

    internal class MsBuildCommandLogger : ICommandLogger
    {
        private readonly TaskLoggingHelper taskLoggingHelper;

        public MsBuildCommandLogger(TaskLoggingHelper taskLoggingHelper)
        {
            this.taskLoggingHelper = taskLoggingHelper;
        }

        public void LogError(string message)
        {
            this.taskLoggingHelper.LogError(message);
        }

        public void LogImportant(string message)
        {
            this.taskLoggingHelper.LogMessage(MessageImportance.High, message);
        }

        public void LogInfo(string message)
        {
            this.taskLoggingHelper.LogMessage(MessageImportance.Normal, message);
        }

        public void LogMessage(string message)
        {
            this.taskLoggingHelper.LogMessage(MessageImportance.Low, message);
        }

        public void LogWarning(string message)
        {
            this.taskLoggingHelper.LogWarning(message);
        }
    }
}