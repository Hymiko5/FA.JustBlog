using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FA.JustBlog.Core.Infrastructure
{
    public class Disposable : IDisposable
    {
        public bool _isDisposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~Disposable()
        {
            Dispose(false);
        }
        public void Dispose(bool disposing)
        {
            if (!_isDisposed && disposing)
            {
                DisposeCore();
            }
            _isDisposed = true;
        }
        protected virtual void DisposeCore() { }
    }
}
