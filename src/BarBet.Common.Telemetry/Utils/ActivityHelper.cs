using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace BarBet.Common.Telemetry.Utils;

public class ActivityHelper
{
    private static ActivitySource? _source;

    public static ActivitySource Source
    {
        get
        {
            if (_source != null) return _source;

            var assemblyName = Assembly.GetExecutingAssembly().GetName();
            _source = new ActivitySource(assemblyName.Name!, assemblyName.Version!.ToString());
            return _source;
        }
    }

    public static string? CurrentId
    {
        get { return Activity.Current?.Id; }
    }

    public static Activity? Start(
        string? operationName = default,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0
    )
    {
        return Start(operationName, default, callerMemberName, callerFilePath, callerLineNumber);
    }

    public static Activity? StartWithParentId(
        string? parentId,
        [CallerMemberName] string callerMemberName = "",
        [CallerFilePath] string callerFilePath = "",
        [CallerLineNumber] int callerLineNumber = 0
    )
    {
        return Start(default, parentId, callerMemberName, callerFilePath, callerLineNumber);
    }

    private static Activity? Start(
        string? operationName = default,
        string? parentId = default,
        string callerMemberName = "",
        string callerFilePath = "",
        int callerLineNumber = 0
    )
    {
        if (string.IsNullOrWhiteSpace(parentId)) parentId = default;
        return Source
            .CreateActivity(operationName ?? callerMemberName, ActivityKind.Internal, parentId)?
            .SetTag("caller.member.name", callerMemberName)
            .SetTag("caller.file.path", callerFilePath)
            .SetTag("caller.file.linenumber", callerLineNumber)
            .Start();
    }
}