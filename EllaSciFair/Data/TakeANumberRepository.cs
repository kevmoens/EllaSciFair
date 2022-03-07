namespace EllaSciFair.Data
{
    public class TakeANumberRepository : ITakeANumberRepository
    {
        private readonly SignUpContext signUpContext;
        public TakeANumberRepository(SignUpContext context)
        {
            signUpContext = context;
        }

        public TakeANumber? Get()
        {
            var result = signUpContext.TakeANumbers?.FirstOrDefault();
            if (result == null)
            {
                result = new TakeANumber() { Id = Guid.NewGuid(), CurrentNumber = 0 };
                if (signUpContext.TakeANumbers == null)
                {
                    return null; 
                }
                signUpContext.TakeANumbers.Add(result);
                signUpContext.SaveChanges();
            }
            return result;
        }

        public TakeANumber? Update(TakeANumber takeANumberChanges)
        {
            if (takeANumberChanges != null)
            {
                var takeANum = signUpContext.TakeANumbers?.Attach(takeANumberChanges);
                if (takeANum == null) return null;
                takeANum.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                signUpContext.SaveChanges();
                return takeANumberChanges;
            }
            return null;
        }
    }
}
