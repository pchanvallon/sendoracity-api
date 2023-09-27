using FluentAssertions;
using Moq;
using SendoraCityApi.Repositories.Database.Models;
using SendoraCityApi.Services;
using SendoraCityApi.Services.Models;
using Xunit;

namespace SendoraCityApi.Tests;

public class StoresServiceTests : ServiceTests
{

    private readonly IStoresService _storesService;

    public StoresServiceTests()
        => _storesService = new StoresService(
            _citiesRepositoryMock.Object,
            _housesRepositoryMock.Object,
            _storesRepositoryMock.Object
            )
        { };

    [Fact]
    public void When_GetStoreByIdAsync_called_with_id_invalid_then_throw()
    {
        var requestedId = 2;
        var returnedStore = new Store
        {
            Id = 1,
            Address = "1 rue de Paris",
            Cityid = 1,
            Name = "Store 1",
            Type = "Other"
        };
        var expectedResponse = new StoreResponse(returnedStore);

        _storesRepositoryMock.Setup(x => x.GetStoreByIdAsync(requestedId)).ReturnsAsync(returnedStore);

        Func<Task> response = () => _storesService.GetStoreByIdAsync(requestedId);
        response.Should().ThrowExactlyAsync<ArgumentOutOfRangeException>($"Store with id {requestedId} not found");

        _storesRepositoryMock.Verify(x => x.GetStoreByIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public async Task When_GetStoreByIdAsync_called_with_id_valid_then_returns_store()
    {
        var requestedId = 1;
        var returnedStore = new Store
        {
            Id = 1,
            Address = "1 rue de Paris",
            Cityid = 1,
            Name = "Store 1",
            Type = "Other"
        };
        var expectedResponse = new StoreResponse(returnedStore);

        _storesRepositoryMock.Setup(x => x.GetStoreByIdAsync(requestedId)).ReturnsAsync(returnedStore);

        var response = await _storesService.GetStoreByIdAsync(requestedId);
        response.Should().BeEquivalentTo(expectedResponse);

        _storesRepositoryMock.Verify(x => x.GetStoreByIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public void When_AddStoreAsync_called_with_cityid_invalid_then_throw()
    {
        var requestedCityId = 1;
        var storeCreateRequest = new StoreCreateRequest
        {
            Address = "1 rue de Paris",
            Cityid = 1,
            Name = "Store 1",
            Type = "Other"
        };

        Func<Task> response = () => _storesService.AddStoreAsync(storeCreateRequest);
        response.Should().ThrowExactlyAsync<ArgumentException>($"City with id {requestedCityId} not found");

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedCityId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void When_AddStoreAsync_called_with_address_already_used_in_city_then_throw(bool house, bool store)
    {
        var requestedCityId = 1;
        var requestedAddress = "1 rue de Paris";
        var storeCreateRequest = new StoreCreateRequest
        {
            Address = requestedAddress,
            Cityid = requestedCityId,
            Name = "Store 1",
            Type = "Other"
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

        Func<Task> response = () => _storesService.AddStoreAsync(storeCreateRequest);
        response.Should().ThrowExactlyAsync<ArgumentException>($"Building with address {requestedAddress} already exists in this city ({requestedCityId})");

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedCityId), Times.Once);
        _housesRepositoryMock.Verify(x => x.GetHousesByCityIdAsync(requestedCityId), Times.Once);
        _storesRepositoryMock.Verify(x => x.GetStoresByCityIdAsync(requestedCityId), house ? Times.Never : Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public async Task When_AddStoreAsync_called_with_address_available_in_city_then_return_store()
    {
        var requestedCityId = 1;
        var requestedAddress = "1 rue de Paris";
        var storeCreateRequest = new StoreCreateRequest
        {
            Address = requestedAddress,
            Cityid = requestedCityId,
            Name = "Store 1",
            Type = "Other"
        };
        var expectedStore = new Store
        {
            Id = 1,
            Address = requestedAddress,
            Cityid = requestedCityId,
            Name = "Store 1",
            Type = "Other"
        };
        var requestedCity = new City
        {
            Id = requestedCityId,
            Name = "Paris",
            Touristic = true
        };
        var expectedResponse = new StoreResponse(expectedStore);

        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedCityId)).ReturnsAsync(requestedCity);
        _housesRepositoryMock.Setup(x => x.GetHousesByCityIdAsync(requestedCityId)).ReturnsAsync(new List<House>());
        _storesRepositoryMock.Setup(x => x.GetStoresByCityIdAsync(requestedCityId)).ReturnsAsync(new List<Store>());
        _storesRepositoryMock.Setup(x => x.AddStoreAsync(It.Is<Store>(store => store.Address == requestedAddress && store.Cityid == requestedCityId))).ReturnsAsync(expectedStore);

        var response = await _storesService.AddStoreAsync(storeCreateRequest);
        response.Should().BeEquivalentTo(expectedResponse);

        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedCityId), Times.Once);
        _housesRepositoryMock.Verify(x => x.GetHousesByCityIdAsync(requestedCityId), Times.Once);
        _storesRepositoryMock.Verify(x => x.GetStoresByCityIdAsync(requestedCityId), Times.Once);
        _storesRepositoryMock.Verify(x => x.AddStoreAsync(It.Is<Store>(store => store.Address == requestedAddress && store.Cityid == requestedCityId)), Times.Once);
        VerifyNoOtherCalls();
    }

    [Fact]
    public void When_UpdateStoreAsync_called_with_cityid_invalid_then_throw()
    {
        var requestedId = 1;
        var storeUpdateRequest = new StoreUpdateRequest
        {
            Address = "1 rue de Paris",
            Cityid = 1,
            Name = "Store 1",
            Type = "Other"
        };

        Func<Task> response = () => _storesService.UpdateStoreAsync(requestedId, storeUpdateRequest);
        response.Should().ThrowExactlyAsync<ArgumentException>($"Store with id {requestedId} not found");

        _storesRepositoryMock.Verify(x => x.GetStoreByIdAsync(requestedId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(2, null)]
    [InlineData(null, "2 rue de Paris")]
    public void When_UpdateStoreAsync_called_with_address_already_used_in_city_then_throw(int? updatedCityid, string updatedAddress)
    {
        var requestedId = 1;
        var initialCityId = 1;
        var initialAddress = "1 rue de Paris";
        var requestedCityId = updatedCityid ?? initialCityId;
        var requestedAddress = updatedAddress ?? initialAddress;
        var initialStore = new Store
        {
            Id = requestedId,
            Address = initialAddress,
            Cityid = initialCityId,
            Name = "Store 1",
            Type = "Other"
        };
        var storeUpdateRequest = new StoreUpdateRequest
        {
            Address = updatedAddress,
            Cityid = updatedCityid,
            Name = "Store 1",
            Type = "Other"
        };
        var requestedCity = new City
        {
            Id = requestedCityId,
            Name = "Paris",
            Touristic = true
        };
        var existingStores = new List<Store>
        {
            new()
            {
                Id = 2,
                Address = requestedAddress,
                Cityid = requestedCityId,
                Name = "Store 2",
                Type = "Other"
            }
        };

        _storesRepositoryMock.Setup(x => x.GetStoreByIdAsync(requestedId)).ReturnsAsync(initialStore);
        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedCityId)).ReturnsAsync(requestedCity);
        _housesRepositoryMock.Setup(x => x.GetHousesByCityIdAsync(requestedCityId)).ReturnsAsync(new List<House>());
        _storesRepositoryMock.Setup(x => x.GetStoresByCityIdAsync(requestedCityId)).ReturnsAsync(existingStores);

        Func<Task> response = () => _storesService.UpdateStoreAsync(requestedId, storeUpdateRequest);
        response.Should().ThrowExactlyAsync<ArgumentException>($"Building with address {requestedAddress} already exists in this city ({requestedCityId})");

        _storesRepositoryMock.Verify(x => x.GetStoreByIdAsync(requestedId), Times.Once);
        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedCityId), Times.Once);
        _housesRepositoryMock.Verify(x => x.GetHousesByCityIdAsync(requestedCityId), Times.Once);
        _storesRepositoryMock.Verify(x => x.GetStoresByCityIdAsync(requestedCityId), Times.Once);
        VerifyNoOtherCalls();
    }

