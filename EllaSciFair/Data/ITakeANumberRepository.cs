namespace EllaSciFair.Data
{
    public interface ITakeANumberRepository
    {
        TakeANumber? Get();
        TakeANumber? Update(TakeANumber takeANumber);

    }
}
