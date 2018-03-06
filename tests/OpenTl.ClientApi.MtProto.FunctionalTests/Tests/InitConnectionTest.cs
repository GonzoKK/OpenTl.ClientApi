﻿namespace OpenTl.ClientApi.MtProto.FunctionalTests.Tests
{
    using System.Threading.Tasks;

    using OpenTl.ClientApi.MtProto.FunctionalTests.Framework;
    using OpenTl.Schema.Help;

    using Xunit;
    using Xunit.Abstractions;

    public sealed class InitConnectionTest : FunctionalTest
    {
        public InitConnectionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetConfig()
        {
            var result = await PackageSender.SendRequest(new RequestGetConfig()).ConfigureAwait(false);
            
            Assert.NotNull(result);
            Assert.NotEmpty(result.DcOptions.Items);
        }

        protected override void Init()
        {
        }
    }
}