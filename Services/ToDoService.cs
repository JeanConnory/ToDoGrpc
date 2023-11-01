using Grpc.Core;
using ToDoGrpc.Data;
using ToDoGrpc.Models;
using ToDoGrpc.Protos;

namespace ToDoGrpc.Services
{
    public class ToDoService : ToDoIt.ToDoItBase
    {
        private readonly AppDbContext _dbContext;

        public ToDoService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<CreateToDoResponse> CreateToDo(CreateToDoRequest request, ServerCallContext context)
        {
            if (request.Title == string.Empty || request.Description == string.Empty)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "You must supply a valid object"));

            var toDoItem = new ToDoItem
            {
                Title = request.Title,
                Description = request.Description
            };

            await _dbContext.AddAsync(toDoItem);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(new CreateToDoResponse
            {
                Id = toDoItem.Id
            });
        }


    }
}
