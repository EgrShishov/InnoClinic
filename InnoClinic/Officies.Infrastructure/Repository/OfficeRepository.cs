using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Officies.Infrastructure.Repository
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly IMongoCollection<Office> _offices;

        public OfficeRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("OfficesDb");
            _offices = database.GetCollection<Office>("Offices");
        }

        public async Task<List<Office>> GetOfficesAsync()
        {
            return await _offices.Find(office => true).ToListAsync();
        }

        public async Task<Office> GetOfficeByIdAsync(string id)
        {
            return await _offices.Find<Office>(office => office.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddOfficeAsync(Office office)
        {
            await _offices.InsertOneAsync(office);
        }

        public async Task UpdateOfficeAsync(Office office)
        {
            await _offices.ReplaceOneAsync(o => o.Id == office.Id, office);
        }

        public async Task DeleteOfficeAsync(string id)
        {
            await _offices.DeleteOneAsync(office => office.Id == id);
        }

        public Task<List<Office>> ListOfficesAsync(Expression<Func<Office, bool>> filter)
        {
            var query = _offices.AsQueryable();

            if(filter != null)
            {
                query = query.Where(filter);
            }

            return query.ToListAsync();
        }
    }
}
