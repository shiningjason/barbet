using BarBet.Application.Web.Utilities;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using NUnit.Framework;

namespace BarBet.Web.Test.UnitTests;

[AllureNUnit]
[AllureSuite("BarBet.Application.Web.Test.UnitTests")]
[AllureSubSuite(nameof(AutoMapperTest))]
[AllureDisplayIgnored]
[TestFixture]
public class AutoMapperTest
{
    [Test]
    public void AutoMapper_Configuration_IsValid()
    {
        AutoMapperProvider.Mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }
}