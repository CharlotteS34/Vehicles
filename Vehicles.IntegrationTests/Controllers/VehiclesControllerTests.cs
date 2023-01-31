using Azure;
using FluentAssertions;
using System.Dynamic;
using System.Net;
using System.Net.Http.Json;
using Vehicles.Api.Models.Requests;
using Vehicles.Common.Extensions;
using Vehicles.Common.Models;
using Vehicles.Repository.Entities;

namespace Vehicles.IntegrationTests.Controllers
{
    public class VehiclesControllerTests : ControllerTestBase
    {
        private const string _baseUrl = "vehicle";

        [Fact]
        public async Task GetVehicles_ReturnsStatusCodeOK() 
        {
            var response = await _client.GetAsync(_baseUrl);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetVehicle_ReturnsStatusCodeNotFound_WhenVehicleCannotBeFound() 
        {
            var response = await _client.GetAsync($"{_baseUrl}/1");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var message = await response.Content.ReadAsStringAsync();
            message.Should().Be("Vehicle with id 1 could not be found.");
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenYearIsNotValid() 
        {
            var request = new CreateVehicleRequest 
            {
                Year = 1800
            };
            var response = await _client.PostAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PostVehicle_ReturnsCreatedStatusCode_AndNewVehicleWithId() 
        {
            var vehicle = new CreateVehicleRequest 
            {
                ModelId = 1,
                ColourId = 1,
                Year = 2015,
            };
            var response = await _client.PostAsJsonAsync(_baseUrl, vehicle);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var vehicleResult = await response.GetResponseBodyAsync<VehicleDTO>();
            vehicleResult.VehicleId.Should().NotBeNull();
            vehicleResult.ModelId.Should().Be(vehicle.ModelId);
            vehicleResult.ColourId.Should().Be(vehicle.ColourId);
            vehicleResult.Year.Should().Be(vehicle.Year);
        }


        [Fact]
        public async Task GetVehicle_ReturnsStatusCodeOK__AndVehicle_WhenVehicleExists()
        {
            var vehicle = new CreateVehicleRequest
            {
                ModelId = 1,
                ColourId = 1,
                Year = 2015,
            };
            var createVehicleResponse = await _client.PostAsJsonAsync(_baseUrl, vehicle);
            var vehicleId = (await createVehicleResponse.GetResponseBodyAsync<VehicleDTO>()).VehicleId;
            var getVehicleResponse = await _client.GetAsync($"{_baseUrl}/{vehicleId}");
            getVehicleResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var vehicleResult = await getVehicleResponse.GetResponseBodyAsync<VehicleDTO>();
            vehicleResult.VehicleId.Should().NotBeNull();
            vehicleResult.ModelId.Should().Be(vehicle.ModelId);
            vehicleResult.ColourId.Should().Be(vehicle.ColourId);
            vehicleResult.Year.Should().Be(vehicle.Year);
        }
    }
}
