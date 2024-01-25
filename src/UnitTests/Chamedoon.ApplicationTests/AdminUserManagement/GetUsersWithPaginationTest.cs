using Xunit;
using Chamedoon.Application.Services.Admin.UserManagement.Query;
using MediatR;

namespace Chamedoon.ApplicationTests.AdminUserManagement;

internal class GetUsersWithPaginationTest
{
    [Fact]
    public async Task returntrue()
    {
        var request = new GetAllUsersWithPaginationQuery
        {

        };
      
    }

}
