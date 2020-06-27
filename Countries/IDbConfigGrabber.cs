using System;

namespace Countries
{
    interface IDbConfigGrabber
    {
        String GetConnectionString(string fileName);
    }
}