    [Theory]
    [InlineData(3, "3 rue de Paris", "Store 3", "Food")]
    [InlineData(3, null, null, null)]
    [InlineData(null, "3 rue de Paris", null, null)]
    [InlineData(null, null, "Store 3", null)]
    [InlineData(null, null, null, "Food")]
    public async Task When_UpdateStoreAsync_called_with_address_available_in_city_then_return_store(
        int? updatedCityId,
        string updatedAddress,
        string updatedName,
        string updatedType
    )
    {
        var requestedId = 1;
        var initialCityId = 1;
        var initialAddress = "1 rue de Paris";
        var initialName = "Store 1";
        var initialType = "Other";
        var requestedCityId = updatedCityId ?? initialCityId;
        var requestedAddress = updatedAddress ?? initialAddress;
        var requestedName = updatedName ?? initialName;
        var requestedType = updatedType ?? initialType;
        var initialStore = new Store
        {
            Id = requestedId,
            Address = initialAddress,
            Cityid = initialCityId,
            Name = initialName,
            Type = initialType,
        };
        var storeUpdateRequest = new StoreUpdateRequest
        {
            Address = updatedAddress,
            Cityid = updatedCityId,
            Name = updatedName,
            Type = updatedType,
        };
        var updatedStore = new Store
        {
            Id = requestedId,
            Address = requestedAddress,
            Cityid = requestedCityId,
            Name = requestedName,
            Type = requestedType,
        };
        var requestedCity = new City
        {
            Id = requestedCityId,
            Name = "Paris",
            Touristic = true
        };
        var existingStores = new List<Store>
        {
            new()
            {
                Id = 2,
                Address = "2 rue de Paris",
                Cityid = 2,
                Name = "Store 2",
                Type = "Other",
            }
        };
        var expectedResponse = new StoreResponse(updatedStore);
        var checkAddress = updatedCityId is not null || updatedAddress is not null;

        _storesRepositoryMock.Setup(x => x.GetStoreByIdAsync(requestedId)).ReturnsAsync(initialStore);
        _citiesRepositoryMock.Setup(x => x.GetCityByIdAsync(requestedCityId)).ReturnsAsync(requestedCity);
        _housesRepositoryMock.Setup(x => x.GetHousesByCityIdAsync(requestedCityId)).ReturnsAsync(new List<House>());
        _storesRepositoryMock.Setup(x => x.GetStoresByCityIdAsync(requestedCityId)).ReturnsAsync(existingStores);
        _storesRepositoryMock.Setup(x => x.UpdateStoreAsync(It.Is<Store>(store => store.Id == requestedId))).ReturnsAsync(updatedStore);

        var response = await _storesService.UpdateStoreAsync(requestedId, storeUpdateRequest);
        response.Should().BeEquivalentTo(expectedResponse);

        _storesRepositoryMock.Verify(x => x.GetStoreByIdAsync(requestedId), Times.Once);
        _citiesRepositoryMock.Verify(x => x.GetCityByIdAsync(requestedCityId), checkAddress ? Times.Once : Times.Never);
        _housesRepositoryMock.Verify(x => x.GetHousesByCityIdAsync(requestedCityId), checkAddress ? Times.Once : Times.Never);
        _storesRepositoryMock.Verify(x => x.GetStoresByCityIdAsync(requestedCityId), checkAddress ? Times.Once : Times.Never);
        _storesRepositoryMock.Verify(x => x.UpdateStoreAsync(It.Is<Store>(store => store.Id == requestedId)), Times.Once);
        VerifyNoOtherCalls();
    }
}