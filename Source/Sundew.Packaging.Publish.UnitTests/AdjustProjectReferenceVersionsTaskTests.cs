﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdjustProjectReferenceVersionsTaskTests.cs" company="Hukano">
// Copyright (c) Hukano. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Packaging.Publish.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using Moq;
    using Sundew.Packaging.Publish;
    using Sundew.Packaging.Publish.Internal.Commands;
    using Sundew.Packaging.Publish.Internal.IO;
    using Xunit;

    public class AdjustProjectReferenceVersionsTaskTests
    {
        private const string ProjectReference = "Reference.csproj";
        private const string DllPath = "Reference.dll";
        private const string AProjectVersion = "3.0.0";
        private readonly AdjustProjectReferenceVersionsTask testee;
        private readonly IFileSystem fileSystem = New.Mock<IFileSystem>();
        private readonly ICommandLogger commandLogger = New.Mock<ICommandLogger>();
        private readonly TaskItem dllTaskItem = new TaskItem(DllPath, new Dictionary<string, string> { { AdjustProjectReferenceVersionsTask.MSBuildSourceProjectFileName, ProjectReference } });
        private readonly TaskItem projectReferenceItem = new TaskItem(ProjectReference, new Dictionary<string, string> { { AdjustProjectReferenceVersionsTask.ProjectVersionName, AProjectVersion } });

        public AdjustProjectReferenceVersionsTaskTests()
        {
            this.testee = new AdjustProjectReferenceVersionsTask(this.fileSystem, this.commandLogger);
        }

        [Fact]
        public void Execute_Then_AdjustedProjectReferencesVersionShouldBeExpectedVersion()
        {
            this.testee.ResolvedProjectReferences = new ITaskItem[] { this.dllTaskItem, };
            this.testee.ProjectReferences = new ITaskItem[] { this.projectReferenceItem, };
            this.fileSystem.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            const string expectedVersion = "3.0.0-pre-u20201010-150729";
            this.fileSystem.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(expectedVersion);

            this.testee.Execute();

            this.testee.AdjustedProjectReferences.FirstOrDefault()!.GetMetadata(AdjustProjectReferenceVersionsTask.ProjectVersionName).Should().Be(expectedVersion);
            this.commandLogger.Verify(x => x.LogInfo(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Execute_When_VersionDoesNotChange_Then_LogInfoShouldNotBeCalled()
        {
            this.testee.ResolvedProjectReferences = new ITaskItem[] { this.dllTaskItem, };
            this.testee.ProjectReferences = new ITaskItem[] { this.projectReferenceItem, };
            this.fileSystem.Setup(x => x.FileExists(It.IsAny<string>())).Returns(true);
            this.fileSystem.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns(AProjectVersion);

            this.testee.Execute();

            this.commandLogger.Verify(x => x.LogInfo(It.IsAny<string>()), Times.Never);
        }
    }
}