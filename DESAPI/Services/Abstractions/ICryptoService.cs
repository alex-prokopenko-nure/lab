using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DESAPI.Services.Abstractions
{
    public interface ICryptoService
    {
        string Encrypt(string input, string keyString);
        string Decrypt(string input, string keyString);
    }
}
