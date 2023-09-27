using Moq;
using SendoraCityApi.Repositories;

namespace SendoraCityApi.Tests;

public class ServiceTests
{
    protected readonly Mock<ICitiesRepository> _citiesRepositoryMock = new();
    protected readonly Mock<IHousesRepository> _housesRepositoryMock = new();
    protected readonly Mock<IStoresRepository> _storesRepositoryMock = new();

    protected void VerifyNoOtherCalls()
    {
        _citiesRepositoryMock.VerifyNoOtherCalls();
        _housesRepositoryMock.VerifyNoOtherCalls();
        _storesRepositoryMock.VerifyNoOtherCalls();
    }
}