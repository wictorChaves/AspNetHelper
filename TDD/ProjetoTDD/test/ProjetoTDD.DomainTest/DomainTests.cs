using ExpectedObjects;
using ProjetoTDD.Domain.Courses;

namespace ProjetoTDD.DomainTest
{
    public class DomainTests
    {
        [Fact(DisplayName = "Testing with the ExpectedObjects library from the nuget package")]
        public void Test1()
        {
            // Arrange
            var expected = new
            {
                Name = "Informática básica",
                Workload = 80,
                TargetAudience = "Estudantes",
                Value = 950m
            };

            // Act
            var course = new Course(expected.Name, expected.Workload, expected.TargetAudience, expected.Value);

            // Assert
            expected.ToExpectedObject().ShouldMatch(course);

        }
    }
}
