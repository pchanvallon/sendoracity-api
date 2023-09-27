using FluentAssertions;
using Moq;
using SendoraCityApi.Repositories.Database.Models;
using SendoraCityApi.Services;
using SendoraCityApi.Services.Models;
using Xunit;

namespace SendoraCityApi.Tests;

public class CitiesServiceTests : ServiceTests
{

    private readonly ICitiesService _citiesService;

    public CitiesServiceTests()
        => _citiesService = new CitiesService(
            _citiesRepositoryMock.Object,
            _housesRepositoryMock.Object,
            _storesRepositoryMock.Object
            )
        { };

    [Fact]
    public void When_GetCityByIdAsync_called_with_id_invalid_then_throw()
    {
        var requestedId = 2;
        var returnedCity = new City
        {
            Id = 1,
            Name = "Paris",
            Touristic = true,
        };
        var expectedResponse = new CityResponse(returnedCity);

        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedId)).ReturnsAsync(returnedCity);

        Func<Task> response = () => _citiesService.GetCityByIdAsync(requestedId);
        response.Should().ThrowExactlyAsync<ArgumentOutOfRangeException>($"City with id {requestedId} not found");

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public async Task When_GetCityByIdAsync_called_with_id_valid_then_return_city()
    {
        var requestedId = 1;
        var returnedCity = new City
        {
            Id = 1,
            Name = "Paris",
            Touristic = true,
        };
        var expectedResponse = new CityResponse(returnedCity);

        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedId)).ReturnsAsync(returnedCity);

        var response = await _citiesService.GetCityByIdAsync(requestedId);
        response.Should().BeEquivalentTo(expectedResponse);

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public void When_AddCityAsync_called_with_name_already_used_then_throw()
    {
        var requestedCityName = "Paris";
        var cityCreateRequest = new CityCreateRequest
        {
            Name = requestedCityName,
            Touristic = true,
        };
        var requestedCity = new City
        {
            Name = requestedCityName,
            Touristic = true,
        };
        var existingCities = new List<City>()
        {
            new() {
                Name = requestedCityName,
                Touristic = true,
            }
        };
        var expectedResponse = new CityResponse(requestedCity);

        _citiesRepositoryMock.Setup(x => x.GetCitiesAsync()).ReturnsAsync(existingCities);

        Func<Task> response = () => _citiesService.AddCityAsync(cityCreateRequest);
        response.Should().ThrowExactlyAsync<InvalidOperationException>($"City with name {requestedCityName} already exists");

        _citiesRepositoryMock.Verify(x => x.GetCitiesAsync(), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData("Paris", true)]
    [InlineData("Detroit", false)]
    public async Task When_AddCityAsync_called_with_name_available_then_return_city(string name, bool touristic)
    {
        var cityCreateRequest = new CityCreateRequest
        {
            Name = name,
            Touristic = touristic,
        };
        var returnedCity = new City
        {
            Name = name,
            Touristic = touristic,
        };
        var existingCities = new List<City>();
        var expectedResponse = new CityResponse(returnedCity);

        _citiesRepositoryMock.Setup(x => x.GetCitiesAsync()).ReturnsAsync(existingCities);
        _citiesRepositoryMock.Setup(x => x.AddCityAsync(It.Is<City>(city => city.Name == name))).ReturnsAsync(returnedCity);

        var response = await _citiesService.AddCityAsync(cityCreateRequest);
        response.Should().BeEquivalentTo(expectedResponse);

        _citiesRepositoryMock.Verify(x => x.GetCitiesAsync(), Times.Once);
        _citiesRepositoryMock.Verify(x => x.AddCityAsync(It.Is<City>(city => city.Name == name)), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public void When_UpdateCityAsync_called_with_id_invalid_then_throw()
    {
        var requestedId = 1;
        var cityUpdateRequest = new CityUpdateRequest
        {
            Name = "Paris",
            Touristic = false,
        };

        Func<Task> response = () => _citiesService.UpdateCityAsync(requestedId, cityUpdateRequest);
        response.Should().ThrowExactlyAsync<ArgumentOutOfRangeException>($"City with id {requestedId} not found");

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();

    }

    [Theory]
    [InlineData("Detroit", false)]
    [InlineData("Detroit", null)]
    [InlineData(null, false)]
    public async Task When_UpdateCityAsync_called_with_id_valid_then_return_city(string updatedName, bool? updatedTouristic)
    {
        var requestedId = 1;
        var initialName = "Paris";
        var initialTouristic = true;
        var cityUpdateRequest = new CityUpdateRequest
        {
            Name = updatedName,
            Touristic = updatedTouristic,
        };
        var initialCity = new City
        {
            Id = 1,
            Name = initialName,
            Touristic = initialTouristic,
        };
        var updatedCity = new City
        {
            Name = updatedName ?? initialName,
            Touristic = updatedTouristic ?? initialTouristic,
        };
        var expectedResponse = new CityResponse(updatedCity);

        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedId)).ReturnsAsync(initialCity);
        _citiesRepositoryMock.Setup(x => x.UpdateCityAsync(It.Is<City>(city => city.Id == requestedId))).ReturnsAsync(updatedCity);

        var response = await _citiesService.UpdateCityAsync(requestedId, cityUpdateRequest);
        response.Should().BeEquivalentTo(expectedResponse);

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedId), Times.Once);
        _citiesRepositoryMock.Verify(x => x.UpdateCityAsync(It.Is<City>(city => city.Id == requestedId)), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public void When_DeleteCityAsync_called_with_id_invalid_then_throw()
    {
        var requestedId = 1;

        Func<Task> response = () => _citiesService.DeleteCityAsync(requestedId);
        response.Should().ThrowExactlyAsync<ArgumentOutOfRangeException>($"City with id {requestedId} not found");

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(2, 0)]
    [InlineData(2, 2)]
    [InlineData(0, 2)]
    public void When_DeleteCityAsync_called_with_city_referenced_then_throw(int houseCount, int storeCount)
    {
        var requestedId = 1;
        var existingCity = new City
        {
            Id = 1,
            Name = "Paris",
            Touristic = true,
        };
        var expectedResponse = new CityResponse(existingCity);
        var linkedHouses = Enumerable.Range(0, houseCount).Select(idx => new House
        {
            Id = idx + 1,
            Address = $"{idx + 1} rue de Paris",
            Cityid = requestedId,
            Inhabitants = 2
        });
        var linkedStores = Enumerable.Range(0, storeCount).Select(idx => new Store
        {
            Id = idx + 1,
            Address = $"{idx + 1} rue de Rennes",
            Cityid = requestedId,
            Name = $"Store {idx + 1}",
            Type = "Other"
        });

        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedId)).ReturnsAsync(existingCity);
        _housesRepositoryMock.Setup(x => x.GetHousesByCityIdAsync(requestedId)).ReturnsAsync(linkedHouses);
        _storesRepositoryMock.Setup(x => x.GetStoresByCityIdAsync(requestedId)).ReturnsAsync(linkedStores);

        Func<Task> response = () => _citiesService.DeleteCityAsync(requestedId);
        response.Should().ThrowExactlyAsync<InvalidOperationException>($"City with id {requestedId} still has buildings");

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedId), Times.Once);
        _housesRepositoryMock.Verify(x => x.GetHousesByCityIdAsync(requestedId), Times.Once);
        _storesRepositoryMock.Verify(x => x.GetStoresByCityIdAsync(requestedId), houseCount > 0 ? Times.Never : Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public async Task When_DeleteCityAsync_called_with_id_valid_and_city_not_referenced_then_return_city()
    {
        var requestedId = 1;
        var existingCity = new City
        {
            Id = 1,
            Name = "Paris",
            Touristic = true,
        };
        var expectedResponse = new CityResponse(existingCity);

        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedId)).ReturnsAsync(existingCity);
        _citiesRepositoryMock.Setup(x => x.DeleteCityAsync(It.Is<City>(city => city.Id == requestedId))).ReturnsAsync(existingCity);
        _housesRepositoryMock.Setup(x => x.GetHousesByCityIdAsync(requestedId)).ReturnsAsync(new List<House>());
        _storesRepositoryMock.Setup(x => x.GetStoresByCityIdAsync(requestedId)).ReturnsAsync(new List<Store>());

        var response = await _citiesService.DeleteCityAsync(requestedId);
        response.Should().BeEquivalentTo(expectedResponse);

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedId), Times.Once);
        _citiesRepositoryMock.Verify(x => x.DeleteCityAsync(It.Is<City>(city => city.Id == requestedId)), Times.Once);
        _housesRepositoryMock.Verify(x => x.GetHousesByCityIdAsync(requestedId), Times.Once);
        _storesRepositoryMock.Verify(x => x.GetStoresByCityIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();
    }
}