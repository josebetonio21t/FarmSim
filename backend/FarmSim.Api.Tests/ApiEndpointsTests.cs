using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FarmSim.Api.Tests;

public sealed class ApiEndpointsTests : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;

    public ApiEndpointsTests(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_returns_service_status_and_correlation_id()
    {
        var response = await _client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Headers.Contains("X-Correlation-ID"));

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal("healthy", json.RootElement.GetProperty("status").GetString());
        Assert.Equal("FarmSim.Api", json.RootElement.GetProperty("service").GetString());
    }

    [Fact]
    public async Task Current_sensor_reading_stays_inside_simulation_ranges()
    {
        var reading = await _client.GetFromJsonAsync<JsonElement>("/api/sensors/current");

        Assert.InRange(reading.GetProperty("ph").GetDouble(), 5.8, 6.5);
        Assert.InRange(reading.GetProperty("waterTemperatureC").GetDouble(), 20, 24);
        Assert.InRange(reading.GetProperty("humidityPercent").GetDouble(), 55, 75);
        Assert.InRange(reading.GetProperty("waterLevelPercent").GetDouble(), 65, 100);
    }

    [Fact]
    public async Task Simulate_uses_supplied_overrides()
    {
        var response = await _client.PostAsJsonAsync("/api/sensors/simulate", new
        {
            ph = 6.15,
            waterTemperatureC = 22.4,
            waterLevelPercent = 81.0
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var reading = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(6.15, reading.GetProperty("ph").GetDouble());
        Assert.Equal(22.4, reading.GetProperty("waterTemperatureC").GetDouble());
        Assert.Equal(81.0, reading.GetProperty("waterLevelPercent").GetDouble());
    }

    [Fact]
    public async Task Simulate_rejects_values_outside_physical_limits()
    {
        var response = await _client.PostAsJsonAsync("/api/sensors/simulate", new { ph = 15 });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task Actuator_can_be_updated_and_unknown_name_returns_problem_details()
    {
        var update = await _client.PostAsJsonAsync("/api/actuators/water-pump", new
        {
            isOn = true,
            powerPercent = 80
        });

        Assert.Equal(HttpStatusCode.OK, update.StatusCode);
        var state = await update.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(state.GetProperty("isOn").GetBoolean());
        Assert.Equal(80, state.GetProperty("powerPercent").GetInt32());

        var unknown = await _client.PostAsJsonAsync("/api/actuators/not-real", new
        {
            isOn = true,
            powerPercent = 50
        });

        Assert.Equal(HttpStatusCode.NotFound, unknown.StatusCode);
        Assert.Equal("application/problem+json", unknown.Content.Headers.ContentType?.MediaType);
    }

    [Theory]
    [InlineData("/openapi/v1.json")]
    [InlineData("/scalar/v1")]
    public async Task OpenApi_and_Scalar_are_available_in_development(string route)
    {
        var response = await _client.GetAsync(route);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Swagger_ui_is_not_enabled()
    {
        var response = await _client.GetAsync("/swagger/index.html");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

public sealed class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
    }
}
