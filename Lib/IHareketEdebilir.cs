namespace UcakSavarOyunu.Lib
{
    public interface IHareketEdebilir
    {
        void HareketEt(Yonler yon);
    }
    public enum Yonler
    {
        Yukari,
        Asagi,
        Sola,
        Saga
    }
}
