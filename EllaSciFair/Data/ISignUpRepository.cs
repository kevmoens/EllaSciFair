namespace EllaSciFair.Data
{
    public interface ISignUpRepository
    {
        IList<SignUp>? GetOpenSignUps();
        IList<SignUp>? GetCompletedSignUps();
        SignUp? GetSignUp(int Id);
        int Add(SignUp signUp);
        SignUp? Update(SignUp signUp);
        void Delete(SignUp signUp);
    }
}
