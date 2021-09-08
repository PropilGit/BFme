using BFme.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BFme.Services
{
    public interface IFileController
    {
        bool Configure(Access access);

        byte[] Download(string name);
        bool Upload(string name, byte[] bytes);
    }
}
