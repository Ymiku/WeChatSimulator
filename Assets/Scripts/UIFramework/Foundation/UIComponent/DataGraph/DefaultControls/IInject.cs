using System.Collections.Generic;

public interface IInject
{
    void Inject<T>(IList<T> data);
    void Inject<T>(IList<T>[] datas);
}