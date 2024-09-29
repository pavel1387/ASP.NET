using AutoFixture.AutoMoq;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Controllers;
using PromoCodeFactory.WebHost.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace PromoCodeFactory.UnitTests.WebHost.Controllers.Partners
{
    public class SetPartnerPromoCodeLimitAsyncTests
    {
        private readonly Mock<IRepository<Partner>> _partnersRepositoryMock;
        private readonly PartnersController _partnersController;

        public SetPartnerPromoCodeLimitAsyncTests()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());

            _partnersRepositoryMock = fixture.Freeze<Mock<IRepository<Partner>>>();

            _partnersController = fixture
                .Build<PartnersController>()
                .OmitAutoProperties()
                .Create();
        }

        public Partner CreateBasePartner()
        {
            var partner = new Partner()
            {
                Id = Guid.Parse("7d994823-8226-4273-b063-1a95f3cc1df8"),
                Name = "Суперигрушки",
                IsActive = true,
                PartnerLimits = new List<PartnerPromoCodeLimit>()
            };

            return partner;
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsNotFound_ReturnsNotFound()
        {
            // Arrange
            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(null as Partner);

            var request = new Fixture().Create<SetPartnerPromoCodeLimitRequest>();

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(It.IsAny<Guid>(), request);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task SetPartnerPromoCodeLimitAsync_PartnerIsLocked_ReturnsBadRequest()
        {
            // Arrange
            var fixture = new Fixture();

            var partner = fixture.Build<Partner>()
                .With(x => x.IsActive, false)
                .Without(x => x.PartnerLimits)
                .Create();

            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture.Create<SetPartnerPromoCodeLimitRequest>();

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_WhenSettingNewLimit_NumberIssuedPromoCodesIsReset()
        {
            // Arrange
            var fixture = new Fixture();

            var partner = CreateBasePartner();

            partner.NumberIssuedPromoCodes = 5;

            var newLimit = fixture.Build<PartnerPromoCodeLimit>()
                .With(x => x.CancelDate, DateTime.Now)
                .With(x => x.Partner, partner)
                .Create();

            partner.PartnerLimits.Add(newLimit);

            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture.Create<SetPartnerPromoCodeLimitRequest>();

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);
            var result = await _partnersRepositoryMock.Object.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.NumberIssuedPromoCodes.Should().Be(5);
        }


        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_WhenPreviousLimitIsExpired_NumberIssuedPromoCodesIsNotReset()
        {
            // Arrange
            var fixture = new Fixture();

            var partner = CreateBasePartner();

            partner.NumberIssuedPromoCodes = 5;

            var newLimit = fixture.Build<PartnerPromoCodeLimit>()
                .With(x => x.Partner, partner)
                .Without(x => x.CancelDate)
                .Create();

            partner.PartnerLimits.Add(newLimit);

            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture.Create<SetPartnerPromoCodeLimitRequest>();

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);
            var result = await _partnersRepositoryMock.Object.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.NumberIssuedPromoCodes.Should().Be(0);
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_WhenSettingNewLimit_PreviousLimitIsCancelled()
        {
            // Arrange
            var fixture = new Fixture();

            var partner = CreateBasePartner();

            var newLimit = fixture.Build<PartnerPromoCodeLimit>()
                .With(x => x.Partner, partner)
                .Without(x => x.CancelDate)
                .Create();

            partner.PartnerLimits.Add(newLimit);

            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture.Create<SetPartnerPromoCodeLimitRequest>();

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);
            var result = await _partnersRepositoryMock.Object.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.PartnerLimits.FirstOrDefault(x => x.CancelDate.HasValue)
                .Should().NotBeNull();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_LimitBelowZero_ReturnsBadRequest()
        {
            // Arrange
            var fixture = new Fixture();

            var partner = CreateBasePartner();

            _partnersRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture
                .Build<SetPartnerPromoCodeLimitRequest>()
                .With(x => x.Limit, -2)
                .Create();

            // Act
            var result = await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);

            // Assert
            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Fact]
        public async void SetPartnerPromoCodeLimitAsync_WhenSettingNewLimit_NewLimitIsSavedInDatabase()
        {
            // Arrange
            var fixture = new Fixture();

            var partner = CreateBasePartner();

            _partnersRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(partner);

            var request = fixture.Create<SetPartnerPromoCodeLimitRequest>();

            // Act
            await _partnersController.SetPartnerPromoCodeLimitAsync(partner.Id, request);
            var result = await _partnersRepositoryMock.Object.GetByIdAsync(It.IsAny<Guid>());

            // Assert
            result.PartnerLimits.FirstOrDefault(x => !x.CancelDate.HasValue).Should().NotBeNull();
        }
    }
}