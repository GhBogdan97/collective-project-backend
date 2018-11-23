using System;

namespace DatabaseAccess.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
