using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace caMon.pages.TIS
{
    public class caMonIF : IPages
    {
        public Page FrontPage => new Root(this);

        public event EventHandler BackToHome;
        public event EventHandler CloseApp;

        public caMonIF()
        {

        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        internal void BackToHomeDo() => BackToHome?.Invoke(null, null);
        internal void CloseAppDo() => CloseApp?.Invoke(null, null);
    }
}
