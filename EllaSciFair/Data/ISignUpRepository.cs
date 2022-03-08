namespace EllaSciFair.Data
{
    public interface ISignUpRepository
    {
        IList<SignUp>? GetOpenSignUps();
        SignUp? GetSignUp(int Id);
        int Add(SignUp signUp);
        SignUp? Update(SignUp signUp);
        void Delete(SignUp signUp);
    }
}
