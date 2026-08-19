using FluentAssertions;

using RunningRacesApi.Models.DTOs;
using RunningRacesApi.Services;

namespace RunningRacesApi.Tests.Services;

public class CsvExportServiceTests
{
    private readonly CsvExportService _service;
    public CsvExportServiceTests()
    {
        _service = new CsvExportService();
    }

    [Theory]
    [InlineData(new[] { "Order" }, "Order")]
    [InlineData(new[] { "Order", "Distance" }, "Order;Distance")]
    [InlineData(new[] { "Distance", "Order" }, "Distance;Order")]
    [InlineData(new[] { "StartWayPointName", "Distance", "Order" }, "StartWayPointName;Distance;Order")]
    public void Export_GeneratesCorrectHeader(string[] columns, string expectedHeader)
    {
        var data = new List<SectionExportDto> { new SectionExportDto {
         Order = 1,
         Distance = 10,
         StartWayPointName = "Zánka"
        } };
        var result = _service.Export(data, columns);
        var lines = result.Split("\n");
        lines[0].Trim().Should().Be(expectedHeader);
    }

    [Fact]
    public void Export_ColumnOrder()
    {
        var data = new List<SectionExportDto> { new SectionExportDto {
         Order = 1,
         Distance = 10
        } };

        var columns = new List<string> { "Distance", "Order" };

        var result = _service.Export(data, columns);

        var lines = result.Split("\n");
        lines[1].Trim().Should().Be("10;1");
    }

    [Fact]
    public void Export_HandlesNullValues()
    {
        var data = new List<SectionExportDto> {
            new SectionExportDto {
                Order = 1,
                Distance = 10,
                Description = null,
                StartWayPointName= null
            }
        };

        var columns = new List<string>
        {
            "Order",
            "Distance",
            "Description",
            "StartWayPointName"
        };

        var result = _service.Export(data, columns);

        var lines = result.Split("\n");
        lines[1].Trim().Should().Be("1;10;;");
    }
}
