using BarBet.Common.Core.Extensions;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;

namespace BarBet.Common.Core.Test.UnitTests;

[AllureNUnit]
[AllureSuite("BarBet.Common.Core.Test.UnitTests")]
[AllureSubSuite(nameof(DateTimeExtensionTest))]
[AllureDisplayIgnored]
[TestFixture]
public class DateTimeExtensionTest
{
    [Test]
    [TestCaseSource(typeof(DateTimeExtensionTest), nameof(DateTimeExtensionToOffsetTestCaseSource))]
    public void DateTimeExtension_ToOffset_ReturnExpectedDateTimeOffset
        (DateTime dateTime, TimeSpan offset, DateTimeOffset expectedDateTimeOffset)
    {
        // Act
        var actualDateTimeOffset = dateTime.ToOffset(offset);

        // Assert
        Assert.That(actualDateTimeOffset, Is.EqualTo(expectedDateTimeOffset));
    }

    private static IEnumerable<TestCaseData> DateTimeExtensionToOffsetTestCaseSource()
    {
        var offset = TimeSpan.FromHours(10);
        yield return new TestCaseData(
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            offset,
            DateTimeOffset.Parse("2024-01-01T10:00:00.000+10:00")
        );
        yield return new TestCaseData(
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Local),
            offset,
            DateTimeOffset.Parse("2024-01-01T00:00:00.000+10:00").Add(offset.Subtract(DateTimeOffset.Now.Offset))
        );
        yield return new TestCaseData(
            new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified),
            offset,
            DateTimeOffset.Parse("2024-01-01T00:00:00.000+10:00")
        );
        yield return new TestCaseData(
            DateTime.MinValue,
            offset,
            DateTimeOffset.MinValue
        );
    }
}