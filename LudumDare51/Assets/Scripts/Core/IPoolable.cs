using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPoolable
{
    bool CanBeTakenFromPool();
    void CleanupForPooling();
}