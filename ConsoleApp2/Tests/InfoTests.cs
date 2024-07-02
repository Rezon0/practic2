using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using NetFrameworkClasses;

namespace Tests
{
    public class InfoTests
    {
        [Test]
        public void GetSystemIdentification_ReturnsCorrectInfo()
        {
            // Arrange
            var info = new Info();

            // Act
            var result = info.GetSystemIdentification();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any()); // ѕровер€ем, что список не пустой

            // ѕровер€ем наличие конкретных ожидаемых строк
            Assert.IsTrue(result.Any(line => line.Contains("OS Name:")));
            Assert.IsTrue(result.Any(line => line.Contains("OS Version:")));
            Assert.IsTrue(result.Any(line => line.Contains("Serial Number:")));
            Assert.IsTrue(result.Any(line => line.Contains("UUID:")));

            // ѕровер€ем, что кажда€ строка в списке не €вл€етс€ пустой или состоит только из пробелов
            Assert.IsFalse(result.Any(line => string.IsNullOrWhiteSpace(line)));

            // ѕровер€ем формат данных
            Assert.IsTrue(result.Any(line => line.StartsWith("OS Name: ")));
            Assert.IsTrue(result.Any(line => line.StartsWith("OS Version: ")));
            Assert.IsTrue(result.Any(line => line.StartsWith("Serial Number: ")));
            Assert.IsTrue(result.Any(line => line.StartsWith("UUID: ")));
        }
    }
}
