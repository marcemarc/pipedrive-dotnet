﻿using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pipedrive.Tests.Integration.Clients
{
    public class PipelinesClientTests
    {
        public class TheGetAllMethod
        {
            [IntegrationTest]
            public async Task CanRetrievePipelines()
            {
                var pipedrive = Helper.GetAuthenticatedClient();

                var pipelines = await pipedrive.Pipeline.GetAll();

                Assert.True(pipelines.Count >= 1);
                Assert.True(pipelines[0].Active);
                Assert.False(pipelines[0].Selected);
                Assert.True(pipelines[1].Active);
                Assert.False(pipelines[1].Selected);
            }
        }

        public class TheCreateMethod
        {
            [IntegrationTest]
            public async Task CanCreate()
            {
                var pipedrive = Helper.GetAuthenticatedClient();
                var fixture = pipedrive.Pipeline;

                var newPipeline = new NewPipeline("name");
                newPipeline.Active = true;

                var pipeline = await fixture.Create(newPipeline);
                Assert.NotNull(pipeline);

                var retrievedAll = await fixture.GetAll();
                var retrieved = retrievedAll.Where(ac => ac.Name == "name").FirstOrDefault();
                Assert.NotNull(retrieved);
            }
        }

        public class TheEditMethod
        {
            [IntegrationTest]
            public async Task CanEdit()
            {
                var pipedrive = Helper.GetAuthenticatedClient();
                var fixture = pipedrive.Pipeline;

                var newPipeline = new NewPipeline("new-name");
                var pipeline = await fixture.Create(newPipeline);

                var editPipeline = pipeline.ToUpdate();
                editPipeline.Name = "updated-name";
                editPipeline.Active = true;

                var updatedPipeline = await fixture.Edit(pipeline.Id, editPipeline);

                Assert.Equal("updated-name", updatedPipeline.Name);
                Assert.True(updatedPipeline.Active);
            }
        }

        public class TheDeleteMethod
        {
            [IntegrationTest]
            public async Task CanDelete()
            {
                var pipedrive = Helper.GetAuthenticatedClient();
                var fixture = pipedrive.Pipeline;

                var newPipeline = new NewPipeline("new-name");
                newPipeline.Active = true;
                var pipeline = await fixture.Create(newPipeline);

                var createdPipeline = await fixture.Get(pipeline.Id);

                Assert.NotNull(createdPipeline);

                await fixture.Delete(createdPipeline.Id);

                var deletedPipeline = await fixture.Get(pipeline.Id);

                Assert.False(deletedPipeline.Active);
            }
        }
    }
}