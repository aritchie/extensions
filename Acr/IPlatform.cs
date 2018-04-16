using System;


namespace Acr
{
    public interface IPlatform
    {
        void InvokeOnMainThread(Action action);
    }
}
