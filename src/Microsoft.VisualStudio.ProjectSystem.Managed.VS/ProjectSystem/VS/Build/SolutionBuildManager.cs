﻿// Licensed to the .NET Foundation under one or more agreements. The .NET Foundation licenses this file to you under the MIT license. See the LICENSE.md file in the project root for more information.

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Threading;

namespace Microsoft.VisualStudio.ProjectSystem.VS.Build;

[Export(typeof(ISolutionBuildManager))]
internal sealed class SolutionBuildManager : OnceInitializedOnceDisposedAsync, ISolutionBuildManager
{
    private readonly IVsService<SVsSolutionBuildManager, IVsSolutionBuildManager2> _vsSolutionBuildManagerService;
    private readonly ReentrantSemaphore _semaphore;

    private IVsSolutionBuildManager2? _vsSolutionBuildManager2;
    private IVsSolutionBuildManager3? _vsSolutionBuildManager3;

    [ImportingConstructor]
    public SolutionBuildManager(
        IVsService<SVsSolutionBuildManager, IVsSolutionBuildManager2> solutionBuildManagerService,
        JoinableTaskContext joinableTaskContext)
        : base(new(joinableTaskContext))
    {
        _vsSolutionBuildManagerService = solutionBuildManagerService;
        _semaphore = ReentrantSemaphore.Create(
            initialCount: 1,
            joinableTaskContext: joinableTaskContext,
            mode: ReentrantSemaphore.ReentrancyMode.NotAllowed);
    }

    protected override async Task InitializeCoreAsync(CancellationToken cancellationToken)
    {
        await JoinableFactory.SwitchToMainThreadAsync(cancellationToken);

        _vsSolutionBuildManager2 = await _vsSolutionBuildManagerService.GetValueAsync(cancellationToken);
        _vsSolutionBuildManager3 = (IVsSolutionBuildManager3)_vsSolutionBuildManager2;
    }

    protected override Task DisposeCoreAsync(bool initialized)
    {
        return Task.CompletedTask;
    }

    public async Task<IAsyncDisposable> SubscribeSolutionEventsAsync(IVsUpdateSolutionEvents eventListener)
    {
        await InitializeAsync();

        Assumes.NotNull(_vsSolutionBuildManager2);
        Assumes.NotNull(_vsSolutionBuildManager3);

        await JoinableFactory.SwitchToMainThreadAsync();

        uint cookie3 = VSConstants.VSCOOKIE_NIL;

        HResult.Verify(
            _vsSolutionBuildManager2.AdviseUpdateSolutionEvents(eventListener, out uint cookie2),
            $"Error advising solution events in {typeof(SolutionBuildManager)}.");

        if (eventListener is IVsUpdateSolutionEvents3 events3)
        {
            int hr = _vsSolutionBuildManager3.AdviseUpdateSolutionEvents3(events3, out cookie3);

            if (hr != HResult.OK)
            {
                // The first subscription succeeded while the second failed.
                // We need to clean up the first subscription before throwing an exception.
                _vsSolutionBuildManager2.UnadviseUpdateSolutionEvents(cookie2);

                // Throw.
                HResult.Verify(hr, $"Error advising solution events 3 in {typeof(SolutionBuildManager)}.");
            }
        }

        return new AsyncDisposable(async () =>
        {
            await JoinableFactory.SwitchToMainThreadAsync();

            int result2 = HResult.OK;
            int result3 = HResult.OK;

            if (cookie2 != VSConstants.VSCOOKIE_NIL)
            {
                result2 = _vsSolutionBuildManager2.UnadviseUpdateSolutionEvents(cookie2);
            }

            if (cookie3 != VSConstants.VSCOOKIE_NIL)
            {
                result3 = _vsSolutionBuildManager3.UnadviseUpdateSolutionEvents3(cookie3);
            }

            // Defer any exception until this point, so that we ensure both subscriptions
            // have a chance to be cleaned up.
            HResult.Verify(result2, $"Error unadvising solution events 2 in {typeof(SolutionBuildManager)}.");
            HResult.Verify(result3, $"Error unadvising solution events 3 in {typeof(SolutionBuildManager)}.");
        });
    }

    public int QueryBuildManagerBusy()
    {
        Assumes.NotNull(_vsSolutionBuildManager2);

        JoinableFactory.Context.VerifyIsOnMainThread();

        ErrorHandler.ThrowOnFailure(_vsSolutionBuildManager2.QueryBuildManagerBusy(out int flags));

        return flags;
    }

    public uint QueryBuildManagerBusyEx()
    {
        Assumes.NotNull(_vsSolutionBuildManager3);

        JoinableFactory.Context.VerifyIsOnMainThread();

        ErrorHandler.ThrowOnFailure(_vsSolutionBuildManager3.QueryBuildManagerBusyEx(out uint flags));

        return flags;
    }

