using System.Linq;

namespace EllaSciFair.Data
{
    public class SignUpRepository : ISignUpRepository
    {
        private readonly SignUpContext signUpContext;
        public SignUpRepository(SignUpContext context)
        {
            signUpContext = context;
        }

        public SignUp? GetEmailSignup(string email)
        {
            return signUpContext.SignUps?
                .Where(r => r.Email != null && r.Email.ToUpper() == email.ToUpper())
                .FirstOrDefault();
        }

        public int Add(SignUp signUp)
        {
            int? maxId = signUpContext.SignUps?.OrderByDescending( s => s.Id).FirstOrDefault()?.Id;
            if (maxId == null)
            {
                maxId = 0;
            }
            maxId++;
            signUp.Id = maxId.Value;
            signUpContext.Add(signUp);
            signUpContext.SaveChanges();
            return maxId.Value;
        }

        public void Delete(SignUp signUp)
        {
            signUpContext.SignUps?.Remove(signUp);
            signUpContext.SaveChanges();
        }

        public IList<SignUp>? GetOpenSignUps()
        {
            return signUpContext.SignUps?
                .Where(r =>  string.IsNullOrEmpty(r.FileName))
                .OrderBy(r => r.Id)
                .ToList();
        }

        public IList<SignUp>? GetPublicSignups()
        {
            return signUpContext.SignUps?
                .Where(r => r.IsPublic)
                .OrderBy(r => r.Id)
                .ToList();
        }

        public SignUp? GetSignUp(int Id)
        {
            return signUpContext.SignUps?.FirstOrDefault(x => x.Id == Id);
        }

        public SignUp? Update(SignUp signUpChanges)
        {
            var signUp = signUpContext.SignUps?.Attach(signUpChanges);
            if (signUp == null) return null;
            signUp.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            signUpContext.SaveChanges();
            return signUpChanges;
        }
    }
}