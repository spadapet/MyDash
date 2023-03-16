namespace MyDash.Data.Model;

public interface ICopyFrom<T>
{
    void CopyFrom(T other);
}
