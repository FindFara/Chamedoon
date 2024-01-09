namespace Chamedoon.Domin.Base;

public class BaseResult_VM<T>
{
    public T Result { get; set; }
    public int Code { get; set; }
    public string Message { get; set; }
}
