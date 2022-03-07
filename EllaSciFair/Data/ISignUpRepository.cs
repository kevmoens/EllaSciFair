namespace EllaSciFair.Data
{
    public interface ISignUpRepository
    {
        IList<SignUp>? GetSignUps();
        SignUp? GetSignUp(int Id);
        void Add(SignUp signUp);
        SignUp? Update(SignUp signUp);
        void Delete(SignUp signUp);
    }
}
