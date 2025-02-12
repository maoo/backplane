/**
	* SPDX-License-Identifier: Apache-2.0
	* Copyright 2021 FINOS FDC3 contributors - see NOTICE file
	*/

using AutoFixture;
using Finos.Fdc3.Backplane.Client.API;
using Finos.Fdc3.Backplane.Client.Transport;
using Finos.Fdc3.Backplane.DTO.FDC3;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Finos.Fdc3.Backplane.Client.Test.API
{
    public class DesktopAgentBackplaneClientTest
    {
        private readonly string? instrument = @"{
                type: 'fdc3.instrument',
                id: {
                    ticker: 'AAPL',
                    ISIN: 'US0378331005',
                    FIGI: 'BBG000B9XRY4',
                },
              }";

        private Fixture _fixture;
        private Lazy<IBackplaneTransport> _backplaneTransport;


        [SetUp]
        public void Setup()
        {
            _fixture = AutoFixture.Create();
            _backplaneTransport = _fixture.Freeze<Lazy<IBackplaneTransport>>();
            _backplaneTransport.Value.GetSystemChannelsAsync().Returns(new Channel[] { new Channel("channel1", "system") });
            _backplaneTransport.Value.ConnectAsync(default, default, default).ReturnsForAnyArgs(new AppIdentifier() { AppId = "Test" });
        }


        [Test]
        public void ShouldNotConnectToBackplaneOnObjectCreation()
        {
            _fixture.Create<BackplaneClient>();
            _backplaneTransport.Value.DidNotReceiveWithAnyArgs().ConnectAsync(default, default);
        }

        [Test]
        public async Task GetSystemChannelsShouldReturnPopulatedChannel()
        {
            BackplaneClient sut = _fixture.Create<BackplaneClient>();
            await sut.ConnectAsync(default, default, default);
            System.Collections.Generic.IEnumerable<Channel> channels = await sut.GetSystemChannelsAsync();
            Assert.AreEqual(channels.Count(), 1);
        }

        [Test]
        public async Task BroadcastContextShouldInvokeBroadcastOverTransport()
        {
            BackplaneClient sut = _fixture.Create<BackplaneClient>();
            await sut.ConnectAsync(default, default, default);
            await sut.BroadcastAsync(new Context(JObject.Parse(instrument)), "channel1");
            await _backplaneTransport.Value.ReceivedWithAnyArgs().BroadcastAsync(default);
        }
    }
}