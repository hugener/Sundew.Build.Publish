﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdjustProjectReferenceVersionsTask.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Build.Publish
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Sundew.Build.Publish.Internal.IO;

    /// <summary>
    /// Task for adjusting project reference versions.
    /// </summary>
    public class AdjustProjectReferenceVersionsTask : Task
    {
        private const string MSBuildSourceProjectFileName = "MSBuildSourceProjectFile";
        private const string SundewBuildPublishVersionFileExtension = "sbpv";
        private const string ProjectVersionName = "ProjectVersion";
        private readonly IFileSystem fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdjustProjectReferenceVersionsTask"/> class.
        /// </summary>
        public AdjustProjectReferenceVersionsTask()
            : this(new FileSystem())
        {
        }

        internal AdjustProjectReferenceVersionsTask(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        /// <summary>
        /// Gets or sets the resolved project references.
        /// </summary>
        /// <value>
        /// The resolved project references.
        /// </value>
        [Required]
        public ITaskItem[] ResolvedProjectReferences { get; set; }

        /// <summary>
        /// Gets or sets the project references.
        /// </summary>
        /// <value>
        /// The project references.
        /// </value>
        [Required]
        public ITaskItem[] ProjectReferences { get; set; }

        /// <summary>
        /// Gets the adjusted project references.
        /// </summary>
        /// <value>
        /// The adjusted project references.
        /// </value>
        [Output]
        public ITaskItem[] AdjustedProjectReferences { get; private set; }

        /// <summary>
        /// Must be implemented by derived class.
        /// </summary>
        /// <returns>
        /// true, if successful.
        /// </returns>
        public override bool Execute()
        {
            try
            {
                foreach (var projectReference in this.ProjectReferences)
                {
                    var resolvedProjectReference = this.ResolvedProjectReferences.FirstOrDefault(x =>
                        x.GetMetadata(MSBuildSourceProjectFileName) == projectReference.ItemSpec);

                    if (resolvedProjectReference == null)
                    {
                        continue;
                    }

                    var assemblyVersionFile = Path.ChangeExtension(resolvedProjectReference.ItemSpec, SundewBuildPublishVersionFileExtension);
                    if (this.fileSystem.FileExists(assemblyVersionFile))
                    {
                        var packageVersion = File.ReadAllText(assemblyVersionFile);
                        if (!string.IsNullOrEmpty(packageVersion))
                        {
                            projectReference.SetMetadata(ProjectVersionName, packageVersion);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.Log.LogWarningFromException(e);
            }

            this.AdjustedProjectReferences = this.ProjectReferences;
            return true;
        }
    }
}