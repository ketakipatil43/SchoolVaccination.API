using UM.Core;
using UM.Data.EFData.Interfaces;

namespace UM.Data.EFData.Repositories
{
    public class CreateAccountRepo : ICreateAccountRepo
    {
        public UmContext _dbContext { get; }

        public CreateAccountRepo(UmContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
