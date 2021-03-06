﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PersistNuGetVersionCommand.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Packaging.Publish.Internal.Commands
{
    using System.IO;
    using Sundew.Packaging.Publish.Internal.IO;

    /// <summary>
    /// Stores the NuGet version in a file at the output path.
    /// </summary>
    public class PersistNuGetVersionCommand : IPersistNuGetVersionCommand
    {
        private const string SbpVersionFileName = ".sbpv";
        private readonly IFileSystem fileSystem;

        /// <summary>Initializes a new instance of the <see cref="PersistNuGetVersionCommand"/> class.</summary>
        public PersistNuGetVersionCommand()
            : this(new FileSystem())
        {
        }

        internal PersistNuGetVersionCommand(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        /// <summary>
        /// Saves the specified version.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="outputName">Name of the output.</param>
        /// <param name="commandLogger">The command logger.</param>
        public void Save(string version, string outputPath, string outputName, ICommandLogger commandLogger)
        {
            var filePath = Path.Combine(outputPath, outputName + SbpVersionFileName);
            this.fileSystem.WriteAllText(filePath, version);
            commandLogger.LogInfo($"Wrote version: {version} to {filePath}");
        }
    }
}