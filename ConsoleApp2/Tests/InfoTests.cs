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
            Assert.IsTrue(result.Any()); // ���������, ��� ������ �� ������

            // ��������� ������� ���������� ��������� �����
            Assert.IsTrue(result.Any(line => line.Contains("OS Name:")));
            Assert.IsTrue(result.Any(line => line.Contains("OS Version:")));
            Assert.IsTrue(result.Any(line => line.Contains("Serial Number:")));
            Assert.IsTrue(result.Any(line => line.Contains("UUID:")));

            // ���������, ��� ������ ������ � ������ �� �������� ������ ��� ������� ������ �� ��������
            Assert.IsFalse(result.Any(line => string.IsNullOrWhiteSpace(line)));

            // ��������� ������ ������
            Assert.IsTrue(result.Any(line => line.StartsWith("OS Name: ")));
            Assert.IsTrue(result.Any(line => line.StartsWith("OS Version: ")));
            Assert.IsTrue(result.Any(line => line.StartsWith("Serial Number: ")));
            Assert.IsTrue(result.Any(line => line.StartsWith("UUID: ")));
        }
    }
}
