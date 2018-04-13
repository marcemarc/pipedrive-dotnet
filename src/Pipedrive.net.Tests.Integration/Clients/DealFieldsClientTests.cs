﻿using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Pipedrive.Tests.Integration.Clients
{
    public class DealFieldsClientTests
    {
        public class TheGetAllMethod
        {
            [IntegrationTest]
            public async Task CanRetrieveDealTypes()
            {
                var pipedrive = Helper.GetAuthenticatedClient();

                var dealFields = await pipedrive.DealField.GetAll();

                Assert.True(dealFields.Count >= 6);
                Assert.True(dealFields[0].ActiveFlag);
                Assert.False(dealFields[0].AddVisibleFlag);
                Assert.True(dealFields[1].ActiveFlag);
                Assert.False(dealFields[1].AddVisibleFlag);
            }
        }

        public class TheGetMethod
        {
            [IntegrationTest]
            public async Task CanRetrieveDealType()
            {
                var pipedrive = Helper.GetAuthenticatedClient();

                var dealField = await pipedrive.DealField.Get(12451);

                Assert.True(dealField.ActiveFlag);
                Assert.False(dealField.AddVisibleFlag);
            }
        }

        public class TheCreateMethod
        {
            [IntegrationTest]
            public async Task CanCreate()
            {
                var pipedrive = Helper.GetAuthenticatedClient();
                var fixture = pipedrive.DealField;

                var newDealField = new NewDealField("name", FieldType.time);

                var dealField = await fixture.Create(newDealField);
                Assert.NotNull(dealField);

                var retrieved = await fixture.Get(dealField.Id.Value);
                Assert.NotNull(retrieved);
            }
        }

        public class TheEditMethod
        {
            [IntegrationTest]
            public async Task CanEdit()
            {
                var pipedrive = Helper.GetAuthenticatedClient();
                var fixture = pipedrive.DealField;

                var newDealField = new NewDealField("new-name", FieldType.varchar);
                var dealField = await fixture.Create(newDealField);

                var editActivityType = dealField.ToUpdate();
                editActivityType.Name = "updated-name";

                var updatedActivityType = await fixture.Edit(dealField.Id.Value, editActivityType);

                Assert.Equal("updated-name", updatedActivityType.Name);
                Assert.Equal(FieldType.varchar, updatedActivityType.FieldType);
            }
        }

        public class TheDeleteMethod
        {
            [IntegrationTest]
            public async Task CanDelete()
            {
                var pipedrive = Helper.GetAuthenticatedClient();
                var fixture = pipedrive.DealField;

                var newDealField = new NewDealField("new-name", FieldType.varchar_auto);
                var dealField = await fixture.Create(newDealField);

                var createdDealField = await fixture.Get(dealField.Id.Value);

                Assert.NotNull(createdDealField);

                await fixture.Delete(createdDealField.Id.Value);

                await Assert.ThrowsAsync<NotFoundException>(() => fixture.Get(createdDealField.Id.Value));
            }
        }
    }
}
