using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Vehicles.Api.Models.Requests;
using Vehicles.Common.Extensions;
using Vehicles.Common.Models;

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Post_ReturnsBadRequest_WhenColourIsNullOrEmpty(string colour)
        {
            var request = new CreateVehicleRequest
            {
                Year = 2012,
                Make = "Volkswagen",
                Model = "Golf",
                Colour = colour
            };
            var response = await _client.PostAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Post_ReturnsBadRequest_WhenMakeIsNullOrEmpty(string make)
        {
            var request = new CreateVehicleRequest
            {
                Year = 2012,
                Make = make,
                Model = "Golf",
                Colour = "Red"
            };
            var response = await _client.PostAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Post_ReturnsBadRequest_WhenModelIsNullOrEmpty(string model)
        {
            var request = new CreateVehicleRequest
            {
                Year = 2012,
                Make = "Volkswagen",
                Model = model,
                Colour = "Red"
            };
            var response = await _client.PostAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenColourDoesNotExist()
        {
            var request = new CreateVehicleRequest
            {
                Year = 2012,
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "White"
            };
            var response = await _client.PostAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var message = await response.Content.ReadAsStringAsync();
            message.Should().Be("Invalid colour White.");
        }

        [Fact]
        public async Task Post_ReturnsBadRequest_WhenMakeAndModelComboDoesNotExist()
        {
            var request = new CreateVehicleRequest
            {
                Year = 2012,
                Make = "Volkswagen",
                Model = "A6",
                Colour = "Red"
            };
            var response = await _client.PostAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var message = await response.Content.ReadAsStringAsync();
            message.Should().Be("Invalid make/model Volkswagen A6.");
        }

        [Fact]
        public async Task Post_ReturnsCreated_WhenVehicleIsValid()
        {
            var request = new CreateVehicleRequest
            {
                Year = 2012,
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "Red"
            };
            var response = await _client.PostAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var vehicleResult = await response.GetResponseBodyAsync<VehicleDetailsDTO>();
            vehicleResult.VehicleId.Should().NotBeNull();
            vehicleResult.Make.Should().Be(request.Make);
            vehicleResult.Model.Should().Be(request.Model);
            vehicleResult.Colour.Should().Be(request.Colour);
            vehicleResult.Year.Should().Be(request.Year);
        }

        [Fact]
        public async Task GetVehicle_ReturnsStatusCodeOKAndVehicleDetails_WhenVehicleHasBeenCreated() 
        {
            var vehicle = await CreateVehicle();
            var response = await _client.GetAsync($"{_baseUrl}/{vehicle.VehicleId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var vehicleResult = await response.GetResponseBodyAsync<VehicleDetailsDTO>();
            vehicleResult.VehicleId.Should().NotBeNull();
            vehicleResult.Make.Should().Be(vehicle.Make);
            vehicleResult.Model.Should().Be(vehicle.Model);
            vehicleResult.Colour.Should().Be(vehicle.Colour);
            vehicleResult.Year.Should().Be(vehicle.Year);
        }

        [Fact]
        public async Task DeleteVehicle_ReturnsStatusCodeOK_WhenVehicleDoesNotExist() 
        {
            var response = await _client.DeleteAsync($"{_baseUrl}/1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_ReturnsOK_WhenVehicleExists() 
        {
            var vehicle = await CreateVehicle();
            var deleteVehicleResponse = await _client.DeleteAsync($"{_baseUrl}/{vehicle.VehicleId}");
            deleteVehicleResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenYearIsNotValid() 
        {
            var request = new UpdateVehicleRequest { VehicleId = 1 };
            var response = await _client.PutAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenUpdatingVehicleWhichIsNotThere() 
        {
            var request = new UpdateVehicleRequest { VehicleId = 1, Year = 2012 };
            var response = await _client.PutAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var message = await response.Content.ReadAsStringAsync();
            message.Should().Be($"Vehicle with id 1 not found.");
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenColourDoesNotExist() 
        {
            var vehicle = await CreateVehicle();
            var request = new UpdateVehicleRequest 
            { 
                VehicleId = vehicle.VehicleId.Value, 
                Year = 2012,
                Colour = "White"
            };
            var response = await _client.PutAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var message = await response.Content.ReadAsStringAsync();
            message.Should().Be($"Invalid colour {request.Colour}.");
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenMakeIsNullAndModelIsNotNull()
        {
            var vehicle = await CreateVehicle();
            var request = new UpdateVehicleRequest
            {
                VehicleId = vehicle.VehicleId.Value,
                Year = 2012,
                Model = "Tuscon"
            };
            var response = await _client.PutAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var message = await response.Content.ReadAsStringAsync();
            message.Should().Be("Invalid make/model.");
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenModelIsNullAndMakeIsNotNull()
        {
            var vehicle = await CreateVehicle();
            var request = new UpdateVehicleRequest
            {
                VehicleId = vehicle.VehicleId.Value,
                Year = 2012,
                Make = "Hyundai"
            };
            var response = await _client.PutAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var message = await response.Content.ReadAsStringAsync();
            message.Should().Be("Invalid make/model.");
        }

        [Fact]
        public async Task Put_ReturnsBadRequest_WhenMakeAndModelComboDoesNotExist()
        {
            var vehicle = await CreateVehicle();
            var request = new UpdateVehicleRequest
            {
                VehicleId = vehicle.VehicleId.Value,
                Year = 2012,
                Make = "Hyundai",
                Model = "Tuscon"
            };
            var response = await _client.PutAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var message = await response.Content.ReadAsStringAsync();
            message.Should().Be("Invalid make/model.");
        }

        [Fact]
        public async Task Put_ReturnsOK_WhenVehicleIsValid_AndShouldUpdateVehicle()
        {
            var vehicle = await CreateVehicle();
            var request = new UpdateVehicleRequest
            {
                VehicleId = vehicle.VehicleId.Value,
                Year = 2013,
                Make = "Audi",
                Model = "A6",
                Colour = "Red"
            };
            var response = await _client.PutAsJsonAsync(_baseUrl, request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var updatedVehicle = await GetVehicle(request.VehicleId);
            updatedVehicle.Year.Should().Be(request.Year);
            updatedVehicle.Make.Should().Be(request.Make);
            updatedVehicle.Model.Should().Be(request.Model);
            updatedVehicle.Colour.Should().Be(request.Colour);
        }

        private async Task<VehicleDetailsDTO> CreateVehicle() 
        {
            var request = new CreateVehicleRequest
            {
                Year = 2012,
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "Red"
            };
            var createVehicleResponse = await _client.PostAsJsonAsync(_baseUrl, request);
            return await createVehicleResponse.GetResponseBodyAsync<VehicleDetailsDTO>();
        }

        private async Task<VehicleDetailsDTO> GetVehicle(int vehicleId) 
        {
            var response = await _client.GetAsync($"{_baseUrl}/{vehicleId}");
            return await response.GetResponseBodyAsync<VehicleDetailsDTO>();
        }
    }
}
