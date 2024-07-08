using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using BarBet.Common.Core.Enums;
using BarBet.Common.Core.Utils;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;

namespace BarBet.Common.Core.Test.UnitTests;

[AllureNUnit]
[AllureSuite("BarBet.Common.Core.Test.UnitTests")]
[AllureSubSuite(nameof(StatusCodeHelperTest))]
[AllureDisplayIgnored]
[TestFixture]
public class StatusCodeHelperTest
{
    [Test]
    [TestCaseSource(typeof(StatusCodeHelperTest), nameof(StatusCodeHelperMapFromExceptionTestCaseSource))]
    public void StatusCodeHelper_MapFromException_ReturnExpectedStatusCode
        (Exception exception, StatusCode expectedStatusCode)
    {
        // Act
        var actualStatusCode = StatusCodeHelper.MapFromException(exception);

        // Assert
        Assert.That(actualStatusCode, Is.EqualTo(expectedStatusCode));
    }

    private static IEnumerable<TestCaseData> StatusCodeHelperMapFromExceptionTestCaseSource()
    {
        yield return new TestCaseData(new ValidationException(), StatusCode.COMM0101);
        yield return new TestCaseData(new SocketException(), StatusCode.COMM0105);
        yield return new TestCaseData(new HttpRequestException(), StatusCode.COMM0105);
        yield return new TestCaseData(new OperationCanceledException(), StatusCode.COMM0111);
        yield return new TestCaseData(new TaskCanceledException(), StatusCode.COMM0111);
        yield return new TestCaseData(new Exception("Incorrect string value: '.*' for column"), StatusCode.COMM0101);
        yield return new TestCaseData
            (new Exception("Unable to connect to any of the specified MySQL hosts"), StatusCode.COMM0105);
        yield return new TestCaseData(new Exception("Got timeout reading communication packets"), StatusCode.COMM0105);
        yield return new TestCaseData(new Exception("Couldn't connect to server"), StatusCode.COMM0105);
        yield return new TestCaseData(new Exception("Connect Timeout expired."), StatusCode.COMM0105);
        yield return new TestCaseData
            (new Exception("An exception occurred while receiving a message from the server"), StatusCode.COMM0105);
        yield return new TestCaseData(new Exception("The client reset the request stream"), StatusCode.COMM0105);
        yield return new TestCaseData(new Exception("\"grpc_status\":14"), StatusCode.COMM0105);
        yield return new TestCaseData(new Exception("\"grpc_status\":4"), StatusCode.COMM0110);
        yield return new TestCaseData(new Exception("\"grpc_status\":1"), StatusCode.COMM0111);
        yield return new TestCaseData(new Exception("StatusCode=\"Cancelled\""), StatusCode.COMM0111);
        yield return new TestCaseData(new Exception("COMM0201 : Invalid data"), StatusCode.COMM0201);
        yield return new TestCaseData(new Exception("UNKN0000 : Unknown status code"), StatusCode.COMM9999);
        yield return new TestCaseData(new Exception(), StatusCode.COMM9999);
    }
}