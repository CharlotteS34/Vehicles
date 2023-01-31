using FluentAssertions;
using NSubstitute;
using Vehicles.Business.Services;
using Vehicles.Common.Models;
using Vehicles.Common.Repositories;

namespace Vehicles.Business.Tests.Services
{
    public class VehicleServiceTests
    {
        private readonly VehicleService _vehicleService;
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleServiceTests() 
        {
            _vehicleRepository = Substitute.For<IVehicleRepository>();
            _vehicleService = new VehicleService(_vehicleRepository);
        }

        [Fact]
        public async Task GetVehicle_ReturnsVehicleFromRepository() 
        {
            var vehicle = new VehicleDetailsDTO 
            {
                VehicleId = 1,
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "Red",
                Year = 2015
            };
            _vehicleRepository.GetVehicle(vehicle.VehicleId.Value)
                .Returns(Task.FromResult(vehicle));
            var vehicleResult = await _vehicleService.GetVehicle(vehicle.VehicleId.Value);
            vehicleResult.VehicleId.Should().Be(vehicle.VehicleId);
            vehicleResult.Make.Should().Be(vehicle.Make);
            vehicleResult.Model.Should().Be(vehicle.Model);
            vehicleResult.Colour.Should().Be(vehicle.Colour);
            vehicleResult.Year.Should().Be(vehicle.Year);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        public async Task GetVehicles_ReturnsSameVehicleCountAsRepository(int vehicleCount) 
        {
            var vehicles = Enumerable.Range(1, vehicleCount)
                .Select(index => new VehicleDetailsDTO());
            _vehicleRepository
                .GetVehicles()
                .Returns(Task.FromResult(vehicles.AsEnumerable()));
            var vehicleResults = await _vehicleService.GetVehicles();
            vehicleResults.Count().Should().Be(vehicleCount);
        }

        [Fact]
        public async Task GetVehicles_ReturnsVehiclesFromRepository()
        {
            var vehicle = new VehicleDetailsDTO
            {
                VehicleId = 1,
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "Red",
                Year = 2015
            };
            var vehicles = new VehicleDetailsDTO[1] 
            {
                vehicle
            };
            _vehicleRepository
                .GetVehicles()
                .Returns(Task.FromResult(vehicles.AsEnumerable()));
            var vehicleResult = (await _vehicleService.GetVehicles()).FirstOrDefault();
            vehicleResult.Should().NotBeNull();
            vehicleResult.VehicleId.Should().Be(vehicle.VehicleId);
            vehicleResult.Make.Should().Be(vehicle.Make);
            vehicleResult.Model.Should().Be(vehicle.Model);
            vehicleResult.Colour.Should().Be(vehicle.Colour);
            vehicleResult.Year.Should().Be(vehicle.Year);
        }
    }
}
