﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

using Microsoft.VisualStudio.ProjectSystem.Query;
using Microsoft.VisualStudio.ProjectSystem.Query.Execution;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Query;

/// <summary>
/// Handles retrieving a set of <see cref="IConfigurationDimensionSnapshot"/>s from an <see cref="ProjectSystem.Properties.IProperty"/>.
/// </summary>
/// <remarks>
/// The <see cref="ProjectSystem.Properties.IProperty"/> comes from the parent <see cref="IUIPropertyValueSnapshot"/>
/// </remarks>
internal class ConfigurationDimensionFromPropertyDataProducer : QueryDataFromProviderStateProducerBase<PropertyValueProviderState>
{
    private readonly IConfigurationDimensionPropertiesAvailableStatus _properties;

    public ConfigurationDimensionFromPropertyDataProducer(IConfigurationDimensionPropertiesAvailableStatus properties)
    {
        _properties = properties;
    }

    protected override Task<IEnumerable<IEntityValue>> CreateValuesAsync(IQueryExecutionContext queryExecutionContext, IEntityValue parent, PropertyValueProviderState providerState)
    {
        return Task.FromResult(ConfigurationDimensionDataProducer.CreateProjectConfigurationDimensions(
            parent,
            providerState.ProjectConfiguration,
            providerState.Property,
            _properties));
    }
}
