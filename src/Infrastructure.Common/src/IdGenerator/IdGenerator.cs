namespace FavoDeMel.Infrastructure.Common.IdGenerator
{
    public class IdGenerator : IIdGenerator
    {
        private IdGen.IdGenerator _idGenerator;
        
        public IdGenerator(int generatorId)
        {
            _idGenerator = new IdGen.IdGenerator(generatorId);
        }

        public long Generate() => _idGenerator.CreateId();
    }
}