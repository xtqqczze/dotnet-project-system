﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

namespace Microsoft.VisualStudio.ProjectSystem.Utilities;

internal sealed class EmptyDisposable : IDisposable
{
    public static IDisposable Instance { get; } = new EmptyDisposable();

    public void Dispose() { }
}
