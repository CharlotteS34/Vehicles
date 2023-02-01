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

        #region GetVehicle
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
            _vehicleRepository.GetVehicleDetails(vehicle.VehicleId.Value)
                .Returns(Task.FromResult(vehicle));
            var vehicleResult = await _vehicleService.GetVehicle(vehicle.VehicleId.Value);
            vehicleResult.VehicleId.Should().Be(vehicle.VehicleId);
            vehicleResult.Make.Should().Be(vehicle.Make);
            vehicleResult.Model.Should().Be(vehicle.Model);
            vehicleResult.Colour.Should().Be(vehicle.Colour);
            vehicleResult.Year.Should().Be(vehicle.Year);
        }

        #endregion


        #region GetVehicles
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

        #endregion

        #region CreateVehicle

        [Fact]
        public async Task CreateVehicle_ReturnsSuccessFalse_WhenColourDoesNotExist() 
        {
            var vehicle = new VehicleDetailsDTO
            {
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "Red",
                Year = 2015
            };
            _vehicleRepository
                .GetColour(vehicle.Colour)
                .Returns(Task.FromResult((ColourDTO?)null));
            var result = await _vehicleService.CreateVehicle(vehicle);
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid colour Red.");
        }

        [Fact]
        public async Task CreateVehicle_ReturnsSuccessFalse_WhenMakeAndModelComboDoesNotExist()
        {
            var vehicle = new VehicleDetailsDTO
            {
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "Red",
                Year = 2015
            };
            _vehicleRepository
                .GetColour(vehicle.Colour)
                .Returns(Task.FromResult(new ColourDTO { ColourId = 1, ColourName = vehicle.Colour }));
            _vehicleRepository
                .GetModel(vehicle.Model, vehicle.Make)
                .Returns(Task.FromResult((ModelDTO?)null));
            var result = await _vehicleService.CreateVehicle(vehicle);
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid make/model Volkswagen Golf.");
        }

        [Fact]
        public async Task CreateVehicle_ReturnsSuccessTrue_WhenVehicleIsValid()
        {
            var vehicle = new VehicleDetailsDTO
            {
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "Red",
                Year = 2015
            };
            _vehicleRepository
                .GetColour(vehicle.Colour)
                .Returns(Task.FromResult(new ColourDTO { ColourId = 1, ColourName = vehicle.Colour }));
            _vehicleRepository
                .GetModel(vehicle.Model, vehicle.Make)
                .Returns(Task.FromResult(new ModelDTO {  ModelId = 1 }));
            _vehicleRepository
                .CreateVehicle(Arg.Is<VehicleDTO>(x => x.Year == vehicle.Year && x.ModelId == 1 && x.ColourId == 1))
                .Returns(Task.FromResult(new VehicleDTO 
                {
                    VehicleId = 1,
                    Year = vehicle.Year,
                    ModelId = 1,
                    ColourId = 1
                }));
            var result = await _vehicleService.CreateVehicle(vehicle);
            result.Success.Should().BeTrue();
            result.ResultValue.Year.Should().Be(vehicle.Year);
            result.ResultValue.Model.Should().Be(vehicle.Model);
            result.ResultValue.Make.Should().Be(vehicle.Make);
            result.ResultValue.Colour.Should().Be(vehicle.Colour);
        }

        #endregion

        #region Update

        [Fact]
        public async Task UpdateVehicle_ReturnsSuccessFalse_WhenVehicleDoesNotExist() 
        {
            var vehicle = new VehicleDetailsDTO
            {
                VehicleId = 1,
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "Red",
                Year = 2015
            };
            _vehicleRepository
                .GetVehicle(vehicle.VehicleId.Value)
                .Returns(Task.FromResult((VehicleDTO?)null));
            var result = await _vehicleService.UpdateVehicle(vehicle);
            result.Success.Should().BeFalse();
            result.Message.Should().Be($"Vehicle with id {vehicle.VehicleId} not found.");
        }

        [Fact]
        public async Task UpdateVehicle_ReturnsSuccessFalse_WhenColourIsNotNullAndDoesNotExist()
        {
            var vehicleDetails = new VehicleDetailsDTO
            {
                VehicleId = 1,
                Colour = "Red",
                Year = 2015
            };
            _vehicleRepository
                .GetVehicle(vehicleDetails.VehicleId.Value)
                .Returns(Task.FromResult(new VehicleDTO
                {
                    VehicleId = 1,
                    ModelId = 1,
                    ColourId = 1,
                    Year = 2015
                }));
            _vehicleRepository
                .GetColour(vehicleDetails.Colour)
                .Returns(Task.FromResult((ColourDTO?)null));
            var result = await _vehicleService.UpdateVehicle(vehicleDetails);
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid colour Red.");
        }

        [Fact]
        public async Task UpdateVehicle_ReturnsSuccessFalse_WhenMakeIsNullAndModelIsNotNull()
        {
            var vehicleDetails = new VehicleDetailsDTO
            {
                VehicleId = 1,
                Model = "Golf",
                Year = 2015
            };
            _vehicleRepository
                .GetVehicle(vehicleDetails.VehicleId.Value)
                .Returns(Task.FromResult(new VehicleDTO 
                {
                    VehicleId = 1,
                    ModelId = 1,
                    ColourId = 1,
                    Year = 2015
                }));
            var result = await _vehicleService.UpdateVehicle(vehicleDetails);
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid make/model.");
        }

        [Fact]
        public async Task UpdateVehicle_ReturnsSuccessFalse_WhenModelIsNullAndMakeIsNotNull()
        {
            var vehicleDetails = new VehicleDetailsDTO
            {
                VehicleId = 1,
                Make = "Volkswagen",
                Year = 2015
            };
            _vehicleRepository
                .GetVehicle(vehicleDetails.VehicleId.Value)
                .Returns(Task.FromResult(new VehicleDTO
                {
                    VehicleId = 1,
                    ModelId = 1,
                    ColourId = 1,
                    Year = 2015
                }));
            var result = await _vehicleService.UpdateVehicle(vehicleDetails);
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid make/model.");
        }

        [Fact]
        public async Task UpdateVehicle_ReturnsSuccessFalse_WhenMakeAndModelComboDoesNotExist()
        {
            var vehicleDetails = new VehicleDetailsDTO
            {
                VehicleId = 1,
                Make = "Volkswagen",
                Model = "Golf",
                Year = 2015
            };
            _vehicleRepository
                .GetVehicle(vehicleDetails.VehicleId.Value)
                .Returns(Task.FromResult(new VehicleDTO
                {
                    VehicleId = 1,
                    ModelId = 1,
                    ColourId = 1,
                    Year = 2015
                }));
            _vehicleRepository
                .GetModel(vehicleDetails.Model, vehicleDetails.Make)
                .Returns(Task.FromResult((ModelDTO?)null));
            var result = await _vehicleService.UpdateVehicle(vehicleDetails);
            result.Success.Should().BeFalse();
            result.Message.Should().Be("Invalid make/model.");
        }

        [Fact]
        public async Task UpdateVehicle_ReturnsSuccessTrue_WhenVehicleIsValid()
        {
            var vehicleDetails = new VehicleDetailsDTO
            {
                VehicleId = 1,
                Make = "Volkswagen",
                Model = "Golf",
                Colour = "Red",
                Year = 2015
            };
            _vehicleRepository
                .GetVehicle(vehicleDetails.VehicleId.Value)
                .Returns(Task.FromResult(new VehicleDTO
                {
                    VehicleId = 1,
                    ModelId = 1,
                    ColourId = 1,
                    Year = 2015
                }));
            _vehicleRepository
                .GetColour(vehicleDetails.Colour)
                .Returns(Task.FromResult(new ColourDTO { ColourId = 1, ColourName = vehicleDetails.Colour }));
            _vehicleRepository
                .GetModel(vehicleDetails.Model, vehicleDetails.Make)
                .Returns(Task.FromResult(new ModelDTO { ModelId = 1 }));
            _vehicleRepository
                .CreateVehicle(Arg.Is<VehicleDTO>(x => x.Year == vehicleDetails.Year && x.ModelId == 1 && x.ColourId == 1))
                .Returns(Task.FromResult(new VehicleDTO
                {
                    VehicleId = 1,
                    Year = vehicleDetails.Year,
                    ModelId = 1,
                    ColourId = 1
                }));
            var result = await _vehicleService.UpdateVehicle(vehicleDetails);
            result.Success.Should().BeTrue();
        }

        #endregion

        #region Delete

        [Fact]
        public async Task DeleteVehicle_CallsRepositoryDelete()
        {
            await _vehicleService.DeleteVehicle(1);
            await _vehicleRepository
                .Received(1)
                .DeleteVehicle(Arg.Any<int>());
        }

        #endregion
    }
}