    public void SaveDocumentsBeforeBuild(IVsHierarchy hierarchy, uint itemId, uint docCookie)
    {
        Assumes.NotNull(_vsSolutionBuildManager2);

        JoinableFactory.Context.VerifyIsOnMainThread();

        ErrorHandler.ThrowOnFailure(_vsSolutionBuildManager2.SaveDocumentsBeforeBuild(hierarchy, itemId, docCookie));
    }

    public void CalculateProjectDependencies()
    {
        Assumes.NotNull(_vsSolutionBuildManager2);

        JoinableFactory.Context.VerifyIsOnMainThread();

        ErrorHandler.ThrowOnFailure(_vsSolutionBuildManager2.CalculateProjectDependencies());
    }

    public IVsHierarchy[] GetProjectDependencies(IVsHierarchy hierarchy)
    {
        Assumes.NotNull(_vsSolutionBuildManager2);

        JoinableFactory.Context.VerifyIsOnMainThread();

        // Find out how many dependent projects there are
        uint[] dependencyCounts = new uint[1];
        ErrorHandler.ThrowOnFailure(_vsSolutionBuildManager2.GetProjectDependencies(hierarchy, 0, null, dependencyCounts));
        uint count = dependencyCounts[0];

        if (count == 0)
        {
            return [];
        }

        // Get all of the dependent projects, and add them to our list
        var projectsArray = new IVsHierarchy[count];
        ErrorHandler.ThrowOnFailure(_vsSolutionBuildManager2.GetProjectDependencies(hierarchy, count, projectsArray, dependencyCounts));
        return projectsArray;
    }

    public void StartUpdateSpecificProjectConfigurations(IVsHierarchy[] hierarchy, uint[] buildFlags, uint dwFlags)
    {
        Assumes.NotNull(_vsSolutionBuildManager2);

        JoinableFactory.Context.VerifyIsOnMainThread();

        ErrorHandler.ThrowOnFailure(_vsSolutionBuildManager2.StartUpdateSpecificProjectConfigurations(
            cProjs: (uint)hierarchy.Length,
            rgpHier: hierarchy,
            rgpcfg: null,
            rgdwCleanFlags: null,
            rgdwBuildFlags: buildFlags,
            rgdwDeployFlags: null,
            dwFlags: dwFlags,
            fSuppressUI: 0));
    }

    public async ValueTask<bool> BuildProjectAndWaitForCompletionAsync(
        IVsHierarchy projectHierarchy,
        CancellationToken cancellationToken = default)
    {
        return await _semaphore.ExecuteAsync(async () => await BuildProjectCoreAsync(), cancellationToken);

        async Task<bool> BuildProjectCoreAsync()
        {
            await InitializeAsync();
            Assumes.NotNull(_vsSolutionBuildManager2);
            await JoinableFactory.SwitchToMainThreadAsync(cancellationToken);
            using var solutionBuildCompleteListener = new SolutionBuildCompleteListener();
            await using var _ = await SubscribeSolutionEventsAsync(solutionBuildCompleteListener);
            var result = _vsSolutionBuildManager2.StartSimpleUpdateProjectConfiguration(
                pIVsHierarchyToBuild: projectHierarchy,
                pIVsHierarchyDependent: null,
                pszDependentConfigurationCanonicalName: null,
                dwFlags: (uint)VSSOLNBUILDUPDATEFLAGS.SBF_OPERATION_BUILD,
                dwDefQueryResults: (uint)VSSOLNBUILDQUERYRESULTS.VSSBQR_SAVEBEFOREBUILD_QUERY_YES,
                fSuppressUI: 0);

            ErrorHandler.ThrowOnFailure(result);

            return await solutionBuildCompleteListener.WaitForSolutionBuildCompletedAsync(cancellationToken);
        }
    }

    private class SolutionBuildCompleteListener() : IVsUpdateSolutionEvents, IDisposable
    {
        private TaskCompletionSource<bool> _buildCompletedSource = new();

        public int UpdateSolution_Begin(ref int pfCancelUpdate)
        {
            _buildCompletedSource = new TaskCompletionSource<bool>();
            return HResult.OK;
        }

        public int UpdateSolution_Done(int fSucceeded, int fModified, int fCancelCommand)
        {
            _buildCompletedSource.TrySetResult(fSucceeded != 0);

            return HResult.OK;
        }

        public int UpdateSolution_StartUpdate(ref int pfCancelUpdate)
        {
            return HResult.OK;
        }

        public int UpdateSolution_Cancel()
        {
            _buildCompletedSource.TrySetCanceled();

            return HResult.OK;
        }

        public int OnActiveProjectCfgChange(IVsHierarchy pIVsHierarchy)
        {
            return HResult.OK;
        }

        public async Task<bool> WaitForSolutionBuildCompletedAsync(CancellationToken ct = default)
        {
            using var _ = ct.Register(() =>
            {
                _buildCompletedSource.TrySetCanceled();
            });

            try
            {
                return await _buildCompletedSource.Task;
            }
            catch (TaskCanceledException)
            {
                return false;
            }
        }

        public void Dispose()
        {
            _buildCompletedSource.TrySetCanceled();
        }
    }
}
