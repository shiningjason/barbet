using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace BarBet.Common.Core.Enums;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum StatusCode
{
    [Description("General error")]
    COMM9999,

    [Description("Success")]
    COMM0000,

    [Description("Partial success")]
    COMM0001,

    [Description("Invalid parameters")]
    COMM0101,

    [Description("Exceed request limit")]
    COMM0102,

    [Description("Data not found")]
    COMM0103,

    [Description("Unexpected return data")]
    COMM0104,

    [Description("Service unavailable")]
    COMM0105,

    [Description("IP blocked")]
    COMM0106,

    [Description("Network timeout")]
    COMM0107,

    [Description("Network connection issue")]
    COMM0108,

    [Description("Invalid operation")]
    COMM0109,

    [Description("Operation timeout")]
    COMM0110,

    [Description("Operation canceled")]
    COMM0111,

    [Description("Invalid status code")]
    COMM0112,

    [Description("Invalid data")]
    COMM0201,

    [Description("Primary key not found")]
    COMM0202,

    [Description("Primary code not found")]
    COMM0203,

    [Description("Primary key duplicated")]
    COMM0204,

    [Description("Primary code duplicated")]
    COMM0205,

    [Description("Invalid reference ID")]
    COMM0206,

    [Description("Reference ID duplicated")]
    COMM0207
}