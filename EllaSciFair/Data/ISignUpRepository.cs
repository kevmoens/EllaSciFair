namespace EllaSciFair.Data
{
    public interface ISignUpRepository
    {
        IList<SignUp>? GetOpenSignUps();
        IList<SignUp>? GetCompletedSignUps();
        SignUp? GetEmailSignup(string email);
        SignUp? GetSignUp(int Id);
        IList<SignUp>? GetPublicSignups();
        int Add(SignUp signUp);
        SignUp? Update(SignUp signUp);
        void Delete(SignUp signUp);
    }
}
