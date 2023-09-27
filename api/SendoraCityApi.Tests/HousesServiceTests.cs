using FluentAssertions;
using Moq;
using SendoraCityApi.Repositories.Database.Models;
using SendoraCityApi.Services;
using SendoraCityApi.Services.Models;
using Xunit;

namespace SendoraCityApi.Tests;

public class HousesServiceTests : ServiceTests
{

    private readonly IHousesService _housesService;

    public HousesServiceTests()
        => _housesService = new HousesService(
            _citiesRepositoryMock.Object,
            _housesRepositoryMock.Object,
            _storesRepositoryMock.Object
            )
        { };

    [Fact]
    public void When_GetHouseByIdAsync_called_with_id_invalid_then_throw()
    {
        var requestedId = 2;
        var returnedHouse = new House
        {
            Id = 1,
            Address = "1 rue de Paris",
            Cityid = 1,
            Inhabitants = 1
        };
        var expectedResponse = new HouseResponse(returnedHouse);

        _housesRepositoryMock.Setup(x => x.GetHouseByIdAsync(requestedId)).ReturnsAsync(returnedHouse);

        Func<Task> response = () => _housesService.GetHouseByIdAsync(requestedId);
        response.Should().ThrowExactlyAsync<ArgumentOutOfRangeException>($"House with id {requestedId} not found");

        _housesRepositoryMock.Verify(x => x.GetHouseByIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public async Task When_GetHouseByIdAsync_called_with_id_valid_then_returns_house()
    {
        var requestedId = 1;
        var returnedHouse = new House
        {
            Id = 1,
            Address = "1 rue de Paris",
            Cityid = 1,
            Inhabitants = 1
        };
        var expectedResponse = new HouseResponse(returnedHouse);

        _housesRepositoryMock.Setup(x => x.GetHouseByIdAsync(requestedId)).ReturnsAsync(returnedHouse);

        var response = await _housesService.GetHouseByIdAsync(requestedId);
        response.Should().BeEquivalentTo(expectedResponse);

        _housesRepositoryMock.Verify(x => x.GetHouseByIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public void When_AddHouseAsync_called_with_cityid_invalid_then_throw()
    {
        var requestedCityId = 1;
        var requestedAddress = "1 rue de Paris";
        var houseCreateRequest = new HouseCreateRequest
        {
            Address = requestedAddress,
            Cityid = requestedCityId,
            Inhabitants = 1
        };

        Func<Task> response = () => _housesService.AddHouseAsync(houseCreateRequest);
        response.Should().ThrowExactlyAsync<ArgumentException>($"City with id {requestedCityId} not found");

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedCityId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void When_AddHouseAsync_called_with_address_already_used_in_city_then_throw(bool house, bool store)
    {
        var requestedCityId = 1;
        var requestedAddress = "1 rue de Paris";
        var houseCreateRequest = new HouseCreateRequest
        {
            Address = requestedAddress,
            Cityid = requestedCityId,
            Inhabitants = 1
        };
        var requestedCity = new City
        {
            Id = requestedCityId,
            Name = "Paris",
            Touristic = true
        };
        var existingHouses = new List<House>
        {
            new()
            {
                Id = 2,
                Address = requestedAddress,
                Cityid = requestedCityId,
                Inhabitants = 1
            }
        };
        var existingStores = new List<Store>
        {
            new()
            {
                Id = 2,
                Address = requestedAddress,
                Cityid = requestedCityId,
                Name = "Store 1",
                Type = "Other"
            }
        };

        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedCityId)).ReturnsAsync(requestedCity);
        _housesRepositoryMock.Setup(x => x.GetHousesByCityIdAsync(requestedCityId)).ReturnsAsync(house ? existingHouses : new List<House>());
        _storesRepositoryMock.Setup(x => x.GetStoresByCityIdAsync(requestedCityId)).ReturnsAsync(store ? existingStores : new List<Store>());

        Func<Task> response = () => _housesService.AddHouseAsync(houseCreateRequest);
        response.Should().ThrowExactlyAsync<ArgumentException>($"Building with address {requestedAddress} already exists in this city ({requestedCityId})");

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedCityId), Times.Once);
        _housesRepositoryMock.Verify(x => x.GetHousesByCityIdAsync(requestedCityId), Times.Once);
        _storesRepositoryMock.Verify(x => x.GetStoresByCityIdAsync(requestedCityId), house ? Times.Never : Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public async Task When_AddHouseAsync_called_with_address_available_in_city_then_return_house()
    {
        var requestedCityId = 1;
        var requestedAddress = "1 rue de Paris";
        var houseCreateRequest = new HouseCreateRequest
        {
            Address = requestedAddress,
            Cityid = requestedCityId,
            Inhabitants = 1
        };
        var expectedHouse = new House
        {
            Id = 1,
            Address = requestedAddress,
            Cityid = requestedCityId,
            Inhabitants = 1
        };
        var requestedCity = new City
        {
            Id = requestedCityId,
            Name = "Paris",
            Touristic = true
        };
        var expectedResponse = new HouseResponse(expectedHouse);

        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedCityId)).ReturnsAsync(requestedCity);
        _housesRepositoryMock.Setup(x => x.GetHousesByCityIdAsync(requestedCityId)).ReturnsAsync(new List<House>());
        _storesRepositoryMock.Setup(x => x.GetStoresByCityIdAsync(requestedCityId)).ReturnsAsync(new List<Store>());
        _housesRepositoryMock.Setup(x => x.AddHouseAsync(It.Is<House>(house => house.Address == requestedAddress && house.Cityid == requestedCityId))).ReturnsAsync(expectedHouse);

        var response = await _housesService.AddHouseAsync(houseCreateRequest);
        response.Should().BeEquivalentTo(expectedResponse);

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedCityId), Times.Once);
        _housesRepositoryMock.Verify(x => x.GetHousesByCityIdAsync(requestedCityId), Times.Once);
        _storesRepositoryMock.Verify(x => x.GetStoresByCityIdAsync(requestedCityId), Times.Once);
        _housesRepositoryMock.Verify(x => x.AddHouseAsync(It.Is<House>(house => house.Address == requestedAddress && house.Cityid == requestedCityId)), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public void When_UpdateHouseAsync_called_with_cityid_invalid_then_throw()
    {
        var requestedId = 1;
        var houseUpdateRequest = new HouseUpdateRequest
        {
            Address = "1 rue de Paris",
            Cityid = 1,
            Inhabitants = 1
        };

        Func<Task> response = () => _housesService.UpdateHouseAsync(requestedId, houseUpdateRequest);
        response.Should().ThrowExactlyAsync<ArgumentException>($"House with id {requestedId} not found");

        _housesRepositoryMock.Verify(x => x.GetHouseByIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(2, null)]
    [InlineData(null, "2 rue de Paris")]
    public void When_UpdateHouseAsync_called_with_address_already_used_in_city_then_throw(int? updatedCityid, string updatedAddress)
    {
        var requestedId = 1;
        var initialCityId = 1;
        var initialAddress = "1 rue de Paris";
        var requestedCityId = updatedCityid ?? initialCityId;
        var requestedAddress = updatedAddress ?? initialAddress;
        var initialHouse = new House
        {
            Id = requestedId,
            Address = initialAddress,
            Cityid = initialCityId,
            Inhabitants = 1
        };
        var houseUpdateRequest = new HouseUpdateRequest
        {
            Address = updatedAddress,
            Cityid = updatedCityid,
            Inhabitants = 1
        };
        var requestedCity = new City
        {
            Id = requestedCityId,
            Name = "Paris",
            Touristic = true
        };
        var existingHouses = new List<House>
        {
            new()
            {
                Id = 2,
                Address = requestedAddress,
                Cityid = requestedCityId,
                Inhabitants = 1
            }
        };

        _housesRepositoryMock.Setup(x => x.GetHouseByIdAsync(requestedId)).ReturnsAsync(initialHouse);
        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedCityId)).ReturnsAsync(requestedCity);
        _housesRepositoryMock.Setup(x => x.GetHousesByCityIdAsync(requestedCityId)).ReturnsAsync(existingHouses);
        _storesRepositoryMock.Setup(x => x.GetStoresByCityIdAsync(requestedCityId)).ReturnsAsync(new List<Store>());

        Func<Task> response = () => _housesService.UpdateHouseAsync(requestedId, houseUpdateRequest);
        response.Should().ThrowExactlyAsync<ArgumentException>($"Building with address {requestedAddress} already exists in this city ({requestedCityId})");

        _housesRepositoryMock.Verify(x => x.GetHouseByIdAsync(requestedId), Times.Once);
        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedCityId), Times.Once);
        _housesRepositoryMock.Verify(x => x.GetHousesByCityIdAsync(requestedCityId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(3, "3 rue de Paris", 3)]
    [InlineData(3, null, null)]
    [InlineData(null, "3 rue de Paris", null)]
    [InlineData(null, null, 3)]
    public async Task When_UpdateHouseAsync_called_with_address_available_in_city_then_return_house(int? updatedCityId, string updatedAddress, int? updatedInhabitants)
    {
        var requestedId = 1;
        var initialCityId = 1;
        var initialAddress = "1 rue de Paris";
        var initialInhbitants = 1;
        var requestedCityId = updatedCityId ?? initialCityId;
        var requestedAddress = updatedAddress ?? initialAddress;
        var requestedInhabitants = updatedInhabitants ?? initialInhbitants;
        var initialHouse = new House
        {
            Id = requestedId,
            Address = initialAddress,
            Cityid = initialCityId,
            Inhabitants = initialInhbitants
        };
        var houseUpdateRequest = new HouseUpdateRequest
        {
            Address = updatedAddress,
            Cityid = updatedCityId,
            Inhabitants = updatedInhabitants
        };
        var updatedHouse = new House
        {
            Id = requestedId,
            Address = requestedAddress,
            Cityid = requestedCityId,
            Inhabitants = requestedInhabitants
        };
        var requestedCity = new City
        {
            Id = requestedCityId,
            Name = "Paris",
            Touristic = true
        };
        var existingHouses = new List<House>
        {
            new()
            {
                Id = 2,
                Address = "2 rue de Paris",
                Cityid = 2,
                Inhabitants = 1
            }
        };
        var expectedResponse = new HouseResponse(updatedHouse);
        var checkAddress = updatedCityId is not null || updatedAddress is not null;

        _housesRepositoryMock.Setup(x => x.GetHouseByIdAsync(requestedId)).ReturnsAsync(initialHouse);
        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedCityId)).ReturnsAsync(requestedCity);
        _housesRepositoryMock.Setup(x => x.GetHousesByCityIdAsync(requestedCityId)).ReturnsAsync(existingHouses);
        _storesRepositoryMock.Setup(x => x.GetStoresByCityIdAsync(requestedCityId)).ReturnsAsync(new List<Store>());
        _housesRepositoryMock.Setup(x => x.UpdateHouseAsync(It.Is<House>(house => house.Id == requestedId))).ReturnsAsync(updatedHouse);

        var response = await _housesService.UpdateHouseAsync(requestedId, houseUpdateRequest);
        response.Should().BeEquivalentTo(expectedResponse);

        _housesRepositoryMock.Verify(x => x.GetHouseByIdAsync(requestedId), Times.Once);
        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedCityId), checkAddress ? Times.Once : Times.Never);
        _housesRepositoryMock.Verify(x => x.GetHousesByCityIdAsync(requestedCityId), checkAddress ? Times.Once : Times.Never);
        _storesRepositoryMock.Verify(x => x.GetStoresByCityIdAsync(requestedCityId), checkAddress ? Times.Once : Times.Never);
        _housesRepositoryMock.Verify(x => x.UpdateHouseAsync(It.Is<House>(house => house.Id == requestedId)), Times.Once);
        VerifyNoOtherCalls();
    }
}