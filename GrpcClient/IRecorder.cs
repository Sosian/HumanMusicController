public interface IRecorder<T>
{
    void ReceiveData(T data);
}