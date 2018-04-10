using System;


namespace Acr
{
    public static class Application
    {
        public static void InvokeOnMainThread(Action action) => action();
    }
}
