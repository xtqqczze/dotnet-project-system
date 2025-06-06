﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Microsoft.VisualStudio.ProjectSystem;

internal static class IActiveConfigurationGroupServiceFactory
{
    public static IActiveConfigurationGroupService Implement(IProjectValueDataSource<IConfigurationGroup<ProjectConfiguration>> source)
    {
        var mock = new Mock<IActiveConfigurationGroupService>();
        mock.SetupGet(s => s.ActiveConfigurationGroupSource)
            .Returns(source);

        return mock.Object;
    }
}
